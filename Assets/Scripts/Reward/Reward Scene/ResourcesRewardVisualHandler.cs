using CardMaga.Rewards.Bundles;
using CardMaga.SequenceOperation;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CardMaga.Rewards
{
    public class ResourcesRewardVisualHandler : BaseRewardsVisualHandler
    {
        [SerializeField,ReadOnly]
        private ResourceRewardVisualHandler[] _resourceVisualHandlers;

        private SequenceHandler _sequenceHandler = new SequenceHandler();
        public IEnumerable<CurrencyReward> CurrencyRewards
        {
            get
            {
                for (int i = 0; i < _rewards.Count; i++)
                    yield return _rewards[i] as CurrencyReward;
            }
        }

        public override void Show()
        {
            base.Show();
            OpenRewardScreens();
        }

        private void OpenRewardScreens()
        {
            for (int i = 0; i < _resourceVisualHandlers.Length; i++)
            {
                Debug.Log(_resourceVisualHandlers[i].CurrencyType + " - " + _resourceVisualHandlers[i].Amount);
                if (_resourceVisualHandlers[i].HasValue)
                    _sequenceHandler.Register(_resourceVisualHandlers[i]);
            }

            _sequenceHandler.StartAll(Hide);
        }

        protected override void AddRewards()
        {
            foreach (var item in _rewards)
                item.AddToDevicesData();
        }

        protected override void CalculateRewards()
        {
 
            foreach (CurrencyReward item in CurrencyRewards)
            {
                ResourcesCost cost = item.ResourcesCost;
                ResourceRewardVisualHandler currency = _resourceVisualHandlers.First(x => x.CurrencyType == cost.CurrencyType);
                currency.AddValue(System.Convert.ToInt32(cost.Amount));
            }
        }

#if UNITY_EDITOR
        [Button]
        private void FindAndSort()
        {
            _resourceVisualHandlers = transform.GetComponentsInChildren<ResourceRewardVisualHandler>();
            _resourceVisualHandlers?.Sort((x, y) => x.CompareTo(y));
        }
#endif
    }


}