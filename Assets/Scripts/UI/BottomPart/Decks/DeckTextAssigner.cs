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


            baseDeck.OnAmountOfFilledSlotsChange += SetDeckText;
            _baseDeck = baseDeck;
        }

        public void SetDeckText(int cardAmount)
        {
            AssignText(_deckText, cardAmount.ToString());
        }

        public override void OnDestroy()
        {
            if (_baseDeck != null)
            _baseDeck.OnAmountOfFilledSlotsChange -= SetDeckText;
        }

    }
}
