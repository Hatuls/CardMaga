using Sirenix.OdinInspector;
using UnityEngine;

namespace Art
{
    [CreateAssetMenu(fileName = "Crafting UI Palette", menuName = "ScriptableObjects/Art/Crafting UI Palette")]
    public class CraftingUIPalette : Palette
    {
        [TitleGroup("CraftingUIPalette", BoldTitle = true)]

        #region Button
        [TabGroup("CraftingUIPalette/Colors", "Crafting Button")]
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

        #region Crafting Slots
      
        [InfoBox("0 - Background\n1 - Decorate\n2 - Glow")]
        [TabGroup("CraftingUIPalette/Colors", "Slots Button")]
        [SerializeField]
        ColorSettings _slotsColors;

        /*
         * 0 - BackGround
         * 1 - Decoration
         * 2 - Glow
         */

        public Color SlotBackgroundColor => _slotsColors.Colors[0];
        public Color SlotDecorationColor => _slotsColors.Colors[1];
        public Color SlotGlowColor => _slotsColors.Colors[2];
        #endregion
    }




}
