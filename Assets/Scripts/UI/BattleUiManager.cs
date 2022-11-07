using Battle;
using CardMaga.Battle.Visual;
using CardMaga.Battle.Visual.Camera;
using CardMaga.Input;
using CardMaga.Keywords;
using CardMaga.SequenceOperation;
using CardMaga.UI;
using CardMaga.UI.Collections;
using CardMaga.UI.Text;
using Keywords;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Battle.UI
{
    public interface IBattleUIManager
    {
        IBattleManager BattleDataManager { get; }
        BuffIconManager BuffIconManager { get; }
        CardUIManager CardUIManager { get; }
        ComboUIManager ComboUIManager { get; }
        CraftingSlotsUIManager_V4 CraftingSlotsUIManager { get; }
        VisualCharactersManager VisualCharactersManager { get; }
        BottomPartDeckVisualHandler BottomPartDeckVisualHandler { get; }
        EndTurnButton EndTurnButton { get; }
        HandUI HandUI { get; }
        GlowManager GlowManager { get; }
        StatsUIManager StatsUIManager { get; }
    }

    public class BattleUiManager : MonoSingleton<BattleUiManager>, ISequenceOperation<IBattleManager>, IBattleUIManager
    {
        #region Fields
        [SerializeField]
        private BottomPartDeckVisualHandler _bottomPartDeckVisualHandler;
        [SerializeField]
        private HandUI _handUI;

        [SerializeField]
        private CardUIManager _cardUIManager;

        [SerializeField]
        private EndTurnButton _endTurnButton;

        [SerializeField]
        private StatsUIManager _statsUIManager;

        [SerializeField]
        private ComboUIManager _comboUIManager;

        [SerializeField]
        private BuffIconManager _buffIconManager;

        [SerializeField]
        private CraftingSlotsUIManager_V4 _craftingSlotsUIManager;

        [SerializeField]
        private VisualCharactersManager _visualCharactersManager;

        [SerializeField]
        private GlowManager _glowManager;

        [SerializeField]
        private MainInputStateMachine _mainInputStateMachine;

        [SerializeField]
        private ComboAndDeckCollectionBattleHandler _comboAndDeckCollectionBattleHandler;

        [SerializeField]
        private TurnCounter _turnCounter;

        [SerializeField]
        private StaminaTextManager _staminaTextManager;

        [SerializeField]
        private CameraManager _cameraManager;

        [SerializeField]
        private DollyTrackCinematicManager _dollyTrackCinematicManager;


        [SerializeField]
        private BattleManager _battleManager;
        private VisualKeywordsHandler _visualKeywordsHandler;
        #endregion


        #region Events


        #endregion

        #region Properties

        public IEnumerable<ISequenceOperation<IBattleUIManager>> VisualInitializers
        {
            get
            { 
                yield return VisualCharactersManager;
                yield return StatsUIManager;
                yield return VisualKeywordsHandler;
                if (MainInputStateMachine != null)
                yield return MainInputStateMachine;
                yield return CardUIManager;
                yield return GlowManager;
                yield return CameraManager;
                yield return HandUI;
                yield return ComboUIManager;
                yield return StaminaTextManager;
                yield return EndTurnButton;
                yield return BottomPartDeckVisualHandler;
                yield return BuffIconManager;
                yield return CraftingSlotsUIManager;
                yield return ComboAndDeckCollectionBattleHandler;
                yield return TurnCounter;
                if(_dollyTrackCinematicManager!=null)
                yield return DollyTrackCinematicManager;
            }
        }
        public int Priority => 1000;
        public DollyTrackCinematicManager DollyTrackCinematicManager => _dollyTrackCinematicManager;
        public MainInputStateMachine MainInputStateMachine => _mainInputStateMachine;
        public TurnCounter TurnCounter => _turnCounter;
        public HandUI HandUI { get => _handUI; }
        public CardUIManager CardUIManager { get => _cardUIManager; }
        public EndTurnButton EndTurnButton { get => _endTurnButton; }
        public StatsUIManager StatsUIManager { get => _statsUIManager; }
        public ComboUIManager ComboUIManager { get => _comboUIManager; }
        public BuffIconManager BuffIconManager { get => _buffIconManager; }
        public CraftingSlotsUIManager_V4 CraftingSlotsUIManager { get => _craftingSlotsUIManager; }
        public VisualCharactersManager VisualCharactersManager => _visualCharactersManager;
        public IBattleManager BattleDataManager => _battleManager;
        public VisualKeywordsHandler VisualKeywordsHandler => _visualKeywordsHandler;
        public GlowManager GlowManager { get => _glowManager; }
        public ComboAndDeckCollectionBattleHandler ComboAndDeckCollectionBattleHandler =>_comboAndDeckCollectionBattleHandler;
        public BottomPartDeckVisualHandler BottomPartDeckVisualHandler => _bottomPartDeckVisualHandler;
        public StaminaTextManager StaminaTextManager => _staminaTextManager;
        public CameraManager CameraManager => _cameraManager;
        #endregion

        #region UnityCallBacks
        public override void Awake()
        {
            _visualKeywordsHandler = new VisualKeywordsHandler();
            _battleManager.Register(this, OrderType.After);
        }
        #endregion
        #region Functions
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            IDisposable token = tokenMachine.GetToken();
        
            foreach (var uiElement in VisualInitializers)
                uiElement.ExecuteTask(tokenMachine, this);

            _battleManager.OnBattleManagerDestroyed += _battleManager_OnBattleManagerDestroyed;
            token.Dispose();
        }

        private void _battleManager_OnBattleManagerDestroyed(IBattleManager obj)
        {
            _battleManager.OnBattleManagerDestroyed -= _battleManager_OnBattleManagerDestroyed;
            VisualCharactersManager.Dispose(this);
        }
        #endregion



        #region Editor
#if UNITY_EDITOR
        [ContextMenu("Find Values And Assign")]
        private void AssignFields()
        {
            _mainInputStateMachine = FindObjectOfType<MainInputStateMachine>();
            _handUI = FindObjectOfType<HandUI>();
            _comboUIManager = FindObjectOfType<ComboUIManager>();
            _cardUIManager = FindObjectOfType<CardUIManager>();
            _endTurnButton = FindObjectOfType<EndTurnButton>();
            _statsUIManager = FindObjectOfType<StatsUIManager>();
            _craftingSlotsUIManager = FindObjectOfType<CraftingSlotsUIManager_V4>();
            _bottomPartDeckVisualHandler = FindObjectOfType<BottomPartDeckVisualHandler>();
            _comboAndDeckCollectionBattleHandler = FindObjectOfType<ComboAndDeckCollectionBattleHandler>();
            _turnCounter = FindObjectOfType<TurnCounter>();
            _staminaTextManager = FindObjectOfType<StaminaTextManager>();
            _cameraManager = FindObjectOfType<CameraManager>();
            _dollyTrackCinematicManager = FindObjectOfType<DollyTrackCinematicManager>();
            _glowManager = FindObjectOfType<GlowManager>();
            _mainInputStateMachine = FindObjectOfType<MainInputStateMachine>();
            _battleManager = FindObjectOfType<BattleManager>();

            _buffIconManager.AssignBuffsFields();
            _visualCharactersManager.AssignVisualCharacter();
        }
#endif
        #endregion
    }

    [Serializable]
    public class VisualCharactersManager : ISequenceOperation<IBattleUIManager>
    {
        [SerializeField]
        private VisualCharacter _leftVisualCharacter;
        [SerializeField]
        private VisualCharacter _rightVisualCharacter;

        public int Priority => 0;

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
        {

            IDisposable token = tokenMachine.GetToken();

            var playersDataManager = battleUIManager.BattleDataManager.PlayersManager;

            var leftPlayerData = playersDataManager.GetCharacter(true);
            var rightPlayerData = playersDataManager.GetCharacter(false);

            CharacterSO leftCharacterSO = leftPlayerData.CharacterSO;
            CharacterSO rightCharacterSO = rightPlayerData.CharacterSO;

            ModelSO leftModel = leftCharacterSO.CharacterAvatar;
            ModelSO rightModel = rightCharacterSO.CharacterAvatar;


            _leftVisualCharacter.InitVisuals(leftPlayerData, leftCharacterSO, false);
            _leftVisualCharacter.ExecuteTask(null, battleUIManager);

            _rightVisualCharacter.InitVisuals(rightPlayerData, rightCharacterSO, rightModel == leftModel);
            _rightVisualCharacter.ExecuteTask(null, battleUIManager);


            token.Dispose();
        }
        public void Dispose(IBattleUIManager battleUIManager)
        {
            _leftVisualCharacter.Dispose();
            _rightVisualCharacter.Dispose();
        }
        public VisualCharacter GetVisualCharacter(bool isLeft) => isLeft ? _leftVisualCharacter : _rightVisualCharacter;

#if UNITY_EDITOR
        public void AssignVisualCharacter()
        {
            var allVisualCharacters = MonoBehaviour.FindObjectsOfType<VisualCharacter>();

            for (int i = 0; i < allVisualCharacters.Length; i++)
            {
                if (allVisualCharacters[i].gameObject.name.Contains("Left"))
                    _leftVisualCharacter = allVisualCharacters[i];
                else if (allVisualCharacters[i].gameObject.name.Contains("Right"))
                    _rightVisualCharacter = allVisualCharacters[i];
                else
                    throw new Exception("More than 2 BuffIconHandler found in scene!\nCheck this object -> " + allVisualCharacters[i].gameObject.name);
            }
        }
#endif
    }
}

