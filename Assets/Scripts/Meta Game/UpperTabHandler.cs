
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
    }
}