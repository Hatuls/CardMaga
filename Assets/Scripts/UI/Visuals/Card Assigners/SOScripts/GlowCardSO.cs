using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Visuals
{
    [CreateAssetMenu(fileName = "Glow Card SO", menuName = "ScriptableObjects/UI/Visuals/Glow Card SO")]

    public class GlowCardSO : ScriptableObject
    {
        public Color _glowColor;
        public Sprite _glowSprite;
    }
}
