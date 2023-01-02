using TMPro;
using System;
using Battle.Deck;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [Serializable]
    public class DeckTextAssigner : BaseTextAssigner<BaseDeck>
    {
        //holds amount of battleCard in deck
        [SerializeField] TextMeshProUGUI _deckText;
        BaseDeck _baseDeck;
        public override void Init(BaseDeck comboData)
        {
            SetDeckText(0);
            comboData.OnAmountOfFilledSlotsChange += SetDeckText;
            _baseDeck = comboData;
        }

        private void SetDeckText(int cardAmount)
        {
            _deckText.AssignText(cardAmount.ToString());
        }

        public override void CheckValidation()
        {
            if (_deckText == null)
                throw new Exception("Deck has no Text");
        }

        public override void Dispose()
        {
            if (_baseDeck != null)
                _baseDeck.OnAmountOfFilledSlotsChange -= SetDeckText;
        }
    }
}
