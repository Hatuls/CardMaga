using CardMaga.Rewards.Bundles;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ResourceVisualAssignerHandler : BaseVisualAssignerHandler<ResourcesCost>
    {
        [SerializeField] ResourceTypeVisualAssigner _resourceTypeVisualAssigner;
        [SerializeField] AddResourceButtonVisualAssigner _addResourceButtonVisualAssigner;
        public override IEnumerable<BaseVisualAssigner<ResourcesCost>> VisualAssigners
        {
            get
            {
                yield return _resourceTypeVisualAssigner;
                yield return _addResourceButtonVisualAssigner;
            }
        }
    }
}