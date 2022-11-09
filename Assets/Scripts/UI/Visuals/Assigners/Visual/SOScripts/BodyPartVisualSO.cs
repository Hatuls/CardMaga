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

        public Sprite GetBodyPartBG(CardTypeEnum cardType)
        {
            if (BodyPartsBG.Length <= (int)cardType)
            {
                return BodyPartsBG[0];
            }
            else
            {
                switch (cardType)
                {
                    case CardTypeEnum.Utility:
                        return BodyPartsBG[3];
                    case CardTypeEnum.Defend:
                        return BodyPartsBG[2];
                    case CardTypeEnum.Attack:
                        return BodyPartsBG[1];
                    case CardTypeEnum.None:
                        return BodyPartsBG[0];
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

