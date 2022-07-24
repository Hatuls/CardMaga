using TMPro;
using System;
using Battle.Deck;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [Serializable]
    public class DeckTextAssigner : BaseTextAssigner<BaseDeck>
    {
        //holds amount of card in deck
        [SerializeField] TextMeshProUGUI _deckText;
        BaseDeck _baseDeck;
        public override void Init(BaseDeck baseDeck)
        {
            if (_deckText == null)
                throw new Exception("Deck has no Text");

            _baseDeck = baseDeck;

            _baseDeck.OnAmountOfFilledSlotsChange += SetDeckText;
        }

        public void SetDeckText(int cardAmount)
        {
            AssignText(_deckText, cardAmount.ToString());
        }
        
        ~DeckTextAssigner()
        {
            _baseDeck.OnAmountOfFilledSlotsChange -= SetDeckText;
        }
    }
}
