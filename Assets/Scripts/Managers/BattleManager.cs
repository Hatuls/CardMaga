using Battles.Turns;
using UnityEngine;
using System;
using Managers;
using System.Collections;
namespace Battles
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        [SerializeField] BattleData _BattleInformation;
      public static bool isGameEnded;

        [SerializeField] Unity.Events.SoundsEvent _playSound;


        IEnumerator _turnCycles;

        public override void Init()
        {
            if (_BattleInformation == null)
                Debug.LogError("BattleManager: Character Dictionary was not assigned");

            ResetParams();
            AssignParams();
            StartBattle();
        }  

        private void AssignParams()
        {
            PlayerManager.Instance.AssignCharacterData(_BattleInformation.OpponentOne);
            EnemyManager.Instance.AssignCharacterData(_BattleInformation.OpponentTwo);
            PlayerManager.Instance.UpdateStats();
            EnemyManager.Instance.UpdateStats();
        }
        private void ResetParams()
        {
         Deck.DeckManager.Instance.ResetDecks();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartBattle();

            }
        }
        public static void StartBattle() 
        {
            // Get enemy oponnent
            // get Player stats and cards
            // assign stats handler
            // set keywords
            // set relics
            // reset decks
            // reset turns

            // turn handler start

         


              Instance._turnCycles = TurnHandler.TurnCycle();


            StartGameTurns();
            Instance.StartCoroutine(Instance.BackGroundSoundDelay());


        }


        private static void StartGameTurns()
            => Instance.StartCoroutine(Instance._turnCycles);


        IEnumerator BackGroundSoundDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _playSound?.Raise(SoundsNameEnum.CombatBackground);
            yield return new WaitForSeconds(0.5f);
            _playSound?.Raise(SoundsNameEnum.VS);
        }
        public static void BattleEnded(bool isPlayerDied)
        {
            UI.StatsUIManager.GetInstance.UpdateHealthBar(isPlayerDied, 0);      
            CardExecutionManager.Instance.ResetExecution();
            if (isPlayerDied)
            {
                PlayerDied();
            }
            else
            {
                EnemyDied();
            }



            isGameEnded = true;
            Instance.StopCoroutine(Instance._turnCycles);
        }

        private static void EnemyDied()
        {
           PlayerManager.Instance.PlayerAnimatorController.CharacterWon();
            EnemyManager.EnemyAnimatorController.CharacterIsDead();
            UI.TextPopUpHandler.GetInstance.CreatePopUpText(UI.TextType.Money, UI.TextPopUpHandler.TextPosition(false), "K.O.");
            Instance._playSound?.Raise(SoundsNameEnum.Victory);
        }

        private static void PlayerDied()
        {
            PlayerManager.Instance.PlayerAnimatorController.CharacterIsDead();
            EnemyManager.EnemyAnimatorController.CharacterWon();
            if (isGameEnded == false)
                Instance._playSound?.Raise(SoundsNameEnum.Defeat);
        }
    }


    public interface IBattleHandler
    {
        void RestartBattle();
      void  AssignCharacterData(CharacterSO characterSO);
        void UpdateStats();
        void OnEndBattle();
    }
}
