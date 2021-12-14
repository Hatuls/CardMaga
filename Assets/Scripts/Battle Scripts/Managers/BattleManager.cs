﻿using Battles.Turns;
using Battles.UI;
using Characters;
using Characters.Stats;
using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Battles
{
    public class BattleManager : MonoSingleton<BattleManager>
    {

        public static bool isGameEnded;
        public static Action OnGameEnded;
        [SerializeField] Unity.Events.StringEvent _playSound;
        public UnityEvent OnPlayerDefeat;
        public UnityEvent OnPlayerVictory;
        public UnityEvent OnBattleStarts;

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

            var battleData = Account.AccountManager.Instance.BattleData;
            PlayerManager.Instance.AssignCharacterData(battleData.Player);
            EnemyManager.Instance.AssignCharacterData(battleData.Opponent);
            
            if (battleData.Player.CharacterData.CharacterStats.Health <= 0)
                throw new Exception("Battle data was not work correctly!");

            PlayerManager.Instance.UpdateStatsUI();
            EnemyManager.Instance.UpdateStatsUI();
            Combo.ComboManager.Instance.Init();
            Keywords.KeywordManager.Instance.Init();
       


            if (EndTurnButton._OnFinishTurnPress != null)
                EndTurnButton._OnFinishTurnPress -= TurnHandler.OnFinishTurn;
            EndTurnButton._OnFinishTurnPress += TurnHandler.OnFinishTurn;

            StaminaHandler.Instance.InitStaminaHandler();
        }
        private void ResetParams()
        {
            AudioManager.Instance.StopAllSounds();
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
            Account.AccountManager.Instance.BattleData.PlayerWon = false;
            Instance. OnBattleStarts?.Invoke();
            StartGameTurns();


        }


        private static void StartGameTurns()
            => Instance.StartCoroutine(Instance._turnCycles);



        public static void BattleEnded(bool isPlayerDied)
        {
            if (isGameEnded == true)
                return;
            OnGameEnded?.Invoke();
            UI.StatsUIManager.GetInstance.UpdateHealthBar(isPlayerDied, 0);
            CardExecutionManager.Instance.ResetExecution();
            CardUIManager.Instance.ResetCardUIManager();


            if (isPlayerDied)
            {
                PlayerDied();

            }
            else
            {
                EnemyDied();
            }
            Account.AccountManager.Instance.BattleData.PlayerWon = !isPlayerDied;
            UpdateStats();

            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.EnemyAnimatorController.ResetLayerWeight();
            isGameEnded = true;
            Instance.StopCoroutine(Instance._turnCycles);
            //   BattleRewardHandler.Instance.FinishMatch(!isPlayerDied);
        }

        private static void UpdateStats()
        {
            var x = Account.AccountManager.Instance.BattleData.Player;
            var playerBattleStats = CharacterStatsManager.GetCharacterStatsHandler(true);

            x.CharacterData.CharacterStats.Health = playerBattleStats.GetStats(Keywords.KeywordTypeEnum.Heal).Amount;

            Account.AccountManager.Instance.BattleData.Player = x;
        }

        public static void DeathAnimationFinished(bool isPlayer)
        {

            if (isPlayer)
                Account.AccountManager.Instance.BattleData.IsFinishedPlaying = true;

            SceneHandler.LoadScene(SceneHandler.ScenesEnum.MapScene);

        }


        private static void EnemyDied()
        {
            PlayerManager.Instance.PlayerAnimatorController.CharacterWon();
            EnemyManager.EnemyAnimatorController.CharacterIsDead();
            UI.TextPopUpHandler.GetInstance.CreatePopUpText(UI.TextType.Money, UI.TextPopUpHandler.TextPosition(false), "K.O.");
            Instance.OnPlayerVictory?.Invoke();
        }

        private static void PlayerDied()
        {
            PlayerManager.Instance.PlayerAnimatorController.CharacterIsDead();
            EnemyManager.EnemyAnimatorController.CharacterWon();
            Instance.OnPlayerDefeat?.Invoke();
        }


        private void OnLoadScene(SceneHandler.ScenesEnum scenesEnum)
        {
            
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

        void AssignCharacterData(Character character);
        void UpdateStatsUI();
        void OnEndBattle();
    }
}
