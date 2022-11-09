using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Glow BattleCard SO", menuName = "ScriptableObjects/UI/Visuals/Glow BattleCard SO")]

    public class GlowCardSO : ScriptableObject
    {
        public Color GlowColor;
        public Sprite GlowSprite;
        public float DefaultAplha;
        public float DiscardAplha;
        public float DiscardAplhaDuration;
    }
}
