using CardMaga.Card;
using CardMaga.Rewards.Bundles;
using CardMaga.UI.Text;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public abstract class BaseResourceVisualHandler : MonoBehaviour, IInitializable<ResourcesCost>
    {
        protected abstract BaseVisualAssignerHandler<ResourcesCost> ResourceVisualAssignerHandler { get; }
        protected abstract BaseTextAssignerHandler<ResourcesCost> ResourceTextAssignerHandler { get;}
        public virtual void CheckValidation()
        {
            ResourceTextAssignerHandler.CheckValidation();
            ResourceVisualAssignerHandler.CheckValidation();
        }
        public virtual void Dispose()
        {
            ResourceTextAssignerHandler.Dispose();
            ResourceVisualAssignerHandler.Dispose();
        }
        public virtual void Init(ResourcesCost resources)
        {
            ResourceTextAssignerHandler.Init(resources);
            ResourceVisualAssignerHandler.Init(resources);
        }
    }
}