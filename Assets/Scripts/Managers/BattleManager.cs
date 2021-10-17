using Battles.Turns;
using UnityEngine;
using Sirenix.OdinInspector;
using Managers;
using System.Collections;
using Characters.Stats;
using System;
using Rewards.Battles;

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
            ResetBattle();
            SceneHandler.onFinishLoadingScene += OnLoadScene;
        }

        private void Start()
        {
            Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(81);
        }

        private void ResetBattle()
        {
            if (_BattleInformation == null)
                Debug.LogError("BattleManager: Character Dictionary was not assigned");
            isGameEnded = false;

            ResetParams();
            AssignParams();
            StartBattle();
        }

        private void AssignParams()
        {
            if (_BattleInformation.UseSO)
            {
                PlayerManager.Instance.AssignCharacterData(_BattleInformation.OpponentOne);
                EnemyManager.Instance.AssignCharacterData(_BattleInformation.OpponentTwo);
            }
            else
            {
                PlayerManager.Instance.AssignCharacterData(_BattleInformation.PlayerCharacterData);
                EnemyManager.Instance.AssignCharacterData(_BattleInformation.OpponentCharacterData);
            }
            if (_BattleInformation.PlayerCharacterData.CharacterStats.Health <= 0)
                throw new Exception("Battle data was not work correctly!");

            PlayerManager.Instance.UpdateStatsUI();
            EnemyManager.Instance.UpdateStatsUI();
            Combo.ComboManager.Instance.Init();
            Keywords.KeywordManager.Instance.Init();
            AudioManager.Instance.ResetAudioCollection();
            EndTurnButton._OnFinishTurnPress += TurnHandler.OnFinishTurn;
        }
        private void ResetParams()
        {
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
            // Get enemy oponnent
            // get Player stats and cards
            // assign stats handler
            // set keywords
            // set relics
            // reset decks
            // reset turns

            // turn handler start


            Instance.StopAllCoroutines();
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

            UpdateStats();

            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.EnemyAnimatorController.ResetLayerWeight();

            isGameEnded = true;
            Instance.StopCoroutine(Instance._turnCycles);
            BattleRewardHandler.Instance.FinishMatch(!isPlayerDied);
        }

        private static void UpdateStats()
        {
            var x = Instance._BattleInformation.PlayerCharacterData;
            var playerBattleStats = CharacterStatsManager.GetCharacterStatsHandler(true);

            x.CharacterStats.Health = playerBattleStats.GetStats(Keywords.KeywordTypeEnum.Heal).Amount;
        
            Instance._BattleInformation.UpdatePlayerCharacter(x);
        }

        public static void DeathAnimationFinished(bool isPlayer)
        {

            BattleUIRewardHandler.Instance.ShowBattleRewardUI(isPlayer);
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
            if (scenesEnum == SceneHandler.ScenesEnum.Battle)
                ResetBattle();
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
      void  AssignCharacterData(CharacterSO characterSO);
      void  AssignCharacterData(Character character);
        void UpdateStatsUI();
        void OnEndBattle();
    }
}
