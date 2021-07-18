using UnityEngine;
using Sirenix.OdinInspector;
namespace Art
{
    [CreateAssetMenu(fileName = "Card UI Palette", menuName = "ScriptableObjects/Art/Card UI Palette")]
    public class CardUIPalette : Palette
    {
        [TitleGroup("Card UI", BoldTitle = true)]


        #region Card UI BackGround
        [TabGroup("Card UI/Colors", "BackGround")]
        [InfoBox("0 - Background")]
        [SerializeField]
        ColorSettings _backGroundColors;
        /*
         *  0 -Default Background
         */
        public Color CardDefaultBackgroundColor => _backGroundColors.Colors[0];
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
    }


}
