using Sirenix.OdinInspector;
using UnityEngine;
namespace Art
{
    [CreateAssetMenu(fileName = "Card Type Palette", menuName = "ScriptableObjects/Art/Card Type Palette")]
    public class CardTypePalette : Palette
    {

        [TitleGroup("CraftingUIPalette", BoldTitle = true)]

        #region Attack
        [TabGroup("CraftingUIPalette/Colors", "Attack")]
        [InfoBox("0 - Background\n1 - Decoration\n2 - Body Part")]
        [SerializeField]
        ColorSettings _attackColors;

        /* 
         * 0 - Icon Background
         * 1 - Icon Decoration
         * 2 - Body Part Icon
         * 
         */

        public Color AttackIconBackgroundColor => _attackColors.Colors[0];
        public Color AttackIconDecorationColor => _attackColors.Colors[1];
        public Color AttackIconBodyPartColor => _attackColors.Colors[2];
        #endregion


        #region Defense
        [TabGroup("CraftingUIPalette/Colors", "Defense")]
        [InfoBox("0 - Background\n1 - Decoration\n2 - Body Part")]
        [SerializeField]
        ColorSettings _defenseColors;

        /* 
        * 0 - Icon Background
        * 1 - Icon Decoration
        * 2 - Body Part Icon
        * 
        */

        public Color DefenseIconBackgroundColor => _defenseColors.Colors[0];
        public Color DefenseIconDecorationColor => _defenseColors.Colors[1];
        public Color DefenseIconBodyPartColor => _defenseColors.Colors[2];
        #endregion

        #region Utility
        [InfoBox("0 - Background\n1 - Decoration\n2 - Body Part")]
        [TabGroup("CraftingUIPalette/Colors", "Utility")]
        [SerializeField]
        ColorSettings _utilityColors;

        /* 
        * 0 - Icon Background
        * 1 - Icon Decoration
        * 2 - Body Part Icon
        * 
        */

        public Color UtilityIconBackgroundColor => _utilityColors.Colors[0];
        public Color UtilityIconDecorationColor => _utilityColors.Colors[1];
        public Color UtilityIconBodyPartColor => _utilityColors.Colors[2];
        #endregion



        public Color GetIconBodyPartColorFromEnum(Cards.CardTypeEnum cardTypeEnum)
        {
            switch (cardTypeEnum)
            {
                case Cards.CardTypeEnum.Utility:
                    return UtilityIconBodyPartColor;
                case Cards.CardTypeEnum.Defend:
                    return DefenseIconBodyPartColor;
                case Cards.CardTypeEnum.Attack:
                    return AttackIconBodyPartColor;
            }
            return Color.black;
        }
        public Color GetDecorationColorFromEnum(Cards.CardTypeEnum cardTypeEnum)
        {
            switch (cardTypeEnum)
            {
                case Cards.CardTypeEnum.Utility:
                    return UtilityIconDecorationColor;
                case Cards.CardTypeEnum.Defend:
                    return DefenseIconDecorationColor;
                case Cards.CardTypeEnum.Attack:
                    return AttackIconDecorationColor;
            }
            return Color.black;
        }
        public Color GetBackgroundColorFromEnum(Cards.CardTypeEnum cardTypeEnum)
        {
            switch (cardTypeEnum)
            {
                case Cards.CardTypeEnum.Utility:
                    return UtilityIconBackgroundColor;
                case Cards.CardTypeEnum.Defend:
                    return DefenseIconBackgroundColor;
                case Cards.CardTypeEnum.Attack:
                    return AttackIconBackgroundColor;
            }
            return Color.black;
        }
    }




}
