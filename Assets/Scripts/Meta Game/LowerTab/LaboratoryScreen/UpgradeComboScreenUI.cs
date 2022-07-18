using Battles.UI;
using Meta;
using Rei.Utilities;
using Rewards;

using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Meta.Laboratory
{
    public class UpgradeComboScreenUI : TabAbst
    {
        [SerializeField]
        ComboRecipeUI _selectedComboUI;
        [SerializeField]
        ComboRecipeUI _upgradedComboUI;

        [SerializeField]
        MetaComboUIFilterScreen _collectionFilterHandler;

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
        bool _toShowDismentalOption = true;

        [SerializeField]
        UnityEvent OnSuccessfullUpgrade;
        [SerializeField]
        UnityEvent OnUnSuccessfullUpgrade;

        [SerializeField] SortByNotSelectedCombo _comboSort;

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
                deck[i].ResetClick();
                deck[i].RegisterClick(SelectCombo);
            }
            _selectedComboUI.ResetClick();
            _selectedComboUI.RegisterClick(RemoveCombo);
        }

        public void ResetScreen()
        {
            ActivateGameObject(_upgradeBtn, false);
            ActivateGameObject(_selectedComboUI.gameObject, false);
            ActivateGameObject(_upgradedComboUI.gameObject, false);
            _instructionText.text = defaultText;
        }

        public void SelectCombo(ComboRecipeUI combo)
        {
            if (combo == null)
                throw new System.Exception($"UpgradeUIScreen : Card Is Null!");

            _selectedComboUI.InitRecipe(combo.Combo);
            ActivateGameObject(_selectedComboUI.gameObject, true);

            var upgradedVersion = UpgradeHandler.GetUpgradedComboVersion(_selectedComboUI.Combo);
            _comboSort.ID = null;
            _comboSort.SortRequest();

            if (upgradedVersion != null)
            {
                SetCostText();
                _upgradedComboUI.InitRecipe(upgradedVersion);
                _comboSort.ID =_selectedComboUI.Combo.ComboSO.ID ;
                _comboSort.SortRequest(); 
            }
            else
            {
                _instructionText.text = defaultText;
                ActivateGameObject(_upgradeBtn, false);
            }


            ActivateGameObject(_upgradedComboUI.gameObject, upgradedVersion != null);
        }
        public void RemoveCombo(ComboRecipeUI card) { ResetScreen(); }
        private void SetCostText()
        {
            _instructionText.text = costText;
            ActivateGameObject(_upgradeBtn, true);
            _costText.text = _upgradeCostSO.NextCardValue(_selectedComboUI.CardUI.CardData, _resourceType).ToString();
        }
        private void ActivateGameObject(GameObject go, bool state)
        {
            if (go.activeSelf != state)
                go.SetActive(state);
        }

        public void OnUpgradeClick()
        {
            if (!_selectedComboUI.gameObject.activeSelf || !_upgradedComboUI.gameObject.activeSelf)
                return;
            if (UpgradeHandler.TryUpgradeCombo(_upgradeCostSO, _selectedComboUI.Combo, _resourceType))
            {
                if (_selectedComboUI.Combo.Level == _selectedComboUI.ComboRecipe.CraftedCard.CardsMaxLevel-1)
                {
                    ResetScreen();
                    ActivateGameObject(_upgradeBtn, false);
                }
                else
                {

                SelectCombo(_selectedComboUI);
                Debug.Log(" Succeed");
                OnSuccessfullUpgrade?.Invoke();
                }
            }
            else
            {
                OnUnSuccessfullUpgrade?.Invoke();
                _comboSort.ID = null;
            _comboSort.SortRequest();
                Debug.Log("Didnt upgrade");
            }
        }

    }
}
