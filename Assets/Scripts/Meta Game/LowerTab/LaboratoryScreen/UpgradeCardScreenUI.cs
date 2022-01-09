﻿using Battles.UI;
using Meta;
using Rewards;

using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.Laboratory
{



    public class UpgradeCardScreenUI : TabAbst
    {

        [SerializeField]
        CardUIInteractionHandle _cardUIInteractionHandle;



        [SerializeField]
        MetaCardUIHandler _selectedCardUI;

        [SerializeField]
        MetaCardUIHandler _upgradedVersion;

        [SerializeField]
        MetaCardUIFilterScreen _collectionFilterHandler;

        [SerializeField]
        CardUpgradeCostSO _upgradeCostSO;

        [SerializeField]
        TextMeshProUGUI _instructionText;
        [SerializeField]
        TextMeshProUGUI _costText;
        [SerializeField]
        GameObject _upgradeBtn;
        [SerializeField]
        string _shownText;

        [SerializeField]
        string defaultText = "Choose Card:";
        [SerializeField]
        string costText = "Upgrade Cost: ";


        [SerializeField]
        ResourceEnum _resourceType = ResourceEnum.Chips;
        [SerializeField]
        bool _toOpenInteractionPanel = true;

        [SerializeField]
        UnityEvent OnSuccessfullUpgrade;
        [SerializeField]
        UnityEvent OnUnSuccessfullUpgrade;

        [SerializeField]
        InfoSettings _upgradedVersionSettings;
        [SerializeField]
        CardUI _currentCardGO;
        [SerializeField]
        InfoSettings _deckSettings;





        public override void Open()
        {
            OnOpenUpgradeScreen();
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            CloseUpgradeScreen();
            gameObject.SetActive(false);
        }

        public void CloseUpgradeScreen()
        {
            var deck = _collectionFilterHandler.Collection;
            for (int i = 0; i < deck.Count; i++)
            {
                deck[i].MetaCardUIInteraction.ResetEnum();
                deck[i].MetaCardUIInteraction.ClosePanel();
            }
            ResetScreen();
        }

        public void OnOpenUpgradeScreen()
        {
            ResetScreen();
            DefaultSet();
        }

        private void DefaultSet()
        {
            var deck = _collectionFilterHandler.Collection;
            for (int i = 0; i < deck.Count; i++)
            {
                var metacardInteraction = deck[i].MetaCardUIInteraction;
                metacardInteraction.ResetInteraction();
                deck[i].ToOnlyClickCardUIBehaviour = !_toOpenInteractionPanel;
                if (_toOpenInteractionPanel)
                {
                    metacardInteraction.SetClickFunctionality(MetaCardUiInteractionEnum.Use, SelectCardUI);
                    metacardInteraction.SetClickFunctionality(MetaCardUiInteractionEnum.Info, (card) => _cardUIInteractionHandle.Open(card, _deckSettings));
                    metacardInteraction.SetClickFunctionality(MetaCardUiInteractionEnum.Dismental, _cardUIInteractionHandle.OpenDismentalScreen);
                }

                else
                {
                    deck[i].OnCardUIClicked += SelectCardUI;
                }
            }
            _selectedCardUI.ResetAll();
            if (_toOpenInteractionPanel)
                _selectedCardUI.MetaCardUIInteraction.SetClickFunctionality(MetaCardUiInteractionEnum.Remove, RemoveCardUI);
            else
            {
                _selectedCardUI.OnCardUIClicked += RemoveCardUI;
                _selectedCardUI.ToOnlyClickCardUIBehaviour = true;
            }

            _upgradedVersion.MetaCardUIInteraction.SetClickFunctionality(MetaCardUiInteractionEnum.Info, (card) => _cardUIInteractionHandle.Open(card, _upgradedVersionSettings));
        }

        public void ResetScreen()
        {
            ActivateGameObject(_upgradeBtn, false);
            ActivateGameObject(_selectedCardUI.gameObject, false);
            ActivateGameObject(_upgradedVersion.gameObject, false);
            if (_currentCardGO != null)
            {
                _currentCardGO.gameObject.SetActive(true);
                _currentCardGO = null;
            }

            _instructionText.text = defaultText;
        }

        public void SelectCardUI(CardUI card)
        {
            if (card == null)
            {
                Debug.LogWarning($"UpgradeUIScreen : Card Is Null!");
                return;
            }
            _currentCardGO = card;
            _selectedCardUI.CardUI.GFX.SetCardReference(card.GFX.GetCardReference);
            ActivateGameObject(_selectedCardUI.gameObject, true);

            var upgradedVersion = UpgradeHandler.GetUpgradedCardVersion(_selectedCardUI.CardUI.GFX.GetCardReference);

            if (upgradedVersion != null)
            {
                SetCostText();
                _upgradedVersion.CardUI.GFX.SetCardReference(upgradedVersion);
                ActivateGameObject(_currentCardGO.gameObject, false);
            }
            else
            {
                _instructionText.text = defaultText;
                ActivateGameObject(_upgradeBtn, false);
            }
            ActivateGameObject(_upgradedVersion.gameObject, upgradedVersion != null);


        }
        public void RemoveCardUI(CardUI card) { ResetScreen(); }
        private void SetCostText()
        {
            _instructionText.text = costText;
            ActivateGameObject(_upgradeBtn, true);
            _costText.text = _upgradeCostSO.NextCardValue(_selectedCardUI.CardUI.GFX.GetCardReference, _resourceType).ToString();
        }
        private void ActivateGameObject(GameObject go, bool state)
        {
            if (go.activeSelf != state)
                go.SetActive(state);
        }

        public void OnUpgradeClick()
        {
            if (!_selectedCardUI.gameObject.activeSelf || !_upgradedVersion.gameObject.activeSelf)
                return;
            var currentCard = _selectedCardUI.CardUI.RecieveCardReference();

            if (UpgradeHandler.TryUpgradeCard(_upgradeCostSO, currentCard, _resourceType))
            {

                if (currentCard.CardsAtMaxLevel == false)
                {
                    //  ResetScreen();
                    SelectCardUI(_currentCardGO);
                    _currentCardGO.gameObject.SetActive(false);
                }
                else
                {
                    _currentCardGO = null;
                    ResetScreen();
                    ActivateGameObject(_upgradeBtn, false);

                }
                _collectionFilterHandler.Refresh();

                Debug.Log(" Succeed");
                OnSuccessfullUpgrade?.Invoke();
            }
            else
            {
                OnUnSuccessfullUpgrade?.Invoke();

                Debug.Log("Didnt upgrade");
            }

        }
        public CardUI SelectCardFromInfoPanel(CardUI card)
               => _collectionFilterHandler.GetCardFromInstanceID(
                   card.RecieveCardReference().CardInstanceID).CardUI;
    }
}
