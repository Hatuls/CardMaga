using Account.GeneralData;
using Meta.Resources;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.PlayScreen
{
    [System.Serializable]
    public class UshortEvent : UnityEvent<ushort> { }
    public class PlayScreenUI : TabAbst
    {



        #region Fields
        private static PlayScreenUI _instance;

        [SerializeField]
        private float _delayBeforeStart = 1;
        [SerializeField]
        private GameObject _backgroundPanel;
        PlayPackage _playpackage = new PlayPackage();
        [SerializeField]
        private ushort _energyCost = 5;
        [SerializeField]
        private SceneIdentificationSO _sceneLoad;
        [SerializeField]
        private PlayButtonUI _playBtn;
        private ISceneHandler _sceneManager;
        [SerializeField]
        private SceneIdentificationSO _mapScene;
        #endregion

        [SerializeField, EventsGroup]
        UshortEvent OnSuccessfullPlayClick;
        [SerializeField, EventsGroup]
        UnityEvent OnUnSuccessfullPlayClick;


        #region Properties
        public static PlayScreenUI Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("CardManager is null!");

                return _instance;
            }
        }

        public PlayPackage playPackage => _playpackage;
        #endregion
        #region Public Methods


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
        }

        public void OnPlayClicked()
        {
            EnergyHandler energyHandler = (EnergyHandler)ResourceManager.Instance.GetResourceHandler<int>(ResourceType.Energy);

            if (energyHandler.HasAmount(energyHandler.AmountToStartPlay))
            {
                OnSuccessfullPlayClick?.Invoke(_energyCost);
                // energyHandler.ReduceAmount(_energyCost); 
                StartCoroutine(StartGameDelay());

            }
            else
            {
                OnUnSuccessfullPlayClick.Invoke();
            }
        }
        #endregion
        #region Private Methods


        private void Inject(ISceneHandler sceneHandler)
            => _sceneManager = sceneHandler;
        private void SentAnalyticEvent()
        {
            const string eventName = "pressed_play_button";
            UnityAnalyticHandler.SendEvent(eventName);
            FireBaseHandler.SendEvent(eventName);
        }

        private IEnumerator StartGameDelay()
        {
            GatherCharacterDataForRun();
            SentAnalyticEvent();
            yield return new WaitForSeconds(_delayBeforeStart);
            GatherCharacterDataForRun();
            ConfirmPlayPackage();
   
            _sceneManager.MoveToScene(_mapScene);
        }
        // Need To be Re-Done
        private void GatherCharacterDataForRun()
        {
            //var account = Account.AccountManager.Instance;
            //_playpackage.CharacterData = account.AccountCharacters.GetCharacterData(CharacterEnum.Chiara);
            //_playpackage.Deck = _playpackage.CharacterData.GetDeckAt(0);
        }


        #endregion


        #region Tab Implementation
        public override void Open()
        {
            BGPanelSetActiveState(true);
            gameObject.SetActive(true);
            OnSuccessfullPlayClick.RemoveAllListeners();
            OnSuccessfullPlayClick.AddListener(ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy).ReduceAmount);
            _playBtn.FinishedCycle();
        }

        public override void Close()
        {
            _playBtn.ResetAnimation();
            BGPanelSetActiveState(false);
            gameObject.SetActive(false);
            OnSuccessfullPlayClick.RemoveListener(ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy).ReduceAmount);
        }
        #endregion

        #region Monobehaviour Callbacks
        private void Awake()
        {
            _instance = this;
            SceneHandler.OnSceneHandlerActivated += Inject;
        }
        private void OnDestroy()
        {
        SceneHandler.OnSceneHandlerActivated -= Inject;
            
        }
    }
    #endregion
}

