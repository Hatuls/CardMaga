using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Visuals
{
    [System.Serializable]
    public class CardRarityVisualAssigner : BaseVisualAssigner
    {
        [SerializeField] RarityCardVisualSO _rarityCardVisualSO;
        [SerializeField] Image _rarity;
        [SerializeField] Image _rarityBG;
        public override void Init()
        {
            if (_rarityCardVisualSO._rarityBG.Length == 0)
                throw new System.Exception("RarityCardVisualSO has no rarity BG");

            if (_rarityCardVisualSO._rarities.Length == 0)
                throw new System.Exception("RarityCardVisualSO has no rarities");
        }

        public void SetRarity(int cardRarityNum)
        {
            var cardRarity = cardRarityNum - 1;
            //Set Rarity BG
            var sprite = GetSpriteToAssign(cardRarity, cardRarity, _rarityCardVisualSO._rarityBG);
            AssignSprite(_rarityBG, sprite);
            //Set Rarity
            sprite = GetSpriteToAssign(cardRarity, cardRarity, _rarityCardVisualSO._rarities);
            AssignSprite(_rarity, sprite);
        }
    }
}
