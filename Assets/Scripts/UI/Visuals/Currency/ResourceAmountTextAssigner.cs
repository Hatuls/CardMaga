using CardMaga.Rewards.Bundles;
using CardMaga.UI.Visuals;
using System;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [Serializable]
    public class ResourceAmountTextAssigner : BaseTextAssigner<ResourcesCost>
    {
        [SerializeField] ResourceCollectionVisualSO _resourceCollection;
        [SerializeField] TextMeshProUGUI _resourceAmountText;
        const string EMPTY_TEXT = "";
        public override void CheckValidation()
        {
            if (_resourceAmountText == null)
                throw new Exception("ResourceAmountTextAssigner has not _resource Amount Text");
            if (_resourceCollection == null)
                throw new Exception("ResourceAmountTextAssigner ha no resourceCollection");
            _resourceCollection.CheckValidation();
        }

        public override void Dispose()
        {
            _resourceAmountText.AssignText(EMPTY_TEXT);
        }
        public override void Init(ResourcesCost resourceData)
        {
            _resourceAmountText.AssignText(resourceData.Amount.ToString());
            _resourceAmountText.color = _resourceCollection.GetResourceSO(resourceData.CurrencyType).ResourceTextColor;
        }
    }
}