using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaCardInstanceInfo : IDisposable
    {
        [SerializeField, ReadOnly] 
        private CardInstance _cardInstance;
        /// <summary>
        ///Associate Deck first number the deck Id, Second number is the number of the Instant in the deck
        /// </summary>
        [SerializeField, ReadOnly] 
        private List<int> _associateDeck;
        
        public int InstanceID => _cardInstance.InstanceID;
        public int CoreID => _cardInstance.CoreID;
        public IReadOnlyList<int> AssociateDeck => _associateDeck;
        /// <summary>
        /// Is true if the CardInstance is in at least one deck
        /// </summary>
        public bool InDeck => _associateDeck.Count > 0;

        public MetaCardInstanceInfo(CardInstance metaCardInstance)
        {
            _cardInstance = metaCardInstance;
            _associateDeck = new List<int>();
        }

        public CardInstance GetCardData()
        {
            var cache = _cardInstance;
            
            return cache;
        }

        public void RemoveFromDeck(int deckId)
        {
            if (!_associateDeck.Contains(deckId))
            {
                Debug.LogWarning( this +" DeckId was not found");
                return;
            }

            _associateDeck.Remove(deckId);
        }

        public void AddToDeck(int deckId)
        {
            if (_associateDeck.Contains(deckId))
            {
                Debug.LogWarning( this +" Deckid was already in list");
                return;
            }
            
            _associateDeck.Add(deckId);
        }

        public bool IsInDeck(int deckID)
        {
            return _associateDeck.Any(DeckId => DeckId == deckID);
        }

        public void Dispose()
        {
            
        }
    }
}