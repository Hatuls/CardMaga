using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Glow Card SO", menuName = "ScriptableObjects/UI/Visuals/Glow Card SO")]

    public class GlowCardSO : ScriptableObject
    {
        public Color GlowColor;
        public Sprite GlowSprite;
    }
}
