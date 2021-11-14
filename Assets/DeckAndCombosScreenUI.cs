using DesignPattern;
using UnityEngine;
namespace Map.UI
{

    public class DeckAndCombosScreenUI : MonoBehaviour, IObserver
    {
        [SerializeField] GameObject _mainPanel;
        [SerializeField] GameObject _comboSelectionPanel;
        [SerializeField] GameObject _cardPanel;
        [SerializeField] GameObject _comboPanel;
        [SerializeField] GameObject _deckSelectionPanel;
        [SerializeField] CanvasGroup _canvasGroup;
        [SerializeField] ObserverSO observerSO;
        [SerializeField]
        ComboUIFilterScreen _comboUIFilter;
        [SerializeField]
        CardUIFilterScreen _cardUIFilter;
        public void Open()
        {
            observerSO.Notify(this);
            _canvasGroup.blocksRaycasts = true;
            OpenCardCollectionScreen();
            _mainPanel.SetActive(true);
        }

        public void Close()
        {
            observerSO.Notify(null);
            _canvasGroup.blocksRaycasts = false;
            _mainPanel.SetActive(false);
            OpenCardCollectionScreen();
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
                _comboUIFilter.ShowAllCombos();
            _comboSelectionPanel.SetActive(toActivate);
            _comboPanel.SetActive(toActivate);
        }
        private void TurnCardCollection(bool toActivate)
        {
            if (toActivate)
                _cardUIFilter.ShowAllCards();

            _deckSelectionPanel.SetActive(toActivate);
            _cardPanel.SetActive(toActivate);
        }

        public void OnNotify(IObserver Myself)
        {
            if ((Object)Myself != this)
                _canvasGroup.blocksRaycasts = false;

        }
    }

}