using Account.GeneralData;
using Meta.Resources;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class PlayScreenUI : MonoBehaviour
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
        ChooseDeckScreen _chooseDeckScreen;
        [SerializeField]
        ChooseCharacterScreen _chooseCharacterScreen;
        [SerializeField]
        GameObject _backgroundPanel;
        PlayPackage _playpackage = new PlayPackage();

        [SerializeField]
        SceneLoaderCallback _sceneLoad;
        #endregion
        #region Properties
        public ChooseDeckScreen ChooseDeckScreen => _chooseDeckScreen;
        public PlayPackage playPackage => _playpackage;
        #endregion
        #region Public Methods
        public void OpenPlayScreen()
        {

        }
        public void ResetPlayScreen()
        {
            _chooseCharacterScreen.ResetCharacterScreen();
            _chooseDeckScreen.ResetChooseDeckScreen();
            _chooseDeckScreen.ResetShowCardsScreen();
            _chooseDeckScreen.ResetShowComboScreen();
            BGPanelSetActiveState(false);
        }
        public void OnPlayClicked()
        {
            EnergyHandler energyHandler = (EnergyHandler)ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy);

            if (energyHandler.HasAmount(energyHandler.AmountToStartPlay))
            {
                Debug.Log("Activating Character Panel");
                BGPanelSetActiveState(true);
                _chooseCharacterScreen.Init();
            }
        }
        public void BGPanelSetActiveState(bool isOn)
        {
            _backgroundPanel.SetActive(isOn);
        }
        public void CharacterChoosen(CharacterData character)
        {
            _playpackage.CharacterData = character;

            ResetPlayScreen();
            ChooseDeckScreen.InitChooseDeckScreen();
        }
        public void DeckChoosen(AccountDeck deck)
        {
            _playpackage.Deck = deck;

            ResetPlayScreen();
            ChooseDeckScreen.ShowDeckCards(deck);
        }
        public void ConfirmPlayPackage()
        {
            ResetPlayScreen();

            EnergyHandler energyHandler = (EnergyHandler)ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy);
            energyHandler.ReduceAmount(5);
            _playpackage.SendPackage();
            _sceneLoad.LoadScene(SceneHandler.ScenesEnum.MapScene);
        }
        #endregion
    }
}
