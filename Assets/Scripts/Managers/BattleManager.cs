using Battles.Turns;
using UnityEngine;
using System;
using Managers;
using System.Collections;
namespace Battles
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        [SerializeField] CharactersDictionary _charactersDictionary;
      public static bool isGameEnded;

        [SerializeField] Unity.Events.SoundsEvent _playSound;


        IEnumerator _turnCycles;


        public static CharactersDictionary GetDictionary(Type _script)
        {
            if (_script == typeof(EnemyManager) || _script == typeof(PlayerManager))
                return Instance._charactersDictionary;

            return null;
        }

        public override void Init()
        {
            if (_charactersDictionary == null)
                Debug.LogError("BattleManager: Character Dictionary was not assigned");

            AssignParams();
            ResetParams();
            StartBattle();
        }  

        private void AssignParams()
        {

        }
        private void ResetParams()
        {

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

            //      SetBattleStats



            EnemyManager.Instance.SetEnemy(Instance._charactersDictionary.GetCharacter(CharactersEnum.Enemy));
            UI.StatsUIManager.GetInstance.InitHealthBar(true, PlayerManager.Instance.GetCharacterStats.Health);

            Instance._turnCycles = TurnHandler.TurnCycle();

            Deck.DeckManager.Instance.ResetDeckManager();
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
        void OnStartBattle();
        void OnEndBattle();
    }
}
