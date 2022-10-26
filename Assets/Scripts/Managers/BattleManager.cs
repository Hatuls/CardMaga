using Battle.Data;
using CardMaga.SequenceOperation;
using Battle.Turns;
using CardMaga.Battle.UI;
using Keywords;
using Managers;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using Account;
using CardMaga.Rules;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    public class BattleManager : MonoSingleton<BattleManager>, IBattleManager
    {
        public static event Action OnGameEnded;
        public event Action<IBattleManager> OnBattleManagerDestroyed;
        
        public static bool isGameEnded;
        [SerializeField, EventsGroup]
        private Unity.Events.StringEvent _playSound;
        [SerializeField, EventsGroup]
        private UnityEvent OnBattleStarts;
        [SerializeField,EventsGroup]
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
        [SerializeField] private EndBattleHandler _endBattleHandler;
        
#if UNITY_EDITOR
        [Header("Editor:")]
        [SerializeField] private bool _hideTutorial;
#endif
        
        private RuleManager _ruleManager;
        private BattleTutorial _battleTutorial;
        private IPlayersManager _playersManager;
        private static SequenceHandler<IBattleManager> _battleStarter = new SequenceHandler<IBattleManager>();
        private GameTurnHandler _gameTurnHandler;
        private bool _isInTutorial;


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

        public MonoBehaviour MonoBehaviour => this;
        #endregion

        #region BattleManagnent

        private void ResetBattle()
        {
            ResetParams();
            InitParams();
            _battleStarter.StartAll(this, StartBattle);
        }

        private void InitParams()
        {
            _gameTurnHandler = new GameTurnHandler(BattleData.BattleConfigSO.CharacterSelecter.GetTurnType());
            _playersManager = new PlayersManager(_playerManager, _enemyManager);
            
            _ruleManager = new RuleManager();
            _endBattleHandler = new EndBattleHandler(this);

            _isInTutorial = !(BattleData.BattleConfigSO.BattleTutorial == null);
            
            _endBattleHandler.OnBattleEnded += EndBattle;
            _endBattleHandler.OnBattleAnimatonEnd += MoveToNextScene;

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

        #endregion
        
        #region EndBattleLogic

        private void EndBattle(bool isLeftPlayerWon)
        {
            OnGameEnded?.Invoke();
        }

        private void MoveToNextScene()
        {
            OnBattleFinished?.Invoke();
        }

        #endregion
        
          private void CreateTutorial(ITokenReciever tokenReciever, IBattleManager battleManager)
    {
#if UNITY_EDITOR
        if (_hideTutorial)
            return;
#endif
            
            if (BattleData.BattleConfigSO?.BattleTutorial == null)
                return;

            _battleTutorial = Instantiate(BattleData.BattleConfigSO.BattleTutorial);
            _battleTutorial.StartTutorial();
    }
        
        #region Observer Pattern 

        public static void Register(ISequenceOperation<IBattleManager> battleStarter, OrderType order)
            => _battleStarter.Register(battleStarter, order);
        public static bool Remove(ISequenceOperation<IBattleManager> battleStarter, OrderType order)
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
            _ruleManager.DisposeRules();
            _endBattleHandler.OnBattleEnded -= EndBattle;
            _endBattleHandler.Dispose();
            
            _battleStarter.Dispose();
            
            TurnHandler.Dispose();
            _gameTurnHandler = null;
        }

        public override void Awake()
        {
            Register(new OperationTask<IBattleManager>(CreateTutorial, 0, OrderType.After), OrderType.After);

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
        event Action<IBattleManager> OnBattleManagerDestroyed;
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
        MonoBehaviour MonoBehaviour { get; }
    }

    public interface IPlayersManager
    {
        IPlayer LeftCharacter { get; }
        IPlayer RightCharacter { get; }

        IPlayer GetCharacter(bool IsLeftCharacter);
    }

    public class PlayersManager : ISequenceOperation<IBattleManager>, IPlayersManager
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

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager battleManager)
        {
            IDisposable token = tokenMachine.GetToken();
            BattleData battleData = battleManager.BattleData;
            // assign data
            LeftCharacter.AssignCharacterData(battleManager, battleData.Left);
            RightCharacter.AssignCharacterData(battleManager, battleData.Right);

            //assign visuals
            CharacterSO leftCharacterSO =  battleData.Left.CharacterData.CharacterSO;
            CharacterSO rightCharacterSO = battleData.Right.CharacterData.CharacterSO;

            ModelSO leftModel = leftCharacterSO.CharacterAvatar;
            ModelSO rightModel = rightCharacterSO.CharacterAvatar;

            LeftCharacter.VisualCharacter.InitVisuals(LeftCharacter, leftCharacterSO, false);
            RightCharacter.VisualCharacter.InitVisuals(RightCharacter, rightCharacterSO, rightModel == leftModel);

            token.Dispose();
        }
    }
}
