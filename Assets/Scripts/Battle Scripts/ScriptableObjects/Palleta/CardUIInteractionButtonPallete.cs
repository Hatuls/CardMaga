using UnityEngine;
namespace Art
{
    [CreateAssetMenu(fileName = "CardUIInteractionPallete", menuName = "ScriptableObjects/Art/CardUI Interaction Button Colors Pallete"),]
    public class CardUIInteractionButtonPallete : Palette
    {
        [SerializeField]
        [Sirenix.OdinInspector.InfoBox("0 - Info Button Color\n1 -Remove Button Color\n2 - Use Button Color\n3 - Dismental Button Color\n4 - Buy Button Colo")]
        Art.ColorSettings _colorSettings;
        public Color GetInteractionColor(UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum type)
        {
            switch (type)
            {

                case UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Info:

                    return _colorSettings.Colors[0];
                case UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Remove:
                    return _colorSettings.Colors[1];
                case UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Use:
                    return _colorSettings.Colors[2];
                case UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Dismental:
                    return _colorSettings.Colors[3];
                case UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.Buy:
                    return _colorSettings.Colors[4];
                case UI.Meta.Laboratory.MetaCardUIInteractionPanel.MetaCardUiInteractionEnum.None:
                default:
                    Debug.LogWarning("Meta card UIInteraction was not valid ");
                    return Color.clear;

            }
        }
    }

}