using Battles.UI;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.Laboratory
{
    public class DeckCollectionScreenUI : TabAbst
    {
        #region Fields

        [SerializeField]
        MetaCardUIFilterScreen _deckScreen;
        [SerializeField]
        MetaCardUIFilterScreen _allCardsScreen;
        [SerializeField]
        CardUIInteractionHandle _cardUIInteractionHandle;
        [SerializeField]
        UnityEvent OnOpenDeckScreen;

        [SerializeField]
        GameObject _selectedCardUIContainer;
        [SerializeField]
        MetaCardUIHandler _selectedCardUI;
        #endregion
        #region Public Methods



        public void DisableInteractions()
        {
            DisableDeckCollectionInteraction();
        }

        public void OpenDeckCollection()
        {
            DisableInteractions();
        }
        private void DisableDeckCollectionInteraction()
        {
            MetaCardUIInteractionPanel.OnOpenInteractionScreen?.Invoke();
        }

        #endregion

        #region Interface

        public override void Open()
        {
            DisableInteractions();
            ResetToDefault();
            gameObject.SetActive(true);

        }

        private void ResetToDefault()
        {
            OnOpenDeckScreen?.Invoke();
            DefaultSettings();
            _selectedCardUI.OnCardClicked.RemoveAllListeners();
            _selectedCardUIContainer.SetActive(false);
            SetMainCardCollectionActiveState(true);
        }

        private void CardSelected(CardUI card)
        {
            SetMainCardCollectionActiveState(false);
            DisableInteractions();
            _selectedCardUI.CardUI.GFX.SetCardReference(card.GFX.GetCardReference);
            SetCardsToWaitForInputState(true);
            _selectedCardUI.MetaCardUIInteraction.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Remove, RemoveSelectedCardUI);
            _selectedCardUIContainer.SetActive(true);
            SetCardsToWaitForInputState(true);
        }
        public void SetCardsToWaitForInputState(bool state)
        {
            var collection = _deckScreen.Collection;
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].CardIsWaitingForInput = state;

                if (state)
                    collection[i].OnCardUIClicked += (SwitchCards);
            }
        }
        public void RemoveSelectedCardUI()
        {
            _selectedCardUI.OnCardClicked.AddListener(Open);
            SetCardsToWaitForInputState(false);

            DisableInteractions();

            OnOpenDeckScreen?.Invoke();

            DefaultSettings();

            _selectedCardUI.MetaCardUIInteraction.ResetInteraction();
            _selectedCardUIContainer.SetActive(false);

            SetMainCardCollectionActiveState(true);
        }
        private void RemoveSelectedCardUI(CardUI card)
    => RemoveSelectedCardUI();
        private void SwitchCards(CardUI card)
        {
            Debug.Log("Switch");
            var account = Account.AccountManager.Instance;
            var selectedCard = account.AccountCharacters.SelectedCharacter;
            var deck = account.AccountCharacters.GetCharacterData(selectedCard).GetDeckAt(0);
            var coreCardInfo = _selectedCardUI.CardUI.GFX.GetCardReference.CardCoreInfo;
            ushort currentCardID = card.GFX.GetCardReference.CardCoreInfo.InstanceID;
            int length = deck.Cards.Length;
            var cards = deck.Cards;
            bool _cardFound = false;


            for (int i = 0; i < length; i++)
            {
                if (cards[i].InstanceID == currentCardID)
                {
                    cards[i] = coreCardInfo;
                    _cardFound = true;
                    break;
                }
            }
            if (!_cardFound)
                throw new System.Exception($"Card Was Not Found In Deck\nID: {currentCardID}");
            _deckScreen.Refresh();
            _allCardsScreen.Refresh();
            ResetToDefault();
        }
        private void SetMainCardCollectionActiveState(bool state) => _allCardsScreen.gameObject.SetActive(state);

        public override void Close()
        {
            _selectedCardUI.gameObject.SetActive(false);
            _deckScreen.gameObject.SetActive(false);
            _allCardsScreen.gameObject.SetActive(true);
            DisableInteractions();

            gameObject.SetActive(false);
        }
        #endregion



        #region States
        #region Default Settings
        private void DefaultSettings()
        {
            var deck = _deckScreen.Collection;
            for (int i = 0; i < deck.Count; i++)
            {
                var metaCardUI = deck[i].MetaCardUIInteraction;
                metaCardUI.ResetEnum();
                metaCardUI.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Info, _cardUIInteractionHandle.Open);
            }
            deck = _allCardsScreen.Collection;

            for (int i = 0; i < deck.Count; i++)
            {
                var metaCardUI = deck[i].MetaCardUIInteraction;
                metaCardUI.ResetEnum();
                metaCardUI.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Info, _cardUIInteractionHandle.Open);
                metaCardUI.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Use, CardSelected);
            }
        }
        #endregion
        #endregion
    }
}
