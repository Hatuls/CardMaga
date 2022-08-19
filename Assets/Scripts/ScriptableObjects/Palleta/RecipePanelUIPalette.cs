using UnityEngine;
using Sirenix.OdinInspector;
namespace Art
{
    [CreateAssetMenu(fileName = "Recipe Panel UI Palette", menuName = "ScriptableObjects/Art/Recipe Panel UI Palette")]
    public class RecipePanelUIPalette : Palette
    {


        [TitleGroup("RecipePanelUI", BoldTitle = true)]


        #region Button
        [TabGroup("RecipePanelUI/Colors", "Button")]
        [InfoBox("0 - Background\n1 - Text\n2 - Decorate\n3 - Glow")]
        [SerializeField]
        ColorSettings _buttonColors;
        /*
         *  0 - Background
         *  1 - Text 
         *  2 - Decororate
         *  3 - Glow
         */
        public Color ButtonBackground => _buttonColors.Colors[0];
        public Color ButtonText => _buttonColors.Colors[1];
        public Color ButtonDecor => _buttonColors.Colors[2];
        public Color ButtonGlow => _buttonColors.Colors[3];
        #endregion

        #region BackGround & Panel

        [InfoBox("0 - Background\n1 - Recipe Panel\n 2 - Title Text")]
        [TabGroup("RecipePanelUI/Colors", "Recipe UI General")]
        [SerializeField]
        ColorSettings _panelsColors;

        /*
         * 0 - BackGround
         * 1 - Recipe Panel
      * 2 - Tilte Text
         */

        public Color RelicBackgroundColor => _panelsColors.Colors[0];
        public Color RelicPanelColor => _panelsColors.Colors[1];
        public Color RelicTitleTextColor => _panelsColors.Colors[2];

        #endregion
    }
}