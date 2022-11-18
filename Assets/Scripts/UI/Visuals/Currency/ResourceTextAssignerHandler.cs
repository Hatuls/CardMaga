using CardMaga.Rewards.Bundles;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class ResourceTextAssignerHandler : BaseTextAssignerHandler<ResourcesCost>
    {
        [SerializeField] ResourceAmountTextAssigner _resourceAmountTextAssigner;
        public override IEnumerable<BaseTextAssigner<ResourcesCost>> TextAssigners
        {
            get {
                yield return _resourceAmountTextAssigner;
            }
        }
    }
}