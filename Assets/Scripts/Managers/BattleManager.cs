using Battle.Data;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Battle.UI;
using Characters.Stats;
using Keywords;
using Managers;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    [DefaultExecutionOrder(-99999)]
    public class BattleManager : MonoSingleton<BattleManager>
    {
        public static event Action OnGameEnded;
        public static event Action<SceneStarter> OnSceneStart;


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
        private UnityEvent OnBattleFinished;

        [SerializeField] private DollyTrackCinematicManager _cinematicManager;
        [SerializeField]
        private PlayerManager _playerManager;
        [SerializeField]
        private EnemyManager _enemyManager;

        private IEnumerator _turnCycles;
        private SceneStarter _BattleStarter = new SceneStarter();




        public override void Init(ITokenReciever token)
        {
         var _initToken = token.GetToken();

            isGameEnded = false;

            ResetParams();
            AssignParams();

            if (AudioManager.Instance != null)
                AudioManager.Instance.BattleMusicParameter();

             _initToken.Dispose();
        }

        private void ResetBattle()
        {

            _BattleStarter.Start(StartBattle);

        }
        private void AssignParams()
        {

            var battleData = BattleData.Instance;
            SpawnModels(battleData);
            _playerManager.AssignCharacterData(battleData.Player);
            _enemyManager.AssignCharacterData(battleData.Opponent);

            StaminaHandler.Instance.InitStaminaHandler();
        }
        private void ResetParams()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopAllSounds();
            isGameEnded = false;


           _playerManager.PlayerAnimatorController.ResetLayerWeight();
           _enemyManager.EnemyAnimatorController.ResetLayerWeight();
        }
        // Need To be Re-Done
        public void StartBattle()
        {
            StopAllCoroutines();

            _turnCycles = TurnHandler.TurnCycle();

            BattleData.Instance.PlayerWon = false;

            OnBattleStarts?.Invoke();

            StartGameTurns();
        }


        private void StartGameTurns()
            => StartCoroutine(_turnCycles);


        // Need To be Re-Done
        public void BattleEnded(bool isPlayerDied)
        {
            if (isGameEnded == true)
                return;

            //    UI.StatsUIManager.Instance.UpdateHealthBar(isPlayerDied, 0);
            CardExecutionManager.Instance.ResetExecution();
            //CardUIManager.Instance.ResetCardUIManager();


            if (isPlayerDied)
                PlayerDied();
            else
                EnemyDied();


            //TextPopUpHandler.Instance.CreatePopUpText(UI.TextType.Money, UI.TextPopUpHandler.TextPosition(isPlayerDied), "K.O.");

            BattleData.Instance.PlayerWon = !isPlayerDied;


            _playerManager.PlayerAnimatorController.ResetLayerWeight();
            _enemyManager.EnemyAnimatorController.ResetLayerWeight();

            isGameEnded = true;
            StopCoroutine(Instance._turnCycles);


            OnGameEnded?.Invoke();
        }

        // Need To be Re-Done
        public void DeathAnimationFinished(bool isPlayer)
        {
            //if (isPlayer || (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == CharacterTypeEnum.Tutorial))
            //    Account.AccountManager.Instance.BattleData.IsFinishedPlaying = true;

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Scene Parameter", 0);

            MoveToNextScene();
        }

        private void MoveToNextScene()
        {
            OnBattleFinished?.Invoke();
        }

        // Need To be Re-Done
        private void EnemyDied()
        {
            _playerManager.PlayerWin();
            _enemyManager.EnemyAnimatorController.CharacterIsDead();
            BattleData.Instance.PlayerWon = true;
            //     SendAnalyticWhenGameEnded("player_won", battleData);
            //    AddRewards();
            //    _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Player);
            //    OnPlayerVictory?.Invoke();
        }

        private void SpawnModels(BattleData data)
        {
            ModelSO playersModelSO = data.Player.CharacterData.CharacterSO.CharacterAvatar;
            ModelSO enemyModelSO = data.Opponent.CharacterData.CharacterSO.CharacterAvatar;
            AvatarHandler avatarHandler = Instantiate(playersModelSO.Model, _playerManager.PlayerAnimatorController.transform);
            AvatarHandler opponentAvatar = Instantiate(enemyModelSO.Model, _enemyManager.EnemyAnimatorController.transform);
            if (playersModelSO == enemyModelSO)
                opponentAvatar.Mesh.material = enemyModelSO.Materials[0].Tinted;

        }
        // Need To be Re-Done
        private void PlayerDied()
        {
            //  var battleData = Account.AccountManager.Instance.BattleData;
            _playerManager.PlayerAnimatorController.CharacterIsDead();
            _enemyManager.EnemyWon();
            BattleData.Instance.PlayerWon = false;
            //      _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Enemy);
            //    SendAnalyticWhenGameEnded("player_defeated", battleData);
            OnPlayerDefeat?.Invoke();
        }



        #region MonoBehaviour Callbacks
        private void Update()
        {
            ThreadsHandler.ThreadHandler.TickThread();
        }
        private void OnDestroy()
        {
            ThreadsHandler.ThreadHandler.ResetList();
            AnimatorController.OnDeathAnimationFinished -= DeathAnimationFinished;
            _BattleStarter.OnDestroy();
            HealthStat.OnCharacterDeath -= BattleEnded;
        }

        public override void Awake()
        {
            HealthStat.OnCharacterDeath += BattleEnded;
            AnimatorController.OnDeathAnimationFinished += DeathAnimationFinished;
            base.Awake();
            _BattleStarter.Register(new OperationTask(Init, 0));
            OnSceneStart?.Invoke(_BattleStarter);
        }
        private void Start()
        {
            ResetBattle();
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






    public interface IBattleManager
    {
        DeckManager DeckManager { get; }
        PlayerManager PlayerManager { get; }
        EnemyManager EnemyManager { get; }
        CardExecutionManager CardExecutionManager { get; }
        CardUIManager CardUIManager { get; }
        ComboManager ComboManager { get; }
        KeywordManager KeywordManager { get; }
        TurnHandler TurnHandler { get; }
        VFXManager VFXManager { get; }
        CameraManager CameraManager { get; }
    }
}
