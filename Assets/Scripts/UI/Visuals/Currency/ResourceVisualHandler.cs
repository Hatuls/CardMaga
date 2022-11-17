using CardMaga.Rewards.Bundles;
using CardMaga.UI.Text;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class ResourceVisualHandler : BaseResourceVisualHandler
    {
        [SerializeField] ResourceVisualAssignerHandler _resourceVisualAssignerHandler;
        [SerializeField] ResourceTextAssignerHandler _resourceTextAssignerHandler;
        protected override BaseVisualAssignerHandler<ResourcesCost> ResourceVisualAssignerHandler => _resourceVisualAssignerHandler;
        protected override BaseTextAssignerHandler<ResourcesCost> ResourceTextAssignerHandler => _resourceTextAssignerHandler;
        public override void CheckValidation()
        {
            base.CheckValidation();
        }
        public override void Dispose()
        {
            base.Dispose();
        }
        public override void Init(ResourcesCost resourceData)
        {
            base.Init(resourceData);
        }
    }
}