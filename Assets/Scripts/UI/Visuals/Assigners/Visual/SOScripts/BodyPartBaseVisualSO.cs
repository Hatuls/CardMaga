using System;
using CardMaga.Card;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Body Parts BasePoolObject SO", menuName = "ScriptableObjects/UI/Visuals/Body Part base Visual SO")]
    public class BodyPartBaseVisualSO : ScriptableObject, ICheckValidation
    {
        [Tooltip("Empty = 0, Head = 1, Elbow = 2, Hand = 3, Knee = 4, Leg = 5, Joker = 6")]
        public Sprite[] BodyParts;
        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Color[] MainColor;
        [Tooltip("Attack - 0, Defense - 1, Utility - 2")]
        public Color[] InnerBGColor;

        public void CheckValidation()
        {
            if (InnerBGColor.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BG Color");

            if (MainColor.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no Main Color");

            if (BodyParts.Length == 0)
                throw new Exception("CardBodyPartVisualAssigner has no BodyParts");
        }
        public Sprite GetBodyPartSprite(CardMaga.Card.BodyPartEnum bodyPartEnum)
        {
            switch (bodyPartEnum)
            {
                case CardMaga.Card.BodyPartEnum.Empty:
                    return BodyParts[0];
                case CardMaga.Card.BodyPartEnum.Head:
                    return BodyParts[1];
                case CardMaga.Card.BodyPartEnum.Elbow:
                    return BodyParts[2];
                case CardMaga.Card.BodyPartEnum.Hand:
                    return BodyParts[3];
                case CardMaga.Card.BodyPartEnum.Knee:
                    return BodyParts[4];
                case CardMaga.Card.BodyPartEnum.Leg:
                    return BodyParts[5];
                case CardMaga.Card.BodyPartEnum.Joker:
                    return BodyParts[6];
                default:
                    throw new ArgumentOutOfRangeException(nameof(bodyPartEnum), bodyPartEnum, null);
            }
        }

        public Color GetMainColor(CardTypeEnum cardTypeEnum)
        {
            switch (cardTypeEnum)
            {
                case CardTypeEnum.Utility:
                    return MainColor[3];
                case CardTypeEnum.Defend:
                    return MainColor[2];
                case CardTypeEnum.Attack:
                    return MainColor[1];
                case CardTypeEnum.None:
                    return MainColor[0];
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardTypeEnum), cardTypeEnum, null);
            }
        }

        public Color GetInnerColor(CardTypeEnum cardTypeEnum)
        {
            switch (cardTypeEnum)
            {
                case CardTypeEnum.Utility:
                    return InnerBGColor[3];
                case CardTypeEnum.Defend:
                    return InnerBGColor[2];
                case CardTypeEnum.Attack:
                    return InnerBGColor[1];
                case CardTypeEnum.None:
                    return InnerBGColor[0];
                default:
                    throw new ArgumentOutOfRangeException(nameof(cardTypeEnum), cardTypeEnum, null);
            }
        }
    }
    //None = 0, Empty = 1, Head = 2, Elbow = 3, Hand = 4, Knee = 5, Leg = 6, Joker = 7 
}
