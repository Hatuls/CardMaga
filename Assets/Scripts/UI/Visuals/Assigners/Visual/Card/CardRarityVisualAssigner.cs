
using Account.GeneralData;
using CardMaga.Card;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class CardRarityVisualAssigner : BaseVisualAssigner<CardCore>
    {
        [SerializeField] RarityCardVisualSO _rarityCardVisualSO;
        [SerializeField] Image _rarity;
        [SerializeField] Image _rarityBG;

        public override void CheckValidation()
        {
            if (_rarityCardVisualSO._rarityBG.Length == 0)
                throw new System.Exception("RarityCardVisualSO has no rarity BG");

            if (_rarityCardVisualSO._rarities.Length == 0)
                throw new System.Exception("RarityCardVisualSO has no rarities");
        }
        public override void Init(CardCore battleCardData)
        {
            int cardRarity = (int)battleCardData.CardSO.Rarity -1;
            //Set Rarity BG
            var sprite = BaseVisualSO.GetSpriteToAssign(cardRarity, cardRarity, _rarityCardVisualSO._rarityBG);
            _rarityBG.AssignSprite(sprite);
            //Set Rarity
            sprite = BaseVisualSO.GetSpriteToAssign(cardRarity, cardRarity, _rarityCardVisualSO._rarities);
            _rarity.AssignSprite(sprite);
        }
        public override void Dispose()
        {
        }
    }
}
