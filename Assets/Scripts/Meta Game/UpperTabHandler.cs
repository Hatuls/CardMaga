
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

        void Start()
        {
            var account = Account.AccountManager.Instance.AccountGeneralData;
            var resources = account.AccountResourcesData;
            RemoveSubScribers(account, resources);

            SubscribeEvents(account, resources);
            UpdateStats();
        }
        public void UpdateStats()
        {
            var account = Account.AccountManager.Instance.AccountGeneralData;
            _chipAmountText.text = $"{account.AccountResourcesData.Chips.Value}";

            _diamondsAmountText.text = $"{ account.AccountResourcesData.Diamonds.Value}";
            _levelAmountText.text = $"{ account.AccountLevelData.Level.Value}";
            _energysAmountText.text = $"{ account.AccountEnergyData.Energy.Value} / {account.AccountEnergyData.MaxEnergy.Value}";
        }
        private void SetEnergyText(ushort val) => _energysAmountText.text = $"{ val} / {Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.MaxEnergy.Value}";
        private void SetMaxEnergyText(ushort val) => _energysAmountText.text = $"{Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.Value } / {val}";
        private void SetDiamondText(ushort val) => _diamondsAmountText.text = val.ToString();
        private void SetLevelText(ushort val) => _levelAmountText.text = val.ToString();

        private void SetChipText(ushort val) => _chipAmountText.text = val.ToString();

        private void SubscribeEvents(Account.GeneralData.AccountGeneralData account, Account.GeneralData.AccountResourcesData resources)
        {
            resources.Chips.OnValueChange += (SetChipText);
            resources.Diamonds.OnValueChange += (SetDiamondText);
            account.AccountLevelData.Level.OnValueChange += (SetLevelText);
            account.AccountEnergyData.Energy.OnValueChange += (SetEnergyText);
            account.AccountEnergyData.MaxEnergy.OnValueChange += (SetMaxEnergyText);
        }

        private void RemoveSubScribers(Account.GeneralData.AccountGeneralData account, Account.GeneralData.AccountResourcesData resources)
        {
            resources.Chips.OnValueChange -= (SetChipText);
            resources.Diamonds.OnValueChange -= (SetDiamondText);
            account.AccountLevelData.Level.OnValueChange -= (SetLevelText);
            account.AccountEnergyData.Energy.OnValueChange -= (SetEnergyText);
            account.AccountEnergyData.MaxEnergy.OnValueChange -= (SetMaxEnergyText);
        }




        public void PlayReduceEnergyAnimation(ushort amount)
        {
            _energyTextAnimation.text = string.Concat("- ", amount);
            _upperTabAnimator.Play("ReduceEnergy");
        }
        private void OnDisable()
        {
            if (Account.AccountManager.Instance != null && Account.AccountManager.Instance.AccountGeneralData != null)
            {

                var account = Account.AccountManager.Instance.AccountGeneralData;
                var resources = account.AccountResourcesData;
                RemoveSubScribers(account, resources);
            }
        }
    }
}