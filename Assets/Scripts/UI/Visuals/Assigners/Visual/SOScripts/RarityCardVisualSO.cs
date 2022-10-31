using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Rarity Card SO", menuName = "ScriptableObjects/UI/Visuals/Rarity Card Visual SO")]
    public class RarityCardVisualSO : ScriptableObject
    {
        [Tooltip("Common = 0, Uncommon = 1, Rare = 2, Epic = 3, Legendery = 4")]
        public Sprite[] _rarityBG;
        [Tooltip("Common = 0, Uncommon = 1, Rare = 2, Epic = 3, Legendery = 4")]
        public Sprite[] _rarities;
    }
}
