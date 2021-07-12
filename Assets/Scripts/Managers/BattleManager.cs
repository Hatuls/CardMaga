using Battles.Turns;
using UnityEngine;
using System;
using Managers;

namespace Battles
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        [SerializeField] CharactersDictionary _charactersDictionary;
      public  static bool isGameEnded;

        [SerializeField] Unity.Events.SoundsEvent _playSound;





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

            Deck.DeckManager.Instance.ResetDeckManager();

            TurnHandler.Instance.ResetTurns();


            Instance.StartCoroutine(Instance.BackGroundSoundDelay());
        }
        System.Collections.IEnumerator BackGroundSoundDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _playSound?.Raise(SoundsNameEnum.CombatBackground);
            yield return new WaitForSeconds(0.5f);
            _playSound?.Raise(SoundsNameEnum.VS);
        }
        public static void BattleEnded(bool isPlayerDied)
        {

            if (isPlayerDied)
            {
                PlayerManager.Instance.PlayerAnimatorController.CharacterIsDead();
                if (isGameEnded == false)
                   Instance._playSound?.Raise(SoundsNameEnum.Defeat);
            }
            else
            {
             
                PlayerManager.Instance.PlayerAnimatorController.CharacterWon();
                EnemyManager.EnemyAnimatorController.CharacterIsDead();
                UI.TextPopUpHandler.GetInstance.CreatePopUpText(UI.TextType.Money, UI.TextPopUpHandler.TextPosition(false), "K.O.");
                Instance._playSound?.Raise(SoundsNameEnum.Victory );
            }
            UI.StatsUIManager.GetInstance.UpdateHealthBar(isPlayerDied, 0);
            CardExecutionManager.Instance.ResetExecution();
            isGameEnded = true;
            TurnHandler.Instance.BattleEnded();
        }
    }


    public interface IBattleHandler
    {
        void OnStartBattle();
        void OnEndBattle();
    }
}
