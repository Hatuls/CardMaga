using CardMaga.Card;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Title and Arrow Combo SO", menuName = "ScriptableObjects/UI/Visuals/Title And Arrow Combo SO")]

    public class TitleAndArrowComboVisualSO : BaseVisualSO
    {
        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public Sprite[] Arrows;

        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public Sprite[] Titles;

        public override void CheckValidation()
        {
            if (Arrows.Length == 0)
                throw new System.Exception("TitleAndArrowComboVisualSO has no arrows");
            if (Titles.Length == 0)
                throw new System.Exception("TitleAndArrowComboVisualSO has no titles");
        }

        public Sprite GetArrowSprite(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.Attack:
                    return GetSpriteToAssign(0, 0, Arrows);
                case CardTypeEnum.Defend:
                    return GetSpriteToAssign(0, 1, Arrows);
                case CardTypeEnum.Utility:
                    return GetSpriteToAssign(0, 2, Arrows);
                default:
                    throw new System.Exception("TitleAndArrowComboVisualSO has no Arrow sprite to assign");
            }
        }
        public Sprite GetTitleSprite(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.Attack:
                    return GetSpriteToAssign(0, 0, Titles);
                case CardTypeEnum.Defend:
                    return GetSpriteToAssign(0, 1, Titles);
                case CardTypeEnum.Utility:
                    return GetSpriteToAssign(0, 2, Titles);
                default:
                    throw new System.Exception("TitleAndArrowComboVisualSO has no Title sprite to assign");
            }
        }

    }
}