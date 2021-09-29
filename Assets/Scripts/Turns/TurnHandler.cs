using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Keywords;
using Characters.Stats;
using Battles.UI;


namespace Battles.Turns
{

    public static class TurnHandler
    {


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


        private static TurnState _currectState;
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

        public static void OnFinishTurn() {
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

            Deck.DeckManager.Instance.DrawHand(
                isPlayerTurn,
                StatsHandler.GetInstance.GetCharacterStats(isPlayerTurn).DrawCardsAmount
                ); 

            StaminaHandler.Instance.ResetStamina(isPlayerTurn);

            Debug.Log("Enemy Drawing Cards!");
            yield return new WaitForSeconds(0.1f);
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
            base.PlayTurn();
            // Activate Previous Action if not null
            yield return EnemyManager.Instance.PlayEnemyTurn();


            yield return null;
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

          // CardUIManager.Instance.RemoveHands();
            CardExecutionManager.Instance.ResetExecution();
            base.PlayTurn();


            CameraController.Instance.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Both);
            Deck.DeckManager.Instance.OnEndTurn(false);

            base.PlayTurn();
            yield return KeywordManager.Instance.OnEndTurnKeywords(false);
            yield return new WaitForSeconds(0.5f);
            StatsHandler.GetInstance.ResetShield(true);
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
            base.PlayTurn();
            yield return KeywordManager.Instance.OnStartTurnKeywords(true);
            Deck.DeckManager.Instance.DrawHand(true,StatsHandler.GetInstance.GetCharacterStats(true).DrawCardsAmount);
            StaminaHandler.Instance.ResetStamina(true) ;
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

            do
            {
            yield return null;
            } while (!TurnHandler.FinishTurn);

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
            yield return null;
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

            Deck.DeckManager.Instance.OnEndTurn(true);
            CardUIManager.Instance.RemoveHands();
            CardExecutionManager.Instance.ResetExecution();
            base.PlayTurn();


            CameraController.Instance.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Both);



            yield return KeywordManager.Instance.OnEndTurnKeywords(true);
            yield return new WaitForSeconds(0.1f);
            StatsHandler.GetInstance.ResetShield(false);
            Managers.PlayerManager.Instance.OnEndTurn();
            EnemyManager.Instance.OnEndTurn();
            MoveToNextTurnState();
        }

    }

}