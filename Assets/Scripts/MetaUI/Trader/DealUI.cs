using CardMaga.Input;
using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using CardMaga.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.MetaUI.Shop
{
    public class DealUI : MonoBehaviour
    {
        public static event Action<BundleRewardFactorySO[]> OnTryBuyDeal;
        [SerializeField]
        private BundleRewardFactorySO[] _rewards;
        [SerializeField]
        public Button _buyMeButton;

        public IEnumerable<ResourcesCost> Costs
        {
            get
            {
                for (int i = 0; i < _rewards.Length; i++)
                {
                    ResourcesCost resourcesCost = _rewards[i].ResourcesCost;
                    if (resourcesCost.Amount > 0)
                        yield return resourcesCost;
                }
            }
        }


        private void Awake()
        {
            CostButtonTextAssigner.InitButtonText(_buyMeButton, Costs);
        }
        public void TryBuy()
        {
            OnTryBuyDeal?.Invoke(_rewards);
        }

        
    }



    public static class CostButtonTextAssigner
    {
        public static void InitButtonText(Button buyMeButton, IEnumerable<ResourcesCost> allCosts)
        {
            IReadOnlyList<ResourcesCost> costs = GetCost(allCosts);
            AssignText(costs, buyMeButton);
        }

        private static void AssignText(IReadOnlyList<ResourcesCost> costs, Button buyMeButton)
        {
            string finalText = string.Empty;

            for (int i = 0; i < costs.Count; i++)
            {
                var current = costs[i];
                string amount = (Mathf.RoundToInt(current.Amount)).ToString();
                finalText += amount.AddImageInFrontOfText((int)current.CurrencyType - 1);
            }

            buyMeButton.SetButtonText(finalText);
        }
        private static IReadOnlyList<ResourcesCost> GetCost( IEnumerable<ResourcesCost> costs)
        {
            List<ResourcesCost> allCosts = new List<ResourcesCost>();

            foreach (var currency in costs)
            {
                CurrencyType currencyType = currency.CurrencyType;
                float amount = currency.Amount;

                if (ContainResourceType(currencyType, allCosts, out ResourcesCost resourcesCost))
                {
                    resourcesCost.AddAmount(amount);
                }
                else
                {
                    allCosts.Add(new ResourcesCost(currencyType, amount));
                }
            }

            return allCosts;

            bool ContainResourceType(CurrencyType type, IReadOnlyList<ResourcesCost> resourcesCosts, out ResourcesCost resourceCost)
            {
                for (int i = 0; i < resourcesCosts.Count; i++)
                {
                    if (type == resourcesCosts[i].CurrencyType)
                    {
                        resourceCost = resourcesCosts[i];
                        return true;
                    }
                }
                resourceCost = null;
                return false;
            }
         
        }
    }
}