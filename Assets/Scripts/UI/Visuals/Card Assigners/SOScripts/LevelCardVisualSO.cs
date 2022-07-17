using UnityEngine;

namespace UI.Visuals
{
    [CreateAssetMenu(fileName = "Level Card SO", menuName = "ScriptableObjects/UI/Visuals/Level SO")]
    public class LevelCardVisualSO : ScriptableObject
    {
        public LevelBGCardVisualSO LevelBGCardVisualSO;
        public Sprite[] OuterLevel;
        public Sprite[] InnerLevel;
        public Sprite[] EmptyOuterLevel;
        public Sprite[] EmptyInnerLevel;
        public Color EmptyColor;
        public Color FullColor;
    }
}
