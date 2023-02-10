using CardMaga.Rewards;
using CardMaga.Rewards.Bundles;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
namespace CardMaga.MetaUI.Shop
{
    public class ShopManager
    {
        public event Action OnCompletePurchase;
        public event Action OnFailedToPurhcase;
        public bool TryBuyDeal(IDealable dealable)
        {
            var userResources = Account.AccountManager.Instance.Data.AccountResources;
            bool success = userResources.TryReduceAmount(dealable.Costs);

            if (success)
                ReceiveDeals(dealable.Products);
            else
                OnFailedToPurhcase?.Invoke();

            return success;
        }

        public bool CanBuy(IReadOnlyList<ResourcesCost> costs)
            => Account.AccountManager.Instance.Data.AccountResources.HasEnoughAmount(costs);
        public bool CanBuy(IDealable dealable)
            => CanBuy(dealable.Costs);


        private void ReceiveDeals(IReadOnlyList<BundleRewardFactorySO> products)
        {
            for (int i = 0; i < products.Count; i++)
            {
                products[i].GenerateReward().AddToDevicesData();
            }

            ITokenReceiver tokenMachine = new TokenMachine(OnCompletePurchase); 
            Account.AccountManager.Instance.SendAccountData(tokenMachine);
        }
    }
}