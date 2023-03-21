using CardMaga.Rewards.Bundles;
using System;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class AddResourceButtonVisualAssigner : BaseVisualAssigner<ResourcesCost>
    {
        [SerializeField] ResourceCollectionVisualSO _resourceCollection;
        [SerializeField] GameObject _addButton;
        public override void CheckValidation()
        {
            if (_addButton == null)
                throw new Exception("AddResourceButtonVisualAssigner has no Button Game Object");
            if (_resourceCollection == null)
                throw new Exception("AddResourceButtonVisualAssigner has no resourceCollection");
            _resourceCollection.CheckValidation();
        }

        public override void Dispose()
        {
            _addButton.SetActive(false);
        }

        public override void Init(ResourcesCost comboData)
        {
            _addButton.SetActive(_resourceCollection.GetResourceSO(comboData.CurrencyType).HasAddResourceButton);
        }
    }
}