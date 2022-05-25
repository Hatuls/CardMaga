using Battles.Turns;
using Battles.UI;
using Characters;
using Characters.Stats;
using Managers;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UI.Meta.Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Battles
{
    public class BattleManager : MonoSingleton<BattleManager>
    {
        public static event Action OnGameEnded;
        public static bool isGameEnded;

        [SerializeField, EventsGroup]
        private Unity.Events.StringEvent _playSound;
        [SerializeField, EventsGroup]
        private UnityEvent OnPlayerDefeat;
        [SerializeField, EventsGroup]
        private UnityEvent OnPlayerVictory;
        [SerializeField, EventsGroup]
        private UnityEvent OnBattleStarts;

        [SerializeField]
        private CameraController _cameraController;

        [SerializeField]
        private SceneIdentificationSO _mapScene;


        private IEnumerator _turnCycles;
        private ISceneHandler _sceneHandler;




        public override void Init(ITokenReciever token)
        {
            using (token.GetToken())
            {
                ResetBattle();
                AudioManager.Instance.BattleMusicParameter();
            }

        }

        private void ResetBattle()
        {

            isGameEnded = false;

            ResetParams();
            AssignParams();

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


            // remove this later
            if (EndTurnButton._OnFinishTurnPress != null)
                EndTurnButton._OnFinishTurnPress -= TurnHandler.OnFinishTurn;

            EndTurnButton._OnFinishTurnPress += TurnHandler.OnFinishTurn;

            StaminaHandler.Instance.InitStaminaHandler();
        }
        private void ResetParams()
        {
            AudioManager.Instance.StopAllSounds();
            isGameEnded = false;


            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.EnemyAnimatorController.ResetLayerWeight();
        }

        public void StartBattle()
        {
            StopAllCoroutines();
            _turnCycles = TurnHandler.TurnCycle();
            Account.AccountManager.Instance.BattleData.PlayerWon = false;
            OnBattleStarts?.Invoke();
            StartGameTurns();
        }


        private void StartGameTurns()
            => StartCoroutine(_turnCycles);



        public void BattleEnded(bool isPlayerDied)
        {
            if (isGameEnded == true)
                return;
            OnGameEnded?.Invoke();
            UI.StatsUIManager.Instance.UpdateHealthBar(isPlayerDied, 0);
            CardExecutionManager.Instance.ResetExecution();
            CardUIManager.Instance.ResetCardUIManager();


            if (isPlayerDied)
                PlayerDied();
            else
                EnemyDied();

            TextPopUpHandler.Instance.CreatePopUpText(UI.TextType.Money, UI.TextPopUpHandler.TextPosition(isPlayerDied), "K.O.");

            Account.AccountManager.Instance.BattleData.PlayerWon = !isPlayerDied;
            UpdateStats();

            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.EnemyAnimatorController.ResetLayerWeight();

            isGameEnded = true;
            StopCoroutine(Instance._turnCycles);
        }

        private static void UpdateStats()
        {
            var playerDAta = Account.AccountManager.Instance.BattleData.Player;
            var playerBattleStats = CharacterStatsManager.GetCharacterStatsHandler(true);

            playerDAta.CharacterData.CharacterStats.Health = playerBattleStats.GetStats(Keywords.KeywordTypeEnum.Heal).Amount;

            Account.AccountManager.Instance.BattleData.Player = playerDAta;
        }

        public void DeathAnimationFinished(bool isPlayer)
        {

            if (isPlayer || (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == CharacterTypeEnum.Tutorial))
                Account.AccountManager.Instance.BattleData.IsFinishedPlaying = true;

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 0);

            _sceneHandler.MoveToScene(_mapScene);
        }

        private void EnemyDied()
        {
            PlayerManager.Instance.PlayerWin();
            EnemyManager.EnemyAnimatorController.CharacterIsDead();
            var battleData = Account.AccountManager.Instance.BattleData;

            SendAnalyticWhenGameEnded("player_won", battleData);
            AddRewards();
            _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Player);
            OnPlayerVictory?.Invoke();
        }

        private void AddRewards()
        {
            var battleData = Account.AccountManager.Instance.BattleData;
            var characterTypeEnum = battleData.Opponent.CharacterData.CharacterSO.CharacterType;
            var reward = Factory.GameFactory.Instance.RewardFactoryHandler.GetRunRewards(characterTypeEnum, battleData.CurrentAct);
            battleData[characterTypeEnum].Diamonds += reward.DiamondsReward;
            battleData[characterTypeEnum].EXP += reward.EXPReward;
        }

        private void PlayerDied()
        {
            var battleData = Account.AccountManager.Instance.BattleData;
            PlayerManager.Instance.PlayerAnimatorController.CharacterIsDead();
            EnemyManager.Instance.EnemyWon();

            _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Enemy);
            SendAnalyticWhenGameEnded("player_defeated", battleData);
            OnPlayerDefeat?.Invoke();
        }


        private void Inject(ISceneHandler sh)
            => _sceneHandler = sh;

        #region MonoBehaviour Callbacks
        private void OnDestroy()
        {
            if (EndTurnButton._OnFinishTurnPress != null)
                EndTurnButton._OnFinishTurnPress -= TurnHandler.OnFinishTurn;

            AnimatorController.OnDeathAnimationFinished -= DeathAnimationFinished;
            SceneHandler.OnBeforeSceneShown -= Init;
            SceneHandler.OnSceneHandlerActivated -= Inject;
            SceneHandler.OnSceneStart -= StartBattle;
            HealthStat.OnCharacterDeath -= BattleEnded;
            SettingsScreenUI.OnAbandon -= BattleEnded;
        }
        public override void Awake()
        {
            SettingsScreenUI.OnAbandon += BattleEnded;
            SceneHandler.OnSceneHandlerActivated += Inject;
            SceneHandler.OnBeforeSceneShown += Init;
            HealthStat.OnCharacterDeath += BattleEnded;
            SceneHandler.OnSceneStart += StartBattle;
            AnimatorController.OnDeathAnimationFinished += DeathAnimationFinished;
            base.Awake();
        }
        #endregion
        #region Analytics

        private static void SendAnalyticWhenGameEnded(string eventName, BattleData battleData)
        {
            var characterSO = battleData.Opponent.CharacterData.CharacterSO;

            string characterEnum = "opponent";
            string characterDifficulty = "difficulty";
            string characterType = "character_type";
            string TurnCount = "turns_count";

            UnityAnalyticHandler.SendEvent(eventName, new System.Collections.Generic.Dictionary<string, object>() {
                  { characterEnum,  characterSO.CharacterEnum.ToString()  },
                  {characterDifficulty,    characterSO.CharacterDiffciulty},
                  {characterType,characterSO.CharacterType.ToString() },
                  {TurnCount, TurnHandler.TurnCount }
            });

            FireBaseHandler.SendEvent(eventName, new Firebase.Analytics.Parameter[4] {
               new Firebase.Analytics.Parameter(characterEnum, characterSO.CharacterEnum.ToString() ),
               new Firebase.Analytics.Parameter(characterDifficulty,    characterSO.CharacterDiffciulty),
               new Firebase.Analytics.Parameter(characterType,characterSO.CharacterType.ToString() ),
               new Firebase.Analytics.Parameter(TurnCount,TurnHandler.TurnCount ),
            });
        }

        #endregion

        #region Editor Section
#if UNITY_EDITOR
        [Button]
        public void KillEnemy()
        => CharacterStatsManager.GetCharacterStatsHandler(false)?.RecieveDamage(1000000);

        [Button]
        public void KillPlayer()
           => CharacterStatsManager.GetCharacterStatsHandler(true)?.RecieveDamage(1000000);
#endif
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
