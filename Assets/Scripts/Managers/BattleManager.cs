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
using CardMaga.Rules;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{

    public class BattleManager : MonoSingleton<BattleManager>, IBattleManager
    {
        public static event Action OnGameEnded;
        public event Action<BattleManager> OnBattleManagerDestroyed;
        
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

        [SerializeField]
        private DollyTrackCinematicManager _cinematicManager;
        [SerializeField]
        private PlayerManager _playerManager;
        [SerializeField]
        private EnemyManager _enemyManager;
        [SerializeField]
        private CardExecutionManager _cardExecutionManager;
        [SerializeField]
        private CardUIManager _cardUIManager;
        [SerializeField]
        private ComboManager _comboManager;
        [SerializeField]
        private KeywordManager _keywordManager;
        [SerializeField]
        private VFXManager _vFXManager;
        [SerializeField]
        private CameraManager _cameraManager;

        private EndBattleHandler _endBattleHandler;
        private RuleManager _ruleManager;
        private BattleTutorial _battleTutorial;
        private IPlayersManager _playersManager;
        private static SequenceHandler<BattleManager> _battleStarter = new SequenceHandler<BattleManager>();
        private GameTurnHandler _gameTurnHandler;

        #region Properties
        public GameTurnHandler TurnHandler => _gameTurnHandler; 
        public IPlayersManager PlayersManager => _playersManager; 
        public CardExecutionManager CardExecutionManager => _cardExecutionManager;
        public CardUIManager CardUIManager => _cardUIManager;
        public ComboManager ComboManager => _comboManager;
        public KeywordManager KeywordManager => _keywordManager;
        public VFXManager VFXManager => _vFXManager;
        public CameraManager CameraManager => _cameraManager;
        public RuleManager RuleManager => _ruleManager;
        public BattleData BattleData => BattleData.Instance;
        #endregion

        private void ResetBattle()
        {
            ResetParams();
            InitParams();
            _battleStarter.StartAll(this, StartBattle);
        }

        private void InitParams()
        {
            _endBattleHandler = new EndBattleHandler(this);
            _gameTurnHandler = new GameTurnHandler();
            _playersManager = new PlayersManager(_playerManager, _enemyManager);
            _ruleManager = new RuleManager();
            _ruleManager.InitRuleList(BattleData.BattleConfigSO.GameRule,BattleData.BattleConfigSO.EndGameRule,this
            );
            _ruleManager.OnGameEnded += EndBattle;

            if (AudioManager.Instance != null)
                AudioManager.Instance.BattleMusicParameter();
        }

        private void ResetParams()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.StopAllSounds();
            isGameEnded = false;

        
        }
        // Need To be Re-Done
        public void StartBattle()
        {
            StopAllCoroutines();
            TurnHandler.Start();
            BattleData.Instance.PlayerWon = false;

            OnBattleStarts?.Invoke();
        }

        private void EndBattle(bool isLeftPlayerWon)
        {
            _endBattleHandler.EndBattle(isLeftPlayerWon);
            OnGameEnded?.Invoke();
        }
        
        // Need To be Re-Done
        public void BattleEnded(bool isPlayerDied)
        {
            if (isGameEnded == true)
                return;

            //    UI.StatsUIManager.Instance.UpdateHealthBar(isPlayerDied, 0);
         _cardExecutionManager.ResetExecution();
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
            BattleData.PlayerWon = true;
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
            BattleData.PlayerWon = false;
            //      _cameraController.MoveCameraAnglePos((int)CameraController.CameraAngleLookAt.Enemy);
            //    SendAnalyticWhenGameEnded("player_defeated", battleData);
            OnPlayerDefeat?.Invoke();
        }

        private void CreateTutorial(ITokenReciever tokenReciever, BattleManager battleManager)
        {
            if (BattleData.BattleConfigSO.BattleTutorial == null)
                return;

            _battleTutorial = Instantiate(BattleData.BattleConfigSO.BattleTutorial);
        }

        #region Observer Pattern 

        public static void Register(ISequenceOperation<BattleManager> battleStarter, OrderType order)
            => _battleStarter.Register(battleStarter, order);
        public static bool Remove(ISequenceOperation<BattleManager> battleStarter, OrderType order)
            => _battleStarter.Remove(battleStarter, order);


        #endregion
        
        #region MonoBehaviour Callbacks
        private void Update()
        {
            ThreadsHandler.ThreadHandler.TickThread();
        }
        
        private void OnDestroy()
        {
            OnBattleManagerDestroyed?.Invoke(this);
            ThreadsHandler.ThreadHandler.ResetList();
            
            _ruleManager.OnGameEnded -= EndBattle;

            AnimatorController.OnDeathAnimationFinished -= DeathAnimationFinished;
            _battleStarter.Dispose();
           
            HealthStat.OnCharacterDeath -= BattleEnded;
            TurnHandler.Dispose();
            _gameTurnHandler = null;
        }

        public override void Awake()
        {
            Register(new OperationTask<BattleManager>(CreateTutorial, 0, OrderType.After),OrderType.After);
            HealthStat.OnCharacterDeath += BattleEnded;
            AnimatorController.OnDeathAnimationFinished += DeathAnimationFinished;
            base.Awake();
        }
        
        private void Start()
        {
            ResetBattle();
           
        }
        
        #endregion
        
        #region Analytics

        private void SendAnalyticWhenGameEnded(string eventName, BattleData battleData)
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
        => _playersManager.GetCharacter(false).StatsHandler?.RecieveDamage(1000000);

        [Button]
        public void KillPlayer()
           => _playersManager.GetCharacter(true).StatsHandler?.RecieveDamage(1000000);
#endif
        #endregion
    }

    
    public interface IBattleManager
    {
        BattleData BattleData { get; }
        IPlayersManager PlayersManager { get; }
        CardExecutionManager CardExecutionManager { get; }
        CardUIManager CardUIManager { get; }
        ComboManager ComboManager { get; }
        KeywordManager KeywordManager { get; }
        GameTurnHandler TurnHandler { get; }
        VFXManager VFXManager { get; }
        CameraManager CameraManager { get; }
        RuleManager RuleManager { get; }
    }

    public interface IPlayersManager
    {
        IPlayer LeftCharacter { get; }
        IPlayer RightCharacter { get; }

        IPlayer GetCharacter(bool IsLeftCharacter);
    }

    public class PlayersManager : ISequenceOperation<BattleManager>, IPlayersManager
    {
        private IPlayer _leftCharacter;
        private IPlayer _rightCharacter;

        public IPlayer LeftCharacter { get => _leftCharacter; private set => _leftCharacter = value; }
        public IPlayer RightCharacter { get => _rightCharacter; private set => _rightCharacter = value; }

        public int Priority => -1;
        /// <summary>
        /// Return the left or right character manager
        /// </summary>
        /// <param name="IsLeftCharacter"></param>
        /// <returns></returns>
        public IPlayer GetCharacter(bool IsLeftCharacter) => IsLeftCharacter ? LeftCharacter : RightCharacter;
        
        public PlayersManager(IPlayer leftCharacter, IPlayer rightCharacter)
        {
            LeftCharacter = leftCharacter;
            RightCharacter = rightCharacter;
            BattleManager.Register(this, OrderType.Before);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, BattleManager battleManager)
        {
            IDisposable token = tokenMachine.GetToken();
            BattleData battleData = battleManager.BattleData;
            // assign data
            LeftCharacter.AssignCharacterData(battleManager, battleData.Left);
            RightCharacter.AssignCharacterData(battleManager, battleData.Right);

            //assign visuals
            CharacterSO leftCharacter =  battleData.Left.CharacterData.CharacterSO;
            CharacterSO rightCharacter = battleData.Right.CharacterData.CharacterSO;
            ModelSO leftModel = leftCharacter.CharacterAvatar;
            ModelSO rightModel = rightCharacter.CharacterAvatar;

            LeftCharacter.VisualCharacter.InitVisuals(LeftCharacter, leftCharacter, false, battleManager.TurnHandler.GetCharacterTurn(LeftCharacter.IsLeft));
            RightCharacter.VisualCharacter.InitVisuals(RightCharacter, rightCharacter, rightModel == leftModel, battleManager.TurnHandler.GetCharacterTurn(RightCharacter.IsLeft));

            token.Dispose();
        }
    }
}
