using CardMaga.Card;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    public struct RarityTextData
    {
        public RarityEnum RarityType;
        public string RarityAmount;
        public string RarityText
        {
            get
            {
                switch (RarityType)
                {
                    case RarityEnum.LegendREI:
                        return "Legendary";
                    default:
                        return RarityType.ToString();

                }
            }
        }
    }

    [System.Serializable]
    public class RarityTabTextAssigner : BaseTextAssigner<RarityTextData>
    {
        [SerializeField]
        private TextMeshProUGUI _rarityText;
        [SerializeField]
        private TextMeshProUGUI _rarityAmount;

        public override void CheckValidation()
        {
            if (_rarityText == null)
                throw new System.Exception($"Text Mesh Pro is not assigned:\nField: _rarityText");
            else if (_rarityAmount == null)
                throw new System.Exception($"Text Mesh Pro is not assigned:\nField: _rarityAmount");

        }

        public override void Dispose()
        {

        }

        public override void Init(RarityTextData data)
        {
            _rarityText.text = data.RarityText;
            _rarityAmount.text = data.RarityAmount;
        }
    }



}