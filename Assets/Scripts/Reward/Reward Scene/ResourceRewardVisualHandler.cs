using CardMaga.Battle.Players;
using CardMaga.CinematicSystem;
using CardMaga.UI;
using CardMaga.UI.Visuals;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
namespace CardMaga.Rewards
{
    public class ResourceRewardVisualHandler : MonoBehaviour, IComparable<ResourceRewardVisualHandler>
    {

        [SerializeField]
        private ObjectActivationManager _objectRenderer;
        [SerializeField]
        private CurrencyAmount[] _currencyTypeInfo;
        [SerializeField]
        private CurrencyType _currencyType;
        [SerializeField]
        private int _priority;

        [ShowInInspector, ReadOnly]
        private int _value;


        public int Amount { get => _value; }
        public CurrencyType CurrencyType { get => _currencyType; }
        public bool HasValue => _value > 0;

        public int Priority => _priority;


        public void AddValue(int amount)
        {
            _value += amount;
        }

        public int CompareTo(ResourceRewardVisualHandler other)
        {
            if (Priority < other.Priority)
                return -1;
            else if (Priority == other.Priority)
                return 0;
            else
                return 1;
        }

        internal void Show()
        {
            if (HasValue == false)
                return;
            // _token = tokenMachine.GetToken();
            var currencyAmount = GetCurrencyAmount();

            _objectRenderer.Activate(currencyAmount.RewardTagSO);
            currencyAmount.AmountText.text = Amount.ToString().AddImageAfterOfText((int)_currencyType - 1);
            currencyAmount.CinematicManager.ResetAll();
            currencyAmount.CinematicManager.StartCinematicSequence();
        }


        private CurrencyAmount GetCurrencyAmount()
        {
            for (int i = _currencyTypeInfo.Length - 1; i >= 0; i--)
            {
                if (_currencyTypeInfo[i].IsAbove(Amount))
                    return _currencyTypeInfo[i];
            }
            throw new Exception($"Amount is not above any of the currency types info amount\nAmount: {Amount}");
        }
        [Serializable]
        protected class CurrencyAmount
        {
            public RewardTagSO RewardTagSO;
            public TextMeshPro AmountText;
            public CinematicManager CinematicManager;

            [SerializeField, Tooltip("Will activate the amount when the current amount is above this number")]
            private int _aboveAmount;

            public bool IsAbove(int currentAmount)
                => currentAmount >= _aboveAmount;
        }
    }

}