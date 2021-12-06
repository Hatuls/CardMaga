using Battles.UI;
using Meta;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Meta.Laboratory
{
    public class UpgradeScreenUI : TabAbst
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
        string costText = "Upgrade Cost: ";







        public override void Open()
        {
            OnOpenUpgradeScreen();
            gameObject.SetActive(true);
        }
        public override void Close()
        {
            var deck = _collectionFilterHandler.Collection;
            for (int i = 0; i < deck.Count; i++)
            {
                deck[i].MetaCardUIInteraction.ResetInteraction();
                deck[i].MetaCardUIInteraction.ResetEnum();
            }
            ResetScreen();
            gameObject.SetActive(false);
        }

        private void OnOpenUpgradeScreen()
        {
            ResetScreen();
            DefaultSet();
        }

        private void DefaultSet()
        {
            var deck = _collectionFilterHandler.Collection;
            for (int i = 0; i <deck.Count; i++)
            {
                var metacardInteraction = deck[i].MetaCardUIInteraction;
                metacardInteraction.ResetInteraction();
                metacardInteraction.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Use, SelectCardUI);
                metacardInteraction.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Dismental, _cardUIInteractionHandle.OpenDismentalScreen);
                metacardInteraction.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Info, _cardUIInteractionHandle.Open);
            }
            _selectedCardUI.MetaCardUIInteraction.SetClickFunctionality(MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Remove, RemoveCardUI);
        }

        public void ResetScreen()
        {
            ActivateGameObject(_upgradeBtn, false);
            ActivateGameObject(_selectedCardUI.gameObject, false);
            ActivateGameObject(_upgradedVersion.gameObject, false);
            _instructionText.text = defaultText;
        }

        public void SelectCardUI(CardUI card)
        {
            if (card == null)
                throw new System.Exception($"UpgradeUIScreen : Card Is Null!");

            _selectedCardUI.CardUI.GFX.SetCardReference(card.GFX.GetCardReference);
            ActivateGameObject(_selectedCardUI.gameObject, true);

            var upgradedVersion = UpgradeHandler.GetUpgradedCardVersion(_selectedCardUI.CardUI.GFX.GetCardReference);

            if (upgradedVersion != null)
            {
                SetCostText();
                _upgradedVersion.CardUI.GFX.SetCardReference(upgradedVersion);
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
            _costText.text = _upgradeCostSO.NextCardValue(_selectedCardUI.CardUI.GFX.GetCardReference).ToString();
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
            if (UpgradeHandler.TryUpgradeCard(_upgradeCostSO, _selectedCardUI.CardUI.GFX.GetCardReference))
            {
                ResetScreen();
                _collectionFilterHandler.Refresh();
                Debug.Log(" Succeed");
                ActivateGameObject(_upgradeBtn, false);
            }
            else
            {


                Debug.Log("Didnt upgrade");
            }

        }

    }
}
