using CardMaga.Card;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    [CreateAssetMenu(fileName = "Rarity BattleCard SO", menuName = "ScriptableObjects/UI/Visuals/Rarity BattleCard Visual SO")]
    public class RarityCardVisualSO : ScriptableObject
    {
        [Tooltip("Common = 0, Uncommon = 1, Rare = 2, Epic = 3, Legendery = 4")]
        public Sprite[] _rarityBG;
        [Tooltip("Common = 0, Uncommon = 1, Rare = 2, Epic = 3, Legendery = 4")]
        public Sprite[] _rarities;

        public Sprite GetRarity(RarityEnum rarityEnum)
        {
            int rarityIndex = (int)rarityEnum;
            rarityIndex -= 1; // not interested in None value

            if (rarityIndex >= 0)
                return _rarities[rarityIndex];
            else
                throw new System.Exception($"Rarity requested is not valid\nRarity input {rarityEnum.ToString()}");
        }
    }
}
