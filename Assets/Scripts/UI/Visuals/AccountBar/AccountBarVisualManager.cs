using Account;
using CardMaga.Rewards.Bundles;
using CardMaga.UI.Visuals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CardMaga.UI.Account
{

    public class AccountBarVisualManager : BaseUIElement
    {
        public static AccountBarVisualManager Instance;
        [SerializeField]
        private AccountBarVisualHandler _accountBarVisualHandler;


        [SerializeField]
        private BaseUIElement[] _elementsToShowAccountBar; 
        [SerializeField]
        private BaseUIElement[] _elementsToHideAccountBar;

        public void Start()
        {
            Refresh();
            Instance = this;
            for (int i = 0; i < _elementsToHideAccountBar.Length; i++)
            {
                _elementsToHideAccountBar[i].OnShow += Hide;
            }

            for (int i = 0; i < _elementsToShowAccountBar.Length; i++)
            {
                _elementsToShowAccountBar[i].OnShow += Show;
            }
            Show();
        }
     
        private void OnDestroy()
        {
            for (int i = 0; i < _elementsToHideAccountBar.Length; i++)
            {
                _elementsToHideAccountBar[i].OnShow -= Hide;
            }

            for (int i = 0; i < _elementsToShowAccountBar.Length; i++)
            {
                _elementsToShowAccountBar[i].OnShow -= Show;
            }
        }
        public void Refresh()
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
                    accountManager.DisplayName,
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