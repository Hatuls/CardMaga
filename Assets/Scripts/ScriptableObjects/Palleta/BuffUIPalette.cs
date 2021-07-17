using UnityEngine;
using Sirenix.OdinInspector;
namespace Art
{
    [CreateAssetMenu(fileName = "Buff Palette", menuName = "ScriptableObjects/Art/Buff Palette")]
    public class BuffUIPalette : Palette
    {
        [TitleGroup("Buff UI", BoldTitle = true)]


        /*
         *  Background
         *  Image
         *  Text
         *  Decorate
         */



        #region Card UI BackGround
        [TabGroup("Buff UI/Colors", "Background")]
        [InfoBox("0 - Background")]
        [SerializeField]
        ColorSettings _backGroundColors;
        /*
         *  0 -Default Background
         */
        public Color CardDefaultBackground => _backGroundColors.Colors[0];
        #endregion

        #region Card UI Images
        [TabGroup("Buff UI/Colors", "Image")]
        [InfoBox("0 - Strength\n1 - Bleed")]
        [SerializeField]
        ColorSettings _buffImageColors;
        /*
         *  0 - Strength
         *  1 - Bleed
         */
    
        public Color GetBuffIconFromColor(Keywords.KeywordTypeEnum keywordTypeEnum)
        {
            Color clr = Color.black;
            switch (keywordTypeEnum)
            {   case Keywords.KeywordTypeEnum.MaxHealth:
                case Keywords.KeywordTypeEnum.Attack:
                case Keywords.KeywordTypeEnum.Defense:
                case Keywords.KeywordTypeEnum.Heal:
               default:
                    break;


                case Keywords.KeywordTypeEnum.Strength:
                    clr = _buffImageColors.Colors[0];
                    break;
                case Keywords.KeywordTypeEnum.Bleed:
                    clr = _buffImageColors.Colors[1];
                    break;
                
            }

            return clr;
        }
        #endregion

        #region Card UI Text
        [TabGroup("Buff UI/Colors", "Text")]
        [InfoBox("0 - Default Text")]
        [SerializeField]
        ColorSettings _textColors;
        /*
         *  0 -Default Text
         */
        public Color CardDefaultTextColor => _textColors.Colors[0];
        #endregion

        #region Card UI Decorate
        [TabGroup("Buff UI/Colors", "Decorate")]
        [InfoBox("0 - Default Decorate")]
        [SerializeField]
        ColorSettings _decorateColors;
        /*
         *  0 -Default Background
         */
        public Color CardDefaultDecorateColor => _decorateColors.Colors[0];
        #endregion

        #region Armor
        [TabGroup("Buff UI/Colors", "Armor Icon")]
        [InfoBox("0 - Strength\n1 - Bleed")]
        [SerializeField]
        ColorSettings _armorColors;
        /*
         * 0 - background
         * 1 - Decorate
         * 2 - Text
         */

        public Color ArmorIconBackGroundColor => _armorColors.Colors[0];
        public Color ArmorIconDecorationColor => _armorColors.Colors[1];
        public Color ArmorIconTextColor => _armorColors.Colors[2];


        #endregion


    }
}