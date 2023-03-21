using UnityEngine;
using Sirenix.OdinInspector;
namespace Art
{
    [CreateAssetMenu(fileName = "Bars UI Palette", menuName = "ScriptableObjects/Art/Bars UI Palette")]
    public class BarsUIPalette : Palette
    {


        [TitleGroup("Bars UI Palette", BoldTitle = true)]


        #region Hp Bar
        [TabGroup("Bars UI Palette/Colors", "HP Bar")]
        [InfoBox("0 - Background\n1 - Fill\n2 - Text\n3 - Glow")]
        [SerializeField]
        ColorSettings _hpBar;

        /*
         *  0 - Background
         *  1 - Fill
         *  2 - Text 
         *  3 - Glow
         */

        public Color HPBarBackground => _hpBar.Colors[0];
        public Color HPBarFill => _hpBar.Colors[1];
        public Color HPBarText => _hpBar.Colors[2];
        public Color HPBarGlow => _hpBar.Colors[3];

        #endregion

  
    }
}