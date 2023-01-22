using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "LevelBG BattleCard SO", menuName = "ScriptableObjects/UI/Visuals/Level BG SO")]
    public class LevelBGCardVisualSO : ScriptableObject
    {
        public Sprite LevelsBG;
        public Sprite LevelsGoldBG;
        public Sprite LevelsBGOutline;
    }
}
