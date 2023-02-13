using CardMaga.Rewards.Bundles;
using CardMaga.UI.Text;
using Sirenix.OdinInspector;
using System;
using UnityEngine;


namespace CardMaga.UI.Visuals
{
    public class ResourceVisualHandler : BaseResourceVisualHandler
    {
        [SerializeField] ResourceVisualAssignerHandler _resourceVisualAssignerHandler;
        [SerializeField] ResourceTextAssignerHandler _resourceTextAssignerHandler;
        protected override BaseVisualAssignerHandler<ResourcesCost> ResourceVisualAssignerHandler => _resourceVisualAssignerHandler;
        protected override BaseTextAssignerHandler<ResourcesCost> ResourceTextAssignerHandler => _resourceTextAssignerHandler;


#if UNITY_EDITOR

        [Header("Test")]
        public ResourcesCost MyResourceCost;

        [Button]
        private void OnTest()
        {
            CheckValidation();
            Init(MyResourceCost);
        }

#endif
    }
}