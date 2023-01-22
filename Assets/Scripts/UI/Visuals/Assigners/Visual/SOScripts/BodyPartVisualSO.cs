using CardMaga.Card;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class BodyPartVisualSO : BaseVisualSO
    {
        [Tooltip("BasePoolObject SO that holds body parts and colors")]
        public BodyPartBaseVisualSO BaseSO;

        [Tooltip("Default - 0, Attack - 1, Defense - 2, Utility - 3")]
        public Sprite[] BodyPartsBG;

        [Tooltip("Default - 0, Attack - 1, Defense - 2, Utility - 3")]
        public Sprite[] BodyPartsInnerBG;

        [Tooltip("Default - 0, Attack - 1, Defense - 2, Utility - 3")]
        public Sprite[] GoldBodyPartBG;

        public Sprite GetBodyPartBG(CardSO card)
        {
            var cardType = card.CardTypeEnum;
            if (BodyPartsBG.Length <= (int)cardType)
            {
                return BodyPartsBG[0];
            }
            else
            {
                switch (cardType)
                {
                    case CardTypeEnum.Utility:
                        return card.IsCombo? GoldBodyPartBG[3] : BodyPartsBG[3];
                    case CardTypeEnum.Defend:
                        return card.IsCombo ? GoldBodyPartBG[2] : BodyPartsBG[2];
                    case CardTypeEnum.Attack:
                        return card.IsCombo ? GoldBodyPartBG[1] : BodyPartsBG[1];
                    case CardTypeEnum.None:
                        return card.IsCombo ? GoldBodyPartBG[0] : BodyPartsBG[0];
                    default:
                        throw new System.Exception($"BodyPartVisualSO Have recived Unknown BattleCard Type {cardType}");
                }
            }
        }
        public Sprite GetBodyPartInnerBG(CardTypeEnum cardType)
        {
            if (BodyPartsBG.Length <= (int)cardType)
            {
                return BodyPartsInnerBG[0];
            }
            else
            {
                switch (cardType)
                {
                    case CardTypeEnum.Utility:
                        return BodyPartsInnerBG[3];
                    case CardTypeEnum.Defend:
                        return BodyPartsInnerBG[2];
                    case CardTypeEnum.Attack:
                        return BodyPartsInnerBG[1];
                    case CardTypeEnum.None:
                        return BodyPartsInnerBG[0];
                    default:
                        throw new System.Exception($"BodyPartVisualSO Have recived Unknown BattleCard Type {cardType}");
                }
            }
        }
        public override void CheckValidation()
        {
            BaseSO.CheckValidation();

            if (BodyPartsBG == null)
                throw new System.Exception("BodyPartVisualSO has no BodyPartsBG");
            if (BodyPartsInnerBG == null)
                throw new System.Exception("BodyPartVisualSO has no BodyOartsInnerBG");
        }
    }
}

