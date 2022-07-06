using Battles.UI;
using Characters.Stats;
using Keywords;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battles.Turns
{

    public static class TurnHandler
    {
        public static Action<int> OnTurnCountChange;
        private static int _turnCount = 0;
        private static TurnState _currectState;


        private static Dictionary<TurnState, Turn> _turnDict = new Dictionary<TurnState, Turn>()
                {
                 { TurnState.Startbattle, new StartBattle() },
                 { TurnState.StartEnemyTurn, new StartEnemyTurn() },
                 { TurnState.EnemyTurn, new EnemyTurn() },
                 { TurnState.EndEnemyTurn, new EndEnemyTurn() },
                 { TurnState.StartPlayerTurn, new StartPlayerTurn() },
                 { TurnState.PlayerTurn, new PlayerTurn() },
                 { TurnState.EndPlayerTurn, new EndPlayerTurn() },
                 { TurnState.EndBattle, new EndBattle() },
                 { TurnState.NotInBattle, null },
                };

        public static int TurnCount
        {
            get => _turnCount;
            set
            {
                _turnCount = value; OnTurnCountChange?.Invoke(_turnCount);
            }
        }
        public static TurnState CurrentState
        {

            get => _currectState;
            private set
            {
                if (_currectState != value || BattleManager.isGameEnded)
                {
                    _currectState = value;
                }

            }
        }
        public static bool FinishTurn { get; set; }
        public static bool IsPlayerTurn
        {
            get
            {
                var State = TurnHandler.CurrentState;
                return (State == TurnState.PlayerTurn || State == TurnState.StartPlayerTurn || State == TurnState.EndPlayerTurn);

            }
        }
        public static void OnFinishTurn()
        {
            FinishTurn = true;
        }
        public static void MoveToNextTurn(TurnState turnState)
        {
            Debug.Log("Current Turn State is " + CurrentState);
            CurrentState = turnState;
            Debug.Log("After update Turn State is " + CurrentState);
        }
        public static IEnumerator TurnCycle()
        {
            TurnState turn = TurnState.NotInBattle;
            CurrentState = TurnState.Startbattle;
            do
            {
                if (turn != CurrentState)
                {
                    turn = CurrentState;
                    yield return _turnDict[turn].PlayTurn();
                }
            }
            while (!BattleManager.isGameEnded);

            CurrentState = TurnState.EndBattle;
            yield return _turnDict[CurrentState].PlayTurn();
        }




        internal static void CheckPlayerTurnForAvailableAction()
        {
            if (CurrentState != TurnState.PlayerTurn && BattleManager.isGameEnded == false)
                return;

            bool noMoreActionAvailable = StaminaHandler.Instance.PlayerStamina.HasStamina == false && CardExecutionManager.CardsQueue.Count == 0;

            if (noMoreActionAvailable)
            {

                EndTurnButton.FinishTurn();
            }
        }
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

    public interface ITurnHandler
    {
        TurnState GetNextTurn { get; }
        IEnumerator PlayTurn();


    }
    public abstract class Turn : ITurnHandler
    {
        public static WaitForSeconds WaitOneSecond = new WaitForSeconds(1f);

        public Turn()
        {


        }

        public abstract TurnState GetNextTurn { get; }
        public virtual IEnumerator PlayTurn()
        {
            if (BattleManager.isGameEnded)
                yield break;
        }
        protected void MoveToNextTurnState()
            => TurnHandler.MoveToNextTurn(GetNextTurn);

    }
    public class StartBattle : Turn
    {
        public StartBattle() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.StartPlayerTurn;

        public override IEnumerator PlayTurn()
        {
            TurnHandler.TurnCount = 0;
            base.PlayTurn();
            yield return null;
            MoveToNextTurnState();
        }

    }
    public class StartEnemyTurn : Turn
    {
        public StartEnemyTurn() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.EnemyTurn;

        public override IEnumerator PlayTurn()
        {
            bool isPlayerTurn = false;
            base.PlayTurn();
            yield return KeywordManager.Instance.OnStartTurnKeywords(isPlayerTurn);

            yield return null;
            MoveToNextTurnState();
        }

    }
    public class EnemyTurn : Turn
    {
        public EnemyTurn() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.EndEnemyTurn;

        public override IEnumerator PlayTurn()
        {
            bool isPlayerTurn = false;
            Deck.DeckManager.Instance.DrawHand(
                isPlayerTurn,
                CharacterStatsManager.GetCharacterStatsHandler(isPlayerTurn).GetStats(KeywordTypeEnum.Draw).Amount
                );

            StaminaHandler.Instance.OnStartTurn(isPlayerTurn);

            Debug.Log("Enemy Drawing Cards!");

            base.PlayTurn();
            // Activate Previous Action if not null


            if (!KeywordManager.Instance.IsCharcterIsStunned(isPlayerTurn))
                yield return EnemyManager.Instance.PlayEnemyTurn();

            MoveToNextTurnState();
        }

    }
    public class EndEnemyTurn : Turn
    {
        public EndEnemyTurn() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.StartPlayerTurn;

        public override IEnumerator PlayTurn()
        {
            /*
             * Activate the enemy keywords 
             * Remove The Player Defense
             */
            GameEventsInvoker.Instance.OnEndTurn?.Invoke();
            // CardUIManager.Instance.RemoveHands();
            CardExecutionManager.Instance.ResetExecution();
            base.PlayTurn();


            CameraController.Instance.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Both);
            Deck.DeckManager.Instance.OnEndTurn(false);

            base.PlayTurn();
            yield return KeywordManager.Instance.OnEndTurnKeywords(false);

            CharacterStatsManager.GetCharacterStatsHandler(true).GetStats(KeywordTypeEnum.Shield).Reset();

            Managers.PlayerManager.Instance.OnEndTurn();
            EnemyManager.Instance.OnEndTurn();
            MoveToNextTurnState();


        }

    }

    public class StartPlayerTurn : Turn
    {
        public StartPlayerTurn() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.PlayerTurn;

        public override IEnumerator PlayTurn()
        {
            TurnHandler.TurnCount++;
            bool isPlayerTurn = true;
            base.PlayTurn();
            yield return KeywordManager.Instance.OnStartTurnKeywords(isPlayerTurn);

            Debug.Log("Player Drawing Cards!");
            MoveToNextTurnState();
        }

    }
    public class PlayerTurn : Turn
    {
        public PlayerTurn() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.EndPlayerTurn;

        public override IEnumerator PlayTurn()
        {
            base.PlayTurn();
            // unlock Player Inputs 
            TurnHandler.FinishTurn = false;
            bool isPlayerTurn = true;
            if (!KeywordManager.Instance.IsCharcterIsStunned(isPlayerTurn))
            {
                Deck.DeckManager.Instance.DrawHand(isPlayerTurn, CharacterStatsManager.GetCharacterStatsHandler(isPlayerTurn).GetStats(KeywordTypeEnum.Draw).Amount);
                StaminaHandler.Instance.OnStartTurn(isPlayerTurn);

                do
                {
                    yield return null;
                } while (!TurnHandler.FinishTurn);
            }
            MoveToNextTurnState();
        }

    }
    public class EndBattle : Turn
    {
        public EndBattle() : base()
        {
        }

        public override TurnState GetNextTurn => TurnState.NotInBattle;

        public override IEnumerator PlayTurn()
        {
            base.PlayTurn();
            SendAnalyticData();
            yield return null;
        }

        private static void SendAnalyticData()
        {
             string turnCount = string.Concat("Finished Battle At Turn ", TurnHandler.TurnCount);
            UnityAnalyticHandler.SendEvent(turnCount);
            FireBaseHandler.SendEvent(turnCount);
        }
    }
    public class EndPlayerTurn : Turn
    {
        public EndPlayerTurn() : base()
        {
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
            GameEventsInvoker.Instance.OnEndTurn?.Invoke();
            StaminaHandler.Instance.OnEndTurn(true);
            Deck.DeckManager.Instance.OnEndTurn(true);
            CardUIManager.Instance.RemoveHands();
            CardExecutionManager.Instance.ResetExecution();
            base.PlayTurn();


            CameraController.Instance.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Both);



            yield return KeywordManager.Instance.OnEndTurnKeywords(true);
            yield return null;
            CharacterStatsManager.GetCharacterStatsHandler(false).GetStats(KeywordTypeEnum.Shield).Reset();

            Managers.PlayerManager.Instance.OnEndTurn();
            EnemyManager.Instance.OnEndTurn();
            MoveToNextTurnState();
        }

    }

}