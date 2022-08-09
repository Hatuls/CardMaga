using Battle.Data;
using Battle.Turns;
using Characters.Stats;
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


        private IEnumerator _turnCycles;
        private IDisposable _initToken;
        private BattleStarter _BattleStarter = new BattleStarter();




        public override void Init(ITokenReciever token)
        {
            _initToken = token.GetToken();

            ResetBattle();
            if (AudioManager.Instance != null)
                AudioManager.Instance.BattleMusicParameter();


        }

        private void ResetBattle()
        {

            isGameEnded = false;

            ResetParams();
            AssignParams();
            _initToken?.Dispose();
        }
        // Need To be Re-Done
        private void AssignParams()
        {

            var battleData = BattleData.Instance;
            SpawnModels(battleData);
            PlayerManager.Instance.AssignCharacterData(battleData.Player);
            EnemyManager.Instance.AssignCharacterData(battleData.Opponent);

            //  if (battleData.Player.CharacterData.CharacterStats.Health <= 0)
            //      throw new Exception("Battle data was not work correctly!");



            //// remove this later
            //if (EndTurnButton._OnFinishTurnPress != null)
            //    EndTurnButton.OnFinishTurnPress -= TurnHandler.OnFinishTurn;



            StaminaHandler.Instance.InitStaminaHandler();
        }
        private void ResetParams()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopAllSounds();
            isGameEnded = false;


            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.Instance.EnemyAnimatorController.ResetLayerWeight();
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


            PlayerManager.Instance.PlayerAnimatorController.ResetLayerWeight();
            EnemyManager.Instance.EnemyAnimatorController.ResetLayerWeight();

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
            PlayerManager.Instance.PlayerWin();
            EnemyManager.Instance.EnemyAnimatorController.CharacterIsDead();
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
            AvatarHandler avatarHandler = Instantiate(playersModelSO.Model, PlayerManager.Instance.PlayerAnimatorController.transform);
            AvatarHandler opponentAvatar = Instantiate(enemyModelSO.Model, EnemyManager.Instance.EnemyAnimatorController.transform);
            if (playersModelSO == enemyModelSO)
                opponentAvatar.Mesh.material = enemyModelSO.Materials[0].Tinted;

        }
        // Need To be Re-Done
        private void PlayerDied()
        {
            //  var battleData = Account.AccountManager.Instance.BattleData;
            PlayerManager.Instance.PlayerAnimatorController.CharacterIsDead();
            EnemyManager.Instance.EnemyWon();
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
            BattleStarter.Register(new SequenceOperation(Init, 0));
        }
        private void Start()
        {
            _BattleStarter.Start(StartBattle);
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



    public class BattleStarter
    {
        public enum BattleStarterOperationType { Early, Start, Late }
        private static OperationHandler<ISequenceOperation> _earlySceneStart = new OperationHandler<ISequenceOperation>();
        private static OperationHandler<ISequenceOperation> _sceneStart = new OperationHandler<ISequenceOperation>();
        private static OperationHandler<ISequenceOperation> _lateSceneStart = new OperationHandler<ISequenceOperation>();
        private static OperationHandler<ISequenceOperation> Get(BattleStarterOperationType type)
        {
            switch (type)
            {
                case BattleStarterOperationType.Start:
                    return _sceneStart;
                case BattleStarterOperationType.Late:
                    return _lateSceneStart;
                case BattleStarterOperationType.Early:
                default:
                    return _earlySceneStart;
            }
        }



        public static void Register(ISequenceOperation sequenceOperation, BattleStarterOperationType to = BattleStarterOperationType.Early)
        => Get(to).Add(sequenceOperation);

        public static void Remove(ISequenceOperation sequenceOperation, BattleStarterOperationType from = BattleStarterOperationType.Early)
             => Get(from).Remove(sequenceOperation);


        public void Start(Action OnComplete = null)
        {
            StartOperation(Get(BattleStarterOperationType.Early), StartScene);
            void StartScene() => StartOperation(Get(BattleStarterOperationType.Start), LateStartScene);
            void LateStartScene() => StartOperation(Get(BattleStarterOperationType.Late), OnComplete);
        }

        private void StartOperation(OperationHandler<ISequenceOperation> operation, Action OnComplete)
        {
            TokenMachine tokenMachine = new TokenMachine(OnComplete);
            using (tokenMachine.GetToken())
            {
                foreach (var item in operation)
                    item.Invoke(tokenMachine);
            }
        }
        public void OnDestroy()
        {
            Reset(_earlySceneStart);
            Reset(_sceneStart);
            Reset(_lateSceneStart);
            _earlySceneStart = null;
            _sceneStart = null;
            _lateSceneStart = null;

            void Reset(OperationHandler<ISequenceOperation> operation)
            {
                operation.Clear();
                operation.Dispose();
            }
        }
    }
}
