
using TMPro;
using UnityEngine;

namespace UI.Meta
{
    public class UpperTabHandler : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _diamondsAmountText;
        [SerializeField]
        TextMeshProUGUI _energysAmountText;
        [SerializeField]
        TextMeshProUGUI _levelAmountText;
        [SerializeField]
        TextMeshProUGUI _chipAmountText;


        [SerializeField]
        TextMeshProUGUI _energyTextAnimation;
        [SerializeField]
        Animator _upperTabAnimator;
        // Need To be Re-Done
        void Start()
        {
            //var account = Account.AccountManager.Instance.AccountGeneralData;
            //var resources = account.AccountResourcesData;
            //RemoveSubScribers(account, resources);

            //SubscribeEvents(account, resources);
            //UpdateStats();
        }
        // Need To be Re-Done
        public void UpdateStats()
        {
            //var account = Account.AccountManager.Instance.AccountGeneralData;
            //_chipAmountText.text = $"{account.AccountResourcesData.Chips.Value}";

            //_diamondsAmountText.text = $"{ account.AccountResourcesData.Diamonds.Value}";
            //_levelAmountText.text = $"{ account.AccountLevelData.Level.Value}";
            //_energysAmountText.text = $"{ account.AccountEnergyData.Energy.Value} / {account.AccountEnergyData.MaxEnergy.Value}";
        }
        // Need To be Re-Done
        //private void SetEnergyText(int val) => _energysAmountText.text = $"{ val} / {Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.MaxEnergy.Value}";
        //// Need To be Re-Done
        //private void SetMaxEnergyText(int val) => _energysAmountText.text = $"{Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.Value } / {val}";
        private void SetDiamondText(int val) => _diamondsAmountText.text = val.ToString();
        private void SetLevelText(int val) => _levelAmountText.text = val.ToString();

        private void SetChipText(int val) => _chipAmountText.text = val.ToString();
        // Need To be Re-Done

        //private void SubscribeEvents(Account.GeneralData.AccountGeneralData account, Account.GeneralData.AccountResourcesData resources)
        //{
        //    resources.Chips.OnValueChange += (SetChipText);
        //    resources.Diamonds.OnValueChange += (SetDiamondText);
        //    account.AccountLevelData.Level.OnValueChange += (SetLevelText);
        //    account.AccountEnergyData.Energy.OnValueChange += (SetEnergyText);
        //    account.AccountEnergyData.MaxEnergy.OnValueChange += (SetMaxEnergyText);
        //}
        // Need To be Re-Done
        //private void RemoveSubScribers(Account.GeneralData.AccountGeneralData account, Account.GeneralData.AccountResourcesData resources)
        //{
        //    resources.Chips.OnValueChange -= (SetChipText);
        //    resources.Diamonds.OnValueChange -= (SetDiamondText);
        //    account.AccountLevelData.Level.OnValueChange -= (SetLevelText);
        //    account.AccountEnergyData.Energy.OnValueChange -= (SetEnergyText);
        //    account.AccountEnergyData.MaxEnergy.OnValueChange -= (SetMaxEnergyText);
        //}




        public void PlayReduceEnergyAnimation(ushort amount)
        {
            _energyTextAnimation.text = string.Concat("- ", amount);
            _upperTabAnimator.Play("ReduceEnergy");
        }
        //private void OnDisable()
        //{
        //    if (Account.AccountManager.Instance != null && Account.AccountManager.Instance.AccountGeneralData != null)
        //    {

        //        var account = Account.AccountManager.Instance.AccountGeneralData;
        //        var resources = account.AccountResourcesData;
        //        RemoveSubScribers(account, resources);
        //    }
        //}
    }
}