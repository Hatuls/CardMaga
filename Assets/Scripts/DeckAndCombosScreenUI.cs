using DesignPattern;
using UnityEngine;
namespace CardMaga.UI
{

    public class DeckAndCombosScreenUI : MonoBehaviour, IObserver
    {
        [SerializeField] GameObject _mainPanel;
        [SerializeField] GameObject _comboSelectionPanel;
        [SerializeField] GameObject _cardPanel;
        [SerializeField] GameObject _comboPanel;
        [SerializeField] GameObject _cardTypeSelectionPanel;
        [SerializeField] GameObject _deckCardsSelectionPanel;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] ObserverSO observerSO;
        [SerializeField]
        MetaComboUIFilterScreen _comboUIFilter;
        [SerializeField]
        MetaCardUIFilterScreen _cardUIFilter;

        [SerializeField] GameObject _container;
        [SerializeField] GameObject _btnContainer;
        [SerializeField]
        ShowAllCards _sort;

        public enum DefaultScreen
        {
            Cards,
            Combos
        }
        public enum ScreenMode
        {
            OpenLastScreenSeen,
            AlwaysOpenCombos,
            AlwaysOpenCards
        }

        [SerializeField]
        ScreenMode _mode;
        [SerializeField]
        DefaultScreen _defaultScreen;

        public void Open()
        {
            observerSO.Notify(this);
            _canvasGroup.blocksRaycasts = true;
            OpenScreenMode();
            _container?.SetActive(true);
            _btnContainer?.SetActive(true);
            _mainPanel.SetActive(true);
        }
        private void OpenScreenMode()
        {
            bool toRemember = false;
            switch (_mode)
            {
                case ScreenMode.OpenLastScreenSeen:
                    toRemember = true;
                    break;

                case ScreenMode.AlwaysOpenCombos:
                    _defaultScreen = DefaultScreen.Combos;
                    break;

                case ScreenMode.AlwaysOpenCards:
                    _defaultScreen = DefaultScreen.Cards;
                    break;


                default:
                    break;
            }
            OpenScreen(_defaultScreen, toRemember);
        }
        private void OpenScreen(DefaultScreen defaultScreen, bool toRemember)
        {
            switch (defaultScreen)
            {
                case DefaultScreen.Cards:
                    if (toRemember)
                        _defaultScreen = DefaultScreen.Cards;
                    OpenCardCollectionScreen();
                    break;
                case DefaultScreen.Combos:
                    if (toRemember)
                        _defaultScreen = DefaultScreen.Combos;
                    OpenComboCollectionScreen();
                    break;
                default:
                    throw new System.Exception("OpenScreen: Mode Is not valid! " + defaultScreen);
            }

        }

        public void Close()
        {
            _container?.SetActive(false);
            _btnContainer?.SetActive(false);
            observerSO.Notify(null);
            _canvasGroup.blocksRaycasts = false;
            _mainPanel.SetActive(false);
        }

        public void OpenCardCollectionScreen()
        {
            TurnCardCollection(true);
            TurnComboCollection(false);
        }

        public void OpenComboCollectionScreen()
        {
            TurnCardCollection(false);
            TurnComboCollection(true);
        }
        private void TurnComboCollection(bool toActivate)
        {
            
            if (toActivate)
                _comboUIFilter.ShowAll();
            _comboSelectionPanel.SetActive(toActivate);
     
            _comboPanel.SetActive(toActivate);
        }
        private void TurnCardCollection(bool toActivate)
        {
            if (toActivate)
                _cardUIFilter.SortBy(_sort);

            _cardTypeSelectionPanel.SetActive(toActivate);
            _cardPanel.SetActive(toActivate);

            //if (SceneHandler.CurrentScene == SceneHandler.ScenesEnum.GameBattleScene)
            //{
            //    _deckCardsSelectionPanel.SetActive(toActivate);
            //    _cardTypeSelectionPanel.SetActive(false);
            //}
            //else
                _cardTypeSelectionPanel.SetActive(toActivate);
        }

        public void OnNotify(IObserver Myself)
        {
            if ((Object)Myself != this)
                _canvasGroup.blocksRaycasts = false;

        }
    }

}