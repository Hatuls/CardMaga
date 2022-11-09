using CardMaga.Meta.AccountMetaData;
using UnityEngine;

namespace CardMaga.MetaData.DeckBuilder
{
    public class DeckBuilder
    {
        private const int MAX_CARD_IN_DECK = 8;
        private const int MAX_COMBO_IN_DECK = 3;

        private MetaDeckData _deck;

        public void AssingDeckToEdit(MetaDeckData deckData)
        {
            _deck = deckData;
        }

        public bool TryAddCard(MetaCardData cardData)
        {
            if (_deck.Cards.Count >= MAX_CARD_IN_DECK)
            {
                Debug.LogWarning("MAX_CARD_IN_DECK");
                return false;
            }
            
            _deck.AddCard(cardData);
            return true;
        }

        public bool TryAddCombo(MetaComboData comboData)
        {
            if (_deck.Combos.Count >= MAX_COMBO_IN_DECK)
            {
                Debug.LogWarning("MAX_CARD_IN_DECK");
                return false;
            }
            
            _deck.AddCombo(comboData);
            return true;
        }

        public void RemoveCard(int id)
        {
            
        }

        public void RemoveCombo(int id)
        {
            
        }
    }
}