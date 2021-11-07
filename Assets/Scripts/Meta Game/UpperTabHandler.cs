using System.Collections;
using System.Collections.Generic;
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

        void Start()
        {
            UpdateStats();
        }
        public void UpdateStats()
        {
            _energysAmountText.text = $"{ Account.AccountManager.Instance.AccountGeneralData.AccountEnergyData.Energy.Value}";
            _diamondsAmountText.text = $"{ Account.AccountManager.Instance.AccountGeneralData.AccountResourcesData.Diamonds.Value}";
            _levelAmountText.text = $"{ Account.AccountManager.Instance.AccountGeneralData.AccountLevelData.Level.Value}";
        }
    }
}