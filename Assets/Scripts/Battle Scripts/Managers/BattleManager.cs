using Battles.Turns;
using UnityEngine;
using Sirenix.OdinInspector;
using Managers;
using System.Collections;
using Characters.Stats;
using System;
using Rewards.Battles;
using Characters;

namespace Battles
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
  
        public static bool isGameEnded;
        public static Action OnGameEnded;
        [SerializeField] Unity.Events.SoundsEvent _playSound;


        IEnumerator _turnCycles;

        public override void Init()
        {
            ResetBattle();
            SceneHandler.onFinishLoadingScene += OnLoadScene;
        }

        private void ResetBattle()
        {

            isGameEnded = false;

            ResetParams();
            AssignParams();
            StartBattle();
        }

        private void AssignParams()
        {
         
            
                PlayerManager.Instance.AssignCharacterData(BattleData.Player);
                EnemyManager.Instance.AssignCharacterData(BattleData.Opponent);
            
            if (BattleData.Player.CharacterData.CharacterStats.Health <= 0)
                throw new Exception("Battle data was not work correctly!");

            PlayerManager.Instance.UpdateStatsUI();
            EnemyManager.Instance.UpdateStatsUI();
            Combo.ComboManager.Instance.Init();
            Keywords.KeywordManager.Instance.Init();
            AudioManager.Instance.ResetAudioCollection();


            if (EndTurnButton._OnFinishTurnPress!= null)
            EndTurnButton._OnFinishTurnPress -= TurnHandler.OnFinishTurn;
            EndTurnButton._OnFinishTurnPress += TurnHandler.OnFinishTurn;

            StaminaHandler.Instance.InitStaminaHandler();
        }
        private void ResetParams()
        {
            isGameEnded = false;

            CardManager.Instance.ResetCards();
            Deck.DeckManager.Instance.ResetDecks();
            UI.CardUIManager.Instance.Init();
            UI.CraftingUIManager.Instance.Init();
            VFXManager.Instance.Init();


           

            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.EnemyAnimatorController.ResetLayerWeight();
        }

        public static void StartBattle()
        {
            Instance.StopAllCoroutines();
            Instance._turnCycles = TurnHandler.TurnCycle();
            BattleData.PlayerWon = false;
            StartGameTurns();
            Instance.StartCoroutine(Instance.BackGroundSoundDelay());


        }


        private static void StartGameTurns()
            => Instance.StartCoroutine(Instance._turnCycles);


        IEnumerator BackGroundSoundDelay()
        {
            yield return new WaitForSeconds(0.5f);
          //  _playSound?.Raise(SoundsNameEnum.CombatBackground);
            yield return new WaitForSeconds(0.5f);
            _playSound?.Raise(SoundsNameEnum.VS);
        }
        public static void BattleEnded(bool isPlayerDied)
        {
            if (isGameEnded == true)
                return;
            OnGameEnded?.Invoke();
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
            BattleData.PlayerWon = !isPlayerDied;
            UpdateStats();

            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.EnemyAnimatorController.ResetLayerWeight();

            isGameEnded = true;
            Instance.StopCoroutine(Instance._turnCycles);
         //   BattleRewardHandler.Instance.FinishMatch(!isPlayerDied);
        }

        private static void UpdateStats()
        {
            var x = BattleData.Player;
            var playerBattleStats = CharacterStatsManager.GetCharacterStatsHandler(true);

            x.CharacterData.CharacterStats.Health = playerBattleStats.GetStats(Keywords.KeywordTypeEnum.Heal).Amount;

            BattleData.Player = x;
        }

        public static void DeathAnimationFinished(bool isPlayer)
        {
          
            if (isPlayer) 
                BattleData.IsFinishedPlaying = true;

            SceneHandler.LoadScene(SceneHandler.ScenesEnum.MapScene);
        
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


        private void OnLoadScene(SceneHandler.ScenesEnum scenesEnum)
        {
            if (scenesEnum == SceneHandler.ScenesEnum.GameBattleScene)
                ResetBattle();
        }









        private void OnDestroy()
        {
            if (EndTurnButton._OnFinishTurnPress != null)
                EndTurnButton._OnFinishTurnPress -= TurnHandler.OnFinishTurn;
        }


        #region Editor Section
        [Button]
        public void KillEnemy()
        => CharacterStatsManager.GetCharacterStatsHandler(false)?.RecieveDamage(1000000);

        [Button]
        public void KillPlayer()
           => CharacterStatsManager.GetCharacterStatsHandler(true)?.RecieveDamage(1000000);
        #endregion
    }


    public interface IBattleHandler
    {
        void RestartBattle();

      void  AssignCharacterData(Character character);
        void UpdateStatsUI();
        void OnEndBattle();
    }
}
