using System.Collections.Generic;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaCharacterData
    {
        #region Fields

        private int _id;
        private int _currentSkin;
        private int _exp;
        private int _skillPoint;
        private int _rank;
        private int _deckAmount = 1;
        private int _mainDeck;
        private int _maxDeckLimit;

        private List<int> _availableSkins;
        private List<MetaDeckData> _decks;

        #endregion

        #region Prop
        
        public int CurrentSkin { get => _currentSkin; set => _currentSkin = value; }
        public int SkillPoint { get => _skillPoint; set => _skillPoint = value; }
        public int Rank { get => _rank; set => _rank = value; }
        public int Exp { get => _exp; set => _exp = value; }
        public int MainDeck { get => _mainDeck; private set => _mainDeck = value; }
        public int Id { get => _id; private set => _id = value; }
        public IReadOnlyList<int> AvailableSkins => _availableSkins;
        public IReadOnlyList<MetaDeckData> Decks => _decks; 
        public int DeckLimit => _deckAmount;//limit

        #endregion

        public MetaCharacterData(Character character)
        {
            _id = character.Id;
            _currentSkin = character.CurrentSkin;
            _exp = character.Exp;
            _skillPoint = character.SkillPoint;
            _rank = character.Rank;
            _deckAmount = character.Deck.Count; //need to check rei _____
            _maxDeckLimit = character.DeckLimit;
            _mainDeck = character.MainDeck;

            _availableSkins = new List<int>(character.AvailableSkins.Count);

            for (int i = 0; i < character.AvailableSkins.Count; i++)
            {
                _availableSkins.Add(character.AvailableSkins[i]);
            }

            _decks = new List<MetaDeckData>(character.Deck.Count);

            for (int i = 0; i < character.Deck.Count; i++)
            {
                _decks.Add(new MetaDeckData(character.Deck[i]));
            }
        }

        public bool TryAddDeck()
        {
            if (_decks.Count >= _maxDeckLimit)
            {
                Debug.LogWarning("Max number of decks");
                return false;
            }
            
            _decks.Add(new MetaDeckData(new DeckData(_decks.Count)));
            return true;
        }
    }
}