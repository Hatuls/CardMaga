﻿using Battle.Data;
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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
 
    public class BattleManager : MonoSingleton<BattleManager>
    {
        public static event Action OnGameEnded;
        public static event Action<SequenceHandler> OnSceneStart;


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

        private PlayersManager _playersManager;
        private IEnumerator _turnCycles;
        private SequenceHandler _BattleStarter = new SequenceHandler();


        private static List<ISequenceOperation> _battleStartUpOrderList = new List<ISequenceOperation>();

        public void Init(ITokenReciever token)
        {
         var _initToken = token.GetToken();

            isGameEnded = false;

            ResetParams();

            if (AudioManager.Instance != null)
                AudioManager.Instance.BattleMusicParameter();

             _initToken.Dispose();
        }

        private void ResetBattle()
        {

            _BattleStarter.Start(StartBattle);

        }

        private void ResetParams()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopAllSounds();
            isGameEnded = false;


           _playerManager.AnimatorController.ResetLayerWeight();
           _enemyManager.AnimatorController.ResetLayerWeight();
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


            _playerManager.AnimatorController.ResetLayerWeight();
            _enemyManager.AnimatorController.ResetLayerWeight();

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
            _enemyManager.AnimatorController.CharacterIsDead();
            BattleData.Instance.PlayerWon = true;
            //     SendAnalyticWhenGameEnded("player_won", battleData);
            //    AddRewards();
            //    _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Player);
            //    OnPlayerVictory?.Invoke();
        }

      
        // Need To be Re-Done
        private void PlayerDied()
        {
            //  var battleData = Account.AccountManager.Instance.BattleData;
            _playerManager.AnimatorController.CharacterIsDead();
            _enemyManager.EnemyWon();
            BattleData.Instance.PlayerWon = false;
            //      _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Enemy);
            //    SendAnalyticWhenGameEnded("player_defeated", battleData);
            OnPlayerDefeat?.Invoke();
        }



        #region Observer Pattern 

        public static void Register(ISequenceOperation battleStarter)
            => _battleStartUpOrderList.Add(battleStarter);
        public static void Remove(ISequenceOperation battleStarter)
            => _battleStartUpOrderList.Remove(battleStarter);

        private void Setup()
        {
            for (int i = 0; i < _battleStartUpOrderList.Count; i++)
                _BattleStarter.Register(_battleStartUpOrderList[i]);
        }
        #endregion




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
            _playersManager = new PlayersManager(_playerManager, _enemyManager);
        }
        private void Start()
        {
            Setup();
            ResetBattle();
        }
        #endregion
        #region Analytics

        private static void SendAnalyticWhenGameEnded(string eventName, BattleData battleData)
        {
            var characterSO = battleData.Right.CharacterData.CharacterSO;

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




    public class PlayersManager : ISequenceOperation
    {
        public IPlayer LeftCharacter { get; private set; }
        public IPlayer RightCharacter { get; private set; }

        public OrderType Order =>  OrderType.Before;
        public int Priority => -1;

        public void ExecuteTask(ITokenReciever tokenMachine)
        {
            IDisposable token = tokenMachine.GetToken();
            var battleData = BattleData.Instance;
            // assign data
            LeftCharacter.AssignCharacterData(battleData.Left);
            RightCharacter.AssignCharacterData(battleData.Right);

            //assign visuals
            var leftModel = battleData.Left.CharacterData.CharacterSO.CharacterAvatar;
            var rightModel = battleData.Right.CharacterData.CharacterSO.CharacterAvatar;

            LeftCharacter.VisualCharacter.SpawnModel(leftModel,false);
            RightCharacter.VisualCharacter.SpawnModel(rightModel, rightModel == leftModel);


            StaminaHandler.Instance.InitStaminaHandler();
            token.Dispose();
        }

        public IPlayer GetCharacter(bool IsLeftCharacter) => IsLeftCharacter ? LeftCharacter : RightCharacter;

        public PlayersManager(IPlayer leftCharacter, IPlayer rightCharacter)
        {
            LeftCharacter = leftCharacter;
            RightCharacter = rightCharacter;
            BattleManager.Register(this);
        }


    }


    
}
