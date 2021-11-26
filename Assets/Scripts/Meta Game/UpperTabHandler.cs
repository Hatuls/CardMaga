
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

        void Start()
        {
            UpdateStats();
        }
        public void UpdateStats()
        {
            var account = Account.AccountManager.Instance.AccountGeneralData;
            _chipAmountText.text = $"{account.AccountResourcesData.Chips.Value}";
            _energysAmountText.text = $"{ account.AccountEnergyData.Energy.Value}";
            _diamondsAmountText.text = $"{ account.AccountResourcesData.Diamonds.Value}";
            _levelAmountText.text = $"{ account.AccountLevelData.Level.Value}";
        }
        private void SetEnergyText(ushort val) => _chipAmountText.text = val.ToString();
        private void SetMaxEnergyText(ushort val) => _chipAmountText.text = val.ToString();
        private void SetDiamondText(ushort val) => _chipAmountText.text = val.ToString();
        private void SetLevelText(ushort val) => _chipAmountText.text = val.ToString();
       
        private void SetChipText(ushort val) => _chipAmountText.text = val.ToString();
        private void Awake()
        {
            var account = Account.AccountManager.Instance.AccountGeneralData;
            var resources = account.AccountResourcesData;
            RemoveSubScribers(account, resources);

            SubscribeEvents(account, resources);
        }

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
            resources.Chips.OnValueChange-=(SetChipText);
            resources.Diamonds.OnValueChange -= (SetDiamondText);
            account.AccountLevelData.Level.OnValueChange -= (SetLevelText);
            account.AccountEnergyData.Energy.OnValueChange -= (SetEnergyText);
            account.AccountEnergyData.MaxEnergy.OnValueChange -= (SetMaxEnergyText);
        }

        private void OnDestroy()
        {
            var account = Account.AccountManager.Instance.AccountGeneralData;
            var resources = account.AccountResourcesData;
            RemoveSubScribers(account, resources);
        }
    }
}