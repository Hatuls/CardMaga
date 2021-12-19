using Account.GeneralData;
using Meta.Resources;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.PlayScreen
{
  [System.Serializable]
public class  UshortEvent : UnityEvent<ushort> { }
    public class PlayScreenUI : TabAbst
    {
        #region Singleton
        private static PlayScreenUI _instance;
        public static PlayScreenUI Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("CardManager is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion
        #region Fields

        [SerializeField]
        int _delayBeforeStart = 1000;
        [SerializeField]
        GameObject _backgroundPanel;
        PlayPackage _playpackage = new PlayPackage();
        [SerializeField] ushort _energyCost = 5;
        [SerializeField]
        SceneLoaderCallback _sceneLoad;
        #endregion

        [SerializeField]
        UshortEvent OnSuccessfullPlayClick;
        [SerializeField]
        UnityEvent OnUnSuccessfullPlayClick;

        #region Properties

        public PlayPackage playPackage => _playpackage;
        #endregion
        #region Public Methods
        private void Start()
        {
            if (Account.AccountManager.Instance.IsDoneTutorial)
            {
                var account = Account.AccountManager.Instance;
                _playpackage.CharacterData = account.AccountCharacters.GetCharacterData(CharacterEnum.Chiara);
                _playpackage.Deck = _playpackage.CharacterData.GetDeckAt(0);
                _playpackage.SendPackage();
                account.BattleData.Opponent = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(Battles.CharacterTypeEnum.Tutorial);
                _sceneLoad.LoadScene(SceneHandler.ScenesEnum.GameBattleScene);
                account.IsDoneTutorial = false;
            }
        }
        public void ResetPlayScreen()
        {
            BGPanelSetActiveState(true);
        }

        public void BGPanelSetActiveState(bool isOn)
        {
            _backgroundPanel.SetActive(isOn);
        }
        public void CharacterChoosen(CharacterData character)
        {
            _playpackage.CharacterData = character;

            ResetPlayScreen();

        }
        public void DeckChoosen(AccountDeck deck)
        {
            _playpackage.Deck = deck;

            ResetPlayScreen();

        }
        public void ConfirmPlayPackage()
        {
            _playpackage.SendPackage();
            _sceneLoad.LoadScene(SceneHandler.ScenesEnum.MapScene);
        }

        public void OnPlayClicked()
        {
              EnergyHandler energyHandler = (EnergyHandler)ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy);

            if (energyHandler.HasAmount(energyHandler.AmountToStartPlay))
            {
                OnSuccessfullPlayClick?.Invoke(_energyCost);
                energyHandler.ReduceAmount(_energyCost);
                StartGameDelay();
            }
            else
            {
                OnUnSuccessfullPlayClick.Invoke();
            }
        }

        private async void StartGameDelay()
        {
            await System.Threading.Tasks.Task.Delay(_delayBeforeStart);
            StartGame();
        }
        private void StartGame()
        {
            var account = Account.AccountManager.Instance;
            _playpackage.CharacterData = account.AccountCharacters.GetCharacterData(CharacterEnum.Chiara);
            _playpackage.Deck = _playpackage.CharacterData.GetDeckAt(0);
            ConfirmPlayPackage();
      
        }
        #region Tab Implementation
        public override void Open()
        {
            BGPanelSetActiveState(true);
            gameObject.SetActive(true);
            OnSuccessfullPlayClick.AddListener(ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy).ReduceAmount);
        
        }

        public override void Close()
        {
            BGPanelSetActiveState(false);
            gameObject.SetActive(false);
            OnSuccessfullPlayClick.RemoveListener(ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy).ReduceAmount);
        }
        #endregion
        #endregion
    }
}
