using Battles.UI;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Cards;
using UnityEngine.Events;

namespace UI.Meta.Laboratory
{
    public class DeckCollectionScreenUI: TabAbst
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
        CardUI _selectedCardUI;
        #endregion
        #region Public Methods



        public void CloseAll()
        {
            foreach (var card in _allCardsScreen.Collection)
                card.ResetMetaCardInteraction();

            foreach (var card in _deckScreen.Collection)
                card.ResetMetaCardInteraction();
        }

        public void OpenDeckCollection()
        {
            CloseAll();
        }


    
        #endregion

        #region Interface

        public override void Open()
        {
            CloseAll();
            OnOpenDeckScreen?.Invoke();
            DefaultSettings();
            gameObject.SetActive(true);

        }
        public void DefaultSettings()
        {
            var deck =_deckScreen.Collection;
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

        private void CardSelected(CardUI card)
        {
            _allCardsScreen.gameObject.SetActive(false);
            _selectedCardUI.GFX.SetCardReference(card.GFX.GetCardReference);
      
            _selectedCardUI.gameObject.SetActive(true);

        }



        public override void Close()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
