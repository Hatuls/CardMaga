using Battles.UI;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
namespace UI.Meta.Laboratory
{
    public class DeckCollectionScreenUI : TabAbst
    {
        #region Fields
        [SerializeField]
        DismentalScreen _dismentalScreen;

        [SerializeField]
        UpgradeCardScreenUI _upgradeScreen;

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
            SetCardsToWaitForInputState(false);
        }

        private void CardSelected(CardUI card)
        {
            SetMainCardCollectionActiveState(false);
            DisableInteractions();
            _selectedCardUI.CardUI.GFX.SetCardReference(card.GFX.GetCardReference);
            SetCardsToWaitForInputState(true);
            _selectedCardUI.MetaCardUIInteraction.SetClickFunctionality(MetaCardUiInteractionEnum.Remove, RemoveSelectedCardUI);
            _selectedCardUIContainer.SetActive(true);
       
        }
        public void SetCardsToWaitForInputState(bool state)
        {
            var collection = _deckScreen.Collection;
            for (int i = 0; i < collection.Count; i++)
            {
                collection[i].ToOnlyClickCardUIBehaviour = state;

                if (state)
                    collection[i].OnCardUIClicked += (SwitchCards);
                else
                    collection[i].OnCardUIClicked -= (SwitchCards);
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
            var coreCardInfo = _selectedCardUI.CardUI.GFX.GetCardReference.CardCoreInfo;

            if (coreCardInfo == null)
                return; 

            Debug.Log("Switch");
            var account = Account.AccountManager.Instance;
            var selectedCard = account.AccountCharacters.SelectedCharacter;
            var deck = account.AccountCharacters.GetCharacterData(selectedCard).GetDeckAt(0);
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

        [SerializeField]
        InfoSettings _deckSettings;
        [SerializeField]
        InfoSettings _cardCollectionSettingsInDeckScreen;
        #region States
        #region Default Settings
        private void DefaultSettings()
        {
            
            var deck = _deckScreen.Collection;
            for (int i = 0; i < deck.Count; i++)
            {
                var metaCardUI = deck[i].MetaCardUIInteraction;
                metaCardUI.ResetEnum();
                metaCardUI.ClosePanel();
                metaCardUI.SetClickFunctionality(MetaCardUiInteractionEnum.Info,(card) => _cardUIInteractionHandle.Open(card, _deckSettings));
            }
            var remainDeck = _allCardsScreen.OnlyActiveCollection;
            
            int remain = remainDeck.Count();
            for (int i = 0; i < remain; i++)
            {
                var metaCardUI = remainDeck.ElementAt(i).MetaCardUIInteraction;
                metaCardUI.ResetEnum();
                metaCardUI.ClosePanel();
                metaCardUI.SetClickFunctionality(MetaCardUiInteractionEnum.Info,(card) =>  _cardUIInteractionHandle.Open(card, _cardCollectionSettingsInDeckScreen));
                metaCardUI.SetClickFunctionality(MetaCardUiInteractionEnum.Use, CardSelected); 
                metaCardUI.SetClickFunctionality(MetaCardUiInteractionEnum.Dismental, card => _dismentalScreen.Open(card));
            }
        }
        #endregion
        #endregion
    }

    [Serializable]
    public class InfoSettings : IInfoSettings<CardUI>
    {
        public Action<CardUI> OnSelectUse;
        [SerializeField]
        private bool canUse;
        [SerializeField]
        private bool canUpgrade;
        [SerializeField]
        private bool canDismental;

        public bool CanUse { get => canUse; set => canUse = value; }
        public bool CanUpgrade { get => canUpgrade; set => canUpgrade = value; }
        public bool CanDismental { get => canDismental; set => canDismental = value; }


        public void OnUse(CardUI card)
        {
            OnSelectUse?.Invoke(card);
        }
    }

}

