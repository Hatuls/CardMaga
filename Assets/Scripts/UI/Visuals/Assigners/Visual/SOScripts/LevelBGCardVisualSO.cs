using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "LevelBG Card SO", menuName = "ScriptableObjects/UI/Visuals/Level BG SO")]
    public class LevelBGCardVisualSO : ScriptableObject
    {
        public Sprite LevelsBG;
        public Sprite LevelsBGOutline;
    }
}
