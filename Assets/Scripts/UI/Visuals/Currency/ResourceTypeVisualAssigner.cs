using CardMaga.Rewards.Bundles;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ResourceTypeVisualAssigner : BaseVisualAssigner<ResourcesCost>
    {
        [SerializeField] ResourceCollectionVisualSO _resourceCollection;
        [SerializeField] Image _resourceIcon;

        public override void CheckValidation()
        {

            if (_resourceIcon == null)
                throw new Exception("ResourceTypeVisualAssigner has no resourceIcon");
            if (_resourceCollection == null)
                throw new Exception("ResourceTypeVisualAssigner has no resourceCollection");
            _resourceCollection.CheckValidation();
        }

        public override void Dispose()
        {

        }

        public override void Init(ResourcesCost resourceData)
        {
            _resourceIcon.sprite = _resourceCollection.GetResourceSO(resourceData.CurrencyType).ResourceIcon;
        }
    }
}