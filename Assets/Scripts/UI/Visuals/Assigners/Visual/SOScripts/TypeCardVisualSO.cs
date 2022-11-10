using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Type BattleCard SO", menuName = "ScriptableObjects/UI/Visuals/Type BattleCard Visual SO")]
    public class TypeCardVisualSO : ScriptableObject
    {
        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public Sprite[] Frames;

        [Tooltip("Attack = 0, Defense = 1, Utility = 2")]
        public Sprite[] InnerFrames;
    }
}
