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

        public delegate void TurnEvent(TurnState state);
        public static TurnEvent SetStateEvent;


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
                 {TurnState.Startbattle, new StartBattle() },
                 {TurnState.StartEnemyTurn, new StartEnemyTurn() },
                 {TurnState.EnemyTurn, new EnemyTurn() },
                 {TurnState.EndEnemyTurn, new EndEnemyTurn() },
                 {TurnState.StartPlayerTurn, new StartPlayerTurn() },
                 {TurnState.PlayerTurn, new PlayerTurn() },
                 {TurnState.EndPlayerTurn, new EndPlayerTurn() },
                 {TurnState.EndBattle, new EndBattle() },
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
   
        public abstract TurnState GetNextTurn { get; }
        public abstract IEnumerator PlayTurn();
        protected void MoveToNextState()
            => TurnHandler.SetStateEvent?.Invoke(GetNextTurn);
    }
    public class StartBattle : Turn
    {
        public override TurnState GetNextTurn => TurnState.StartEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            yield return null;
            MoveToNextState();
        }

    }
    public class StartEnemyTurn : Turn
    {
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
        public override TurnState GetNextTurn => TurnState.PlayerTurn;

        public override IEnumerator PlayTurn()
        {

            yield return KeywordManager.Instance.OnStartTurnKeywords(true);
            Deck.DeckManager.Instance.DrawHand(StatsHandler.GetInstance.GetCharacterStats(true).DrawCardsAmount);
            Debug.Log("Drawing Cards!");
            MoveToNextState();
        }

    }
    public class PlayerTurn : Turn
    {
        public override TurnState GetNextTurn => TurnState.EndEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            // unlock Player Inputs 
            yield return null;
     
        }
        
    }
    public class EndBattle : Turn
    {
        public override TurnState GetNextTurn => TurnState.NotInBattle;

        public override IEnumerator PlayTurn()
        {
            yield return null;
        }

    } 
    public class EndPlayerTurn : Turn
    {
        public override TurnState GetNextTurn => TurnState.StartEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            CardUIManager.Instance.RemoveHands();
            yield return CardExecutionManager.Instance.StartExecution();
            PlaceHolderHandler.Instance.PlayerPlaceHolder.ResetPlaceHolders();
            Deck.DeckManager.Instance.OnEndTurn();

            yield return KeywordManager.Instance.OnEndTurnKeywords(true);
            yield return new WaitForSeconds(0.5f);
            StatsHandler.GetInstance.ResetShield(false);
            MoveToNextState();
        }

    }

}