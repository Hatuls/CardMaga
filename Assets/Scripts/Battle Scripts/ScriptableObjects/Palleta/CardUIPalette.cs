using CardMaga.Card;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Art
{
    [CreateAssetMenu(fileName = "Card UI Palette", menuName = "ScriptableObjects/Art/Card UI Palette")]
    public class CardUIPalette : Palette
    {
        [TitleGroup("Card UI", BoldTitle = true)]


        #region Card UI BackGround
        [TabGroup("Card UI/Colors", "BackGround Sprite")]
        [InfoBox("0 - Background")]
        [SerializeField] Sprite[] _frames;
        public Sprite GetCardUIImage(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.Utility:
                    return _frames[0];

                case CardTypeEnum.Defend:
                    return _frames[1];
                case CardTypeEnum.Attack:
                    return _frames[2];

                case CardTypeEnum.None:
                default:
                    throw new System.Exception("CardUIPallete: Cannot Return Sprite Based on the cardtypeEnum " + cardType);

            }
        }


        #endregion

        #region Card UI Glow
        [TabGroup("Card UI/Colors", "Glow")]
        [InfoBox("0 - Clicked\n 1 - Combo Detected")]
        [SerializeField]
        ColorSettings _cardUIGlow;

        /*
         *  0 - Glow Clicked
         *  1 - Combo Detected
         */

        public Color CardClickedGlowColor => _cardUIGlow.Colors[0];
        #endregion

        #region Stamina On Card
        [TabGroup("Card UI/Colors", "Stamina")]
        [InfoBox("0 - Background\n1 - Text\n2 - Decorate")]
        [SerializeField]
        ColorSettings _staminaColors;
        /*
         *  0 - Background
         *  1 - Text 
         *  2 - Decorate
         */
        public Color StaminaBackgroundColor => _staminaColors.Colors[0];
        public Color StaminaTextColor => _staminaColors.Colors[1];
        public Color StaminaDecorateColor => _staminaColors.Colors[2];


        #endregion

        #region Body Part Icon On Card

        [TabGroup("Card UI/Colors", "Body Part Icon")]
        [InfoBox("0 - Background\n1 - Image\n2 - Decoration (Out Line)")]
        [SerializeField]
        ColorSettings _bodyPartIconColors;
        /*
         *  0 - Background
         *  1 - Decoration
         */
        public Color BodyPartIconBackgroundColor => _bodyPartIconColors.Colors[0];
        public Color BodyPartIconDecorationColor => _bodyPartIconColors.Colors[1];
        #endregion

        #region Card Information

        [TabGroup("Card UI/Colors", "Card Information")]
        [InfoBox("0 - Background\n1 - Title Text\n2 - Description Text")]
        [SerializeField]
        ColorSettings _cardInformation;
        /*
         *  0 - Background
         *  1 - Title Text 
         *  2 - Description Text
         */
        public Color CardInformationIconBackgroundColor => _cardInformation.Colors[0];
        public Color CardInformationTitleTextColor => _cardInformation.Colors[1];
        public Color CardInformationDescriptionTextColor => _cardInformation.Colors[2];
        #endregion

        #region Rarity Colors
        [SerializeField]
        [TabGroup("Card UI/Colors", "Card Rarity")]
        [InfoBox("0 - Common\n1 - UnCommon\n2 - Rare\n3 - Epic\n4 - LegendRei")]
        ColorSettings _rarityColors;


        public Color GetRarityColor(RarityEnum rarityEnum)
        {
            Color[] colors = _rarityColors.Colors;

            switch (rarityEnum)
            {
                case RarityEnum.Common:
                    return colors[0];
                case RarityEnum.Uncommon:
                    return colors[1];
                case RarityEnum.Rare:
                    return colors[2];
                case RarityEnum.Epic:
                    return colors[3];
                case RarityEnum.LegendREI:
                    return colors[4];
                case RarityEnum.None:
                default:
                    throw new System.Exception($"Rarity Enum selected is not valid!\nInput {rarityEnum}");
            }
        }
        #endregion
    }


}
