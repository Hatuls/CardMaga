using UnityEngine;

namespace UI.Visuals
{
    [CreateAssetMenu(fileName = "Type Card SO", menuName = "ScriptableObjects/UI/Visuals/Type Card Visual SO")]
    public class TypeCardVisualSO : ScriptableObject
    {
        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public Sprite[] Frames;

        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public Sprite[] InnerFrames;
    }
}
