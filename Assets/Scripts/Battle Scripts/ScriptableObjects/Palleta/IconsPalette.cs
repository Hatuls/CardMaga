using Sirenix.OdinInspector;
using UnityEngine;
namespace Art
{
    [CreateAssetMenu(fileName = "Icons Palette", menuName = "ScriptableObjects/Art/Icons Palette")]
    public class IconsPalette : Palette
    {
        [TitleGroup("Icons Color", BoldTitle = true)]



        #region _staminaColors
        [InfoBox("0 - Background\n1 - Glow\n2 - Text")]
        [TabGroup("Icons Color/Colors", "Stamina")]
        [SerializeField]
        ColorSettings _staminaColors;

        /*
         *  0 - background
         *  1 - glow
         *  2 - Text
         */

        public Color StaminaBackgroundColor => _staminaColors.Colors[0];
        public Color StaminaGlowColor => _staminaColors.Colors[1];
        public Color StaminaTextColor => _staminaColors.Colors[2];


        #endregion


        #region _deckColors
        [InfoBox("0 - Background\n1 - Image\n2 - Text\n3 - Decoration")]
        [TabGroup("Icons Color/Colors", "Deck")]
        [SerializeField]
        ColorSettings _deckColors;

        /*
         *  0 - background
         *  1 - Image
         *  2 - Text
         *  3 - Decoration
         */
        public Color DeckBackgroundIconColor => _deckColors.Colors[0];
        public Color DeckImageColor => _deckColors.Colors[1];
        public Color DeckTextColor => _deckColors.Colors[2];
        public Color DeckDecorationColor => _deckColors.Colors[3];
        #endregion

        #region _discardColors
        [InfoBox("0 - Background\n1 - Image\n2 - Text\n3 - Decoration")]
        [TabGroup("Icons Color/Colors", "Discard")]
        [SerializeField]
        ColorSettings _discardColors;

        /*
         *  0 - background
         *  1 - Image
         *  2 - Text
         *  3 - Decoration
         */
        public Color DiscardBackgroundIconColor => _discardColors.Colors[0];
        public Color DiscardImageColor => _discardColors.Colors[1];
        public Color DiscardTextColor => _discardColors.Colors[2];
        public Color DiscardDecorationColor => _discardColors.Colors[3];
        #endregion

        #region End Turn Icon
        [InfoBox("0 - Background\n1 - Glow\n2 - Text\n3 - Decoration")]
        [TabGroup("Icons Color/Colors", "End Turn")]
        [SerializeField]
        ColorSettings _endTurnColors;

        /*
         *  0 - background
         *  1 - glow
         *  2 - Text
         *  3 - Decoration
         */
        public Color EndTurnBackgroundIconColor => _endTurnColors.Colors[0];
        public Color EndTurnGlowColor => _endTurnColors.Colors[1];
        public Color EndTurnTextColor => _endTurnColors.Colors[2];
        public Color EndTurnDecorationColor => _endTurnColors.Colors[3];
        #endregion


        #region Enemy Action Icon
        [InfoBox("0 - Background\n1 - Image\n2 - Text\n3 - Decoration")]
        [TabGroup("Icons Color/Colors", "Enemy Icon")]
        [SerializeField]
        ColorSettings _enemyIcon;

        /*
         *  0 - background
         *  1 - Image
         *  2 - Text
         *  3 - Decoration
         */

        public Color EnemyIconBackgroundIconColor => _enemyIcon.Colors[0];
        public Color EnemyIconImageColor => _enemyIcon.Colors[1];
        public Color EnemyIconTextColor => _enemyIcon.Colors[2];
        public Color EnemyIconDecorationColor => _enemyIcon.Colors[3];
        #endregion


        #region Settings Icon
        [InfoBox("0 - Background\n1 - Image")]
        [TabGroup("Icons Color/Colors", "Settings Icon")]
        [SerializeField]
        ColorSettings _settingsIcon;
        /*
         *  0 - background
         *  1 - Image
         */

        public Color SettingsIconBackgroundIconColor => _settingsIcon.Colors[0];
        public Color SettingsIconImageColor => _settingsIcon.Colors[1];

        #endregion

    }




}
