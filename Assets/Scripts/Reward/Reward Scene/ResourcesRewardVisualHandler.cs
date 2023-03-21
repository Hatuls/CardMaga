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
        [SerializeField]
        private ClickHelper _moveNextClickHelper;
        private SequenceHandler _sequenceHandler = new SequenceHandler();
        private List<ResourceRewardVisualHandler> _resourcesList = new List<ResourceRewardVisualHandler>();
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
            _resourcesList.Clear();
            OpenRewardScreens();
        }

        private void OpenRewardScreens()
        {
            for (int i = 0; i < _resourceVisualHandlers.Length; i++)
            {
                ResourceRewardVisualHandler resourceRewardVisualHandler = _resourceVisualHandlers[i];
                Debug.Log(resourceRewardVisualHandler.CurrencyType + " - " + resourceRewardVisualHandler.Amount);
                if (resourceRewardVisualHandler.HasValue)
                    _resourcesList.Add(resourceRewardVisualHandler);
            }
            MoveNext();
         //   _sequenceHandler.StartAll(Hide);
        }

        public void MoveNext()
        {
            if(_resourcesList.Count <= 0)
            {
                Hide();
                return;
            }

            var nextElement = _resourcesList[0];
            _resourcesList.RemoveAt(0);
            nextElement.Show();
        }
        public void WaitForInput() => _moveNextClickHelper.Open(MoveNext);
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