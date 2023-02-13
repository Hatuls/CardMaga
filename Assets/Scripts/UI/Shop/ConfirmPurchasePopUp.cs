using CardMaga.Input;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using CardMaga.UI;
using CardMaga.UI.Account;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
namespace CardMaga.MetaUI.Shop
{

    public class ConfirmPurchasePopUp : BaseUIElement
    {

        [SerializeField]
        private TextMeshProUGUI _purchaseText;
        [SerializeField]
        private Button _purchaseButton;

        [SerializeField]
        private Color _canBuyColor = Color.white;
        [SerializeField]
        private Color _cantBuyColor = Color.red;

        private IDealable _currentDeal;
        private ShopManager _shopManager;

        private void Awake()
        {
            _shopManager = new ShopManager();
            _shopManager.OnCompletePurchase += ExitScreen;
        }
        private void OnDestroy()
        {
            _shopManager.OnCompletePurchase -= ExitScreen;

        }
        public void Init(IDealable dealable)
        {
            // Set Text;
            SetPurchaseDescription(dealable.Products);
            // Button Text;
            SetButtonText(dealable.Costs);
            // Button text's color
            SetButtonTextColor(dealable.Costs);

            _currentDeal = dealable;
        }

        private void SetButtonTextColor(IReadOnlyList<ResourcesCost> costs)
        {
            Color clr = _shopManager.CanBuy(costs) ? _canBuyColor : _cantBuyColor;
            _purchaseButton.SetButtonText(_purchaseButton.ButtonText.ColorString(clr));
        }

        public void TryPurchase()
        {
            if (_shopManager.TryBuyDeal(_currentDeal))
            {
                RewardManager.Instance.OpenRewardsScene(_currentDeal.Products.ToArray());
                Hide();
                AccountBarVisualManager.Instance.Refresh();
                return;
            }
        }
        private void SetButtonText(IReadOnlyList<ResourcesCost> costs)
        {
            StringBuilder stringBuilder = new StringBuilder(GetCostWithIconForText(costs[0]));

            for (int i = 1; i < costs.Count; i++)
                stringBuilder.Append(string.Concat("\n", GetCostWithIconForText(costs[i])));

            _purchaseButton.SetButtonText(stringBuilder.ToString());

            stringBuilder.Clear();

            string GetCostWithIconForText(ResourcesCost cost)
            {
                string amount = Mathf.RoundToInt(cost.Amount).ToString();
                return amount.AddImageInFrontOfText((int)cost.CurrencyType - 1);
            }
        }

        private void SetPurchaseDescription(IReadOnlyList<BundleRewardFactorySO> bundleRewardFactorySOs)
        {
            const string CONTEXT = "Would you like to purchase ";
            string products = bundleRewardFactorySOs[0].Name;
            for (int i = 1; i < bundleRewardFactorySOs.Count; i++)
            {
                var current = bundleRewardFactorySOs[i];
                products += string.Concat(" And " + current.Name);
            }

            _purchaseText.text = string.Concat(CONTEXT, products, "?");
        }
        public void ExitScreen()
        {
            Hide();
        }
    }

}