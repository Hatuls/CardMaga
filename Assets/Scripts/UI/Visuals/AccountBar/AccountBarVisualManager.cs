using Account;
using CardMaga.Rewards.Bundles;
using CardMaga.UI.Visuals;
using UnityEngine;
namespace CardMaga.UI.Account
{

    public class AccountBarVisualManager : MonoBehaviour
    {
        [SerializeField]
        private AccountBarVisualHandler _accountBarVisualHandler;

        private void Awake()
        {
            _accountBarVisualHandler.Init(GenerateAccoundDataFirstTime());
        }

        private AccountBarVisualData GenerateAccoundDataFirstTime()
        {
            var accountManager = AccountManager.Instance;
            if (accountManager != null && accountManager.Data != null)
            {
                AccountData data = accountManager.Data;
                return new AccountBarVisualData
                    (
                    data.DisplayName,
                    data.AccountGeneralData.ImageID,// Take from account
                    data.AccountLevel.Exp,
                    0,
                    data.AccountLevel.Level,
                    new ResourcesCost(Rewards.CurrencyType.Chips, data.AccountResources.Chips),
                    new ResourcesCost(Rewards.CurrencyType.Gold, data.AccountResources.Gold),
                    new ResourcesCost(Rewards.CurrencyType.Diamonds, data.AccountResources.Diamonds)
                    );

            }
            else
                return new AccountBarVisualData
                  (
                 "New User",
                  0,
                  0,
                  0,
                  1,
                  new ResourcesCost(Rewards.CurrencyType.Chips, 50),
                  new ResourcesCost(Rewards.CurrencyType.Gold, 500),
                  new ResourcesCost(Rewards.CurrencyType.Diamonds, 100)
                  );


        }
    }

}