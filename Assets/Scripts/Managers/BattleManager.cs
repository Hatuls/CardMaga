using Battle;
using Battle.Data;
using Battle.Turns;
using CardMaga.Battle.Combo;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Battle.UI;
using CardMaga.Commands;
using CardMaga.Keywords;
using CardMaga.Rules;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.Battle
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
        [SerializeField, EventsGroup]
        private UnityEvent OnBattleFinished;

        [SerializeField]
        private EnemyManager _enemyManager;


        [SerializeField]
        private BattleUiManager _battleUiManager;



#if UNITY_EDITOR
        [Header("Editor:")]
        [SerializeField] private bool _hideTutorial;
#endif
        private ComboManager _comboManager;
        private KeywordManager _keywordManager;
        private EndBattleHandler _endBattleHandler;
        private CardExecutionManager _cardExecutionManager;
        private PlayerManager _playerManager;
        private RuleManager _ruleManager;
        private BattleTutorial _battleTutorial;
        private TurnHandler _gameTurnHandler;
        private GameCommands _gameCommands;
        private IPlayersManager _playersManager;
        private static SequenceHandler<IBattleManager> _battleStarter = new SequenceHandler<IBattleManager>();
        private bool _isInTutorial;

        #region Properties
        public IPlayersManager PlayersManager => _playersManager;
        public IBattleUIManager BattleUIManager => _battleUiManager;
        public CardExecutionManager CardExecutionManager => _cardExecutionManager;
        public TurnHandler TurnHandler => _gameTurnHandler;
        public ComboManager ComboManager => _comboManager;
        public KeywordManager KeywordManager => _keywordManager;
        public GameCommands GameCommands => _gameCommands;
        public RuleManager RuleManager => _ruleManager;
        public BattleData BattleData => BattleData.Instance;
        public EndBattleHandler EndBattleHandler => _endBattleHandler;

        public MonoBehaviour MonoBehaviour => this;

        #endregion

        #region BattleManagment

        private void ResetBattle()
        {
            ResetParams();
            InitParams();
            _battleStarter.StartAll(this, StartBattle);
        }

        private void InitParams()
        {
            _gameCommands = new GameCommands(this);
            _gameTurnHandler = new TurnHandler(BattleData.BattleConfigSO.CharacterSelecter.GetTurnType());
            _playerManager = new PlayerManager();
            _playersManager = new PlayersManager(this, _playerManager, _enemyManager);
            _keywordManager = new KeywordManager(this);
            _ruleManager = new RuleManager(this);
            _endBattleHandler = new EndBattleHandler(this);
            _cardExecutionManager = new CardExecutionManager(this);
            _comboManager = new ComboManager(this);


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
        public void Register(ISequenceOperation<IBattleManager> sequenceOperation, OrderType orderType)
        {
            _battleStarter.Register(sequenceOperation, orderType);
        }
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
            this.Register(new OperationTask<IBattleManager>(CreateTutorial, 0, OrderType.After), OrderType.After);

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
        {
            var keywordFactory = Factory.GameFactory.Instance.KeywordFactoryHandler;
            var keywordSO = keywordFactory.GetKeywordSO(KeywordType.Attack);
            var keywordCommand = new CardsKeywordsCommands(new KeywordData[1] { new KeywordData(keywordSO, TargetEnum.Opponent, 100000, 0) }, CommandType.Instant);
            keywordCommand.Init(_playerManager.IsLeft, PlayersManager, KeywordManager);
            GameCommands.GameDataCommands.DataCommands.AddCommand(keywordCommand);

        }

        [Button]
        public void KillPlayer()
        {
            var keywordFactory = Factory.GameFactory.Instance.KeywordFactoryHandler;
            var keywordSO = keywordFactory.GetKeywordSO(KeywordType.Attack);
            var keywordCommand = new CardsKeywordsCommands(new KeywordData[1] { new KeywordData(keywordSO, TargetEnum.MySelf, 100000, 0) }, CommandType.Instant);
            keywordCommand.Init(_playerManager.IsLeft, PlayersManager, KeywordManager);
            GameCommands.GameDataCommands.DataCommands.AddCommand(keywordCommand);

        }

#endif
        #endregion
    }


    public interface IBattleManager
    {
        event Action<IBattleManager> OnBattleManagerDestroyed;
        IBattleUIManager BattleUIManager { get; }
        BattleData BattleData { get; }
        IPlayersManager PlayersManager { get; }
        CardExecutionManager CardExecutionManager { get; }
        EndBattleHandler EndBattleHandler { get; }
        ComboManager ComboManager { get; }
        KeywordManager KeywordManager { get; }
        TurnHandler TurnHandler { get; }
        RuleManager RuleManager { get; }
        MonoBehaviour MonoBehaviour { get; }
        GameCommands GameCommands { get; }
        void Register(ISequenceOperation<IBattleManager> sequenceOperation, OrderType orderType);
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

        public PlayersManager(IBattleManager battleManager, IPlayer leftCharacter, IPlayer rightCharacter)
        {
            LeftCharacter = leftCharacter;
            RightCharacter = rightCharacter;

            battleManager.Register(this, OrderType.Before);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager battleManager)
        {
            IDisposable token = tokenMachine.GetToken();
            BattleData battleData = battleManager.BattleData;
            // assign data
            LeftCharacter.AssignCharacterData(battleManager, battleData.Left);
            RightCharacter.AssignCharacterData(battleManager, battleData.Right);

            //assign visuals
            CharacterSO leftCharacterSO = battleData.Left.CharacterData.CharacterSO;
            CharacterSO rightCharacterSO = battleData.Right.CharacterData.CharacterSO;

            ModelSO leftModel = leftCharacterSO.CharacterAvatar;
            ModelSO rightModel = rightCharacterSO.CharacterAvatar;

            //LeftCharacter.VisualCharacter.InitVisuals(LeftCharacter, leftCharacterSO, false);
            //RightCharacter.VisualCharacter.InitVisuals(RightCharacter, rightCharacterSO, rightModel == leftModel);

            token.Dispose();
        }
    }
}
