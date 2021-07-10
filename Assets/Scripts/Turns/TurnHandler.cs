using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Keywords;
using Characters.Stats;
using Battles.UI;
namespace Battles.Turns
{
    public interface IEventHandler {
        void SubscribeEvents();
        void UnSubscribeEvents();
     }
    public enum TurnState
    {
        NotInBattle,
        Startbattle,
        StartPlayerTurn,
        PlayerTurn,
        EndPlayerTurn,
        StartEnemyTurn,
        EnemyTurn,
        EndEnemyTurn,
        EndBattle
    };

    public class TurnHandler :MonoSingleton<TurnHandler>, IEventHandler
    {
        #region Fields
        private Dictionary<TurnState, Turn> _turnDict;
        private static TurnState _currectState;

        [SerializeField] AnimatorController _playerControler;

        public delegate void TurnEvent(TurnState state);
        public static TurnEvent SetStateEvent;

        private  bool _isTurnFinished;
        public  bool IsTurnFinished
        {
            get => _isTurnFinished; set
            {
                if (_isTurnFinished != value)
                {
                    _isTurnFinished = value;
                }
            } 
        }
        #endregion

        #region Private Methods
        private static TurnState CurrentState
        {

            get => _currectState;
            set
            {
                if (_currectState != value)
                {
                    _currectState = value;
                    Instance.StartTurn();
                }

            }
        }
        private static void SetState(TurnState moveTo)
        {
            Debug.Log("Previous turn was: " + CurrentState);
            CurrentState = moveTo;
            Debug.Log("Current Turn Is: " + CurrentState );
        }
        private Turn GetCurrentTurn()
        {
            return _turnDict[CurrentState];
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        #endregion

        #region Public Methods
        public void StartTurn()
        {
            if (GetCurrentTurn() == null)
                return;
          
            StartCoroutine(GetCurrentTurn().PlayTurn());
        }
        public void ResetTurnHandler() { 
         CurrentState = TurnState.NotInBattle;
        }
        public void PlayerEndTurn()
        {
            if (CurrentState == TurnState.PlayerTurn)
            {
                
              CurrentState = TurnState.EndPlayerTurn;
            }
        }

        public void FinishedExecution()
        {
            // need 
            IsTurnFinished = true;
        }
        public void EnemyEndTurn()
            => CurrentState = TurnState.EndEnemyTurn;
        public void BattleEnded()
        {
            CurrentState = TurnState.EndBattle;
            StopAllCoroutines();
        }
        public void SubscribeEvents()
        {
            SetStateEvent += SetState;
        }
        public void UnSubscribeEvents()
        {
            SetStateEvent -= SetState;
        }

        public override void Init()
        {
            if (_turnDict == null || _turnDict.Count == 0)
            {
                _turnDict = new Dictionary<TurnState, Turn>()
                {
                 {TurnState.Startbattle, new StartBattle(this) },
                 {TurnState.StartEnemyTurn, new StartEnemyTurn(this) },
                 {TurnState.EnemyTurn, new EnemyTurn(this) },
                 {TurnState.EndEnemyTurn, new EndEnemyTurn(this) },
                 {TurnState.StartPlayerTurn, new StartPlayerTurn(this) },
                 {TurnState.PlayerTurn, new PlayerTurn(this) },
                 {TurnState.EndPlayerTurn, new EndPlayerTurn(this ,_playerControler) },
                 {TurnState.EndBattle, new EndBattle(this) },
                 {TurnState.NotInBattle, null },
                };

            }
            ResetTurnHandler();
            SubscribeEvents();
        }

        public void ResetTurns()
        {
            Init();
            CurrentState = TurnState.Startbattle;
        }
        #endregion

    }


    public interface ITurnHandler
    {
         TurnState GetNextTurn { get; }
         IEnumerator PlayTurn();
    

    }
    public abstract class Turn : ITurnHandler {
        protected TurnHandler _turnHandler;
        public Turn(TurnHandler _th)
        {
            _turnHandler = _th;
        }

        public abstract TurnState GetNextTurn { get; }
        public abstract IEnumerator PlayTurn();
        protected void MoveToNextState()
            => TurnHandler.SetStateEvent?.Invoke(GetNextTurn);
    }
    public class StartBattle : Turn
    {
        public StartBattle(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.StartEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            yield return null;
            MoveToNextState();
        }

    }
    public class StartEnemyTurn : Turn
    {
        public StartEnemyTurn(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.EnemyTurn;

        public override IEnumerator PlayTurn()
        {
            yield return KeywordManager.Instance.OnStartTurnKeywords(false);
            yield return new WaitForSeconds(0.1f);
            MoveToNextState();
        }

    }
    public class EnemyTurn : Turn
    {
        public EnemyTurn(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.EndEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            // Activate Previous Action if not null
            yield return EnemyManager.Instance.GetEnemy.PlayEnemyTurn();
            // set a new action
            yield return EnemyManager.Instance.GetEnemy.AssignNextCard();

            yield return null;
            MoveToNextState();
        }

    }
    public class EndEnemyTurn : Turn
    {
        public EndEnemyTurn(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.StartPlayerTurn;

        public override IEnumerator PlayTurn()
        {
        /*
         * Activate the enemy keywords 
         * Remove The Player Defense
         */
            yield return KeywordManager.Instance.OnEndTurnKeywords(false);
            yield return new WaitForSeconds(0.5f);
            StatsHandler.GetInstance.ResetShield(true);
            MoveToNextState();
        }

    }

    public class StartPlayerTurn : Turn
    {
        public StartPlayerTurn(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.PlayerTurn;

        public override IEnumerator PlayTurn()
        {

            yield return KeywordManager.Instance.OnStartTurnKeywords(true);
            Deck.DeckManager.Instance.DrawHand(StatsHandler.GetInstance.GetCharacterStats(true).DrawCardsAmount);
            StaminaHandler.ResetStamina();
            Debug.Log("Drawing Cards!");
            MoveToNextState();
        }

    }
    public class PlayerTurn : Turn
    {
        public PlayerTurn(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.EndEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            // unlock Player Inputs 
            _turnHandler.IsTurnFinished = false;
            yield return null;
     
        }
        
    }
    public class EndBattle : Turn
    {
        public EndBattle(TurnHandler _th) : base(_th)
        {
        }

        public override TurnState GetNextTurn => TurnState.NotInBattle;

        public override IEnumerator PlayTurn()
        {
            yield return null;
        }

    } 
    public class EndPlayerTurn : Turn
    {
        AnimatorController _playerControler;
        public EndPlayerTurn(TurnHandler _th, AnimatorController _playerControler) : base(_th)
        {
            this._playerControler = _playerControler;
        }

        public override TurnState GetNextTurn => TurnState.StartEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            /*
             * we first remove the hands cards
             * then we pass the CardExecutionManager all the cards we currently have in the placementdeck
             * we give the animator the current cards so he can play the animations
             * each animation that finished tell the CardExecutionManager to execute the keyword effects of the current card
             * and remove the current card from the placementslot
            */
            CardUIManager.Instance.RemoveHands();
            CardExecutionManager.Instance.ResetExecution();


            yield return new WaitUntil (() => _playerControler.IsCurrentlyIdle || _turnHandler.IsTurnFinished == true);
            Deck.DeckManager.Instance.OnEndTurn();


            yield return KeywordManager.Instance.OnEndTurnKeywords(true);
            yield return new WaitForSeconds(0.5f);
            StatsHandler.GetInstance.ResetShield(false);
            MoveToNextState();
        }

    }

}