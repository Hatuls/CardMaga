using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class MetaComboInstanceInfo : IEquatable<MetaComboInstanceInfo>,IEquatable<int>
    {
        [SerializeField, ReadOnly] private ComboInstance _comboInstance;
        [SerializeField,ReadOnly]
        private List<int> _associateDeck;

        public ComboInstance ComboInstance => _comboInstance;

        public int InstanceID => _comboInstance.InstanceID;

        public int CoreID => _comboInstance.CoreID;

        public bool InDeck => _associateDeck.Count > 0;

        public MetaComboInstanceInfo(ComboInstance comboInstance)
        {
            _associateDeck = new List<int>();
            _comboInstance = comboInstance;
        }

        public void AddDeckReference(int deckId)
        {
            _associateDeck.Add(deckId);
        }

        public bool RemoveDeckReference(int deckId)
        {
            if (!_associateDeck.Contains(deckId)) return false;
            _associateDeck.Add(deckId);
            return true;

        }
        
        public bool IsInDeck(int deckID)
        {
            return _associateDeck.Any(DeckId => DeckId == deckID);
        }

        public bool Equals(MetaComboInstanceInfo other)
        {
            return other != null && InstanceID == other.InstanceID;
        }

        public bool Equals(int other)
        {
            return CoreID == other;
        }
    }
}