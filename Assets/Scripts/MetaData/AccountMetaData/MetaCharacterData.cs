using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection.DeckCollection;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    public class MetaCharacterData
    {
        #region Fields

        private int _id;
        private int _currentSkin;
        private int _exp;
        private int _skillPoint;
        private int _rank;
        private int _deckAmount = 4;
        private int _mainDeckIndex;
        private int _maxDeckLimit;

        private Character _characterData;

        private MetaDeckIDGenerator _deckIDGenerator;

        private List<int> _availableSkins;
        private List<MetaDeckData> _decks;

        #endregion

        #region Prop
        
        public int CurrentSkin { get => _currentSkin; set => _currentSkin = value; }
        public int SkillPoint { get => _skillPoint; set => _skillPoint = value; }
        public int Rank { get => _rank; set => _rank = value; }
        public int Exp { get => _exp; set => _exp = value; }

        public MetaDeckData MainDeck
        {
            get => _decks[_mainDeckIndex];
        }

        public int Id
        {
            get => _id;
        }

        public IReadOnlyList<int> AvailableSkins => _availableSkins;
        public IReadOnlyList<MetaDeckData> Decks => _decks; 
        public int DeckLimit => _deckAmount;//limit

        #endregion

        public MetaCharacterData(Character character)
        {
            _characterData = character;
            _id = character.ID;
            _currentSkin = character.CurrentColor;
            _exp = character.EXP;
            _skillPoint = character.SkillPoints;
            _rank = character.Rank;
            _deckAmount = character.Deck.Count; //need to check rei _____
            _maxDeckLimit = character.DeckAmount;
            _mainDeckIndex = character.MainDeck;

            _availableSkins = new List<int>(character.AvailableSkins.Count);

            _deckIDGenerator = new MetaDeckIDGenerator();

            for (int i = 0; i < character.AvailableSkins.Count; i++)
            {
                _availableSkins.Add(character.AvailableSkins[i]);
            }

            _decks = new List<MetaDeckData>(character.Deck.Count);

            for (int i = 0; i < character.Deck.Count; i++)
            {
                _decks.Add(new MetaDeckData(character.Deck[i],i,false));
            }
        }

        public MetaDeckData AddDeck()
        {
            if (_decks.Count >= _maxDeckLimit)
            {
                Debug.LogWarning("Max number of decks");
                return null;
            }

            MetaDeckData cache = new MetaDeckData(new DeckData(_deckIDGenerator.GetNewDeckID(_decks.ToArray())),_decks.Count,true);
            _decks.Add(cache);
            
            SetMainDeck(_decks.Count - 1);

            return cache;
        }

        public void SetMainDeck(int deckID)
        {
            if (deckID > _decks.Count)
            {
                Debug.LogWarning("Invalid deck index");
                return;
            }
            
            int cache = FindDeckIndexByID(deckID);
            _characterData.SetMainDeck(cache);
            _mainDeckIndex = cache;
        }

        private int FindDeckIndexByID(int deckId)
        {
            for (int i = 0; i < _decks.Count; i++)
            {
                if (_decks[i].DeckId == deckId)
                {
                    return i;
                }
            }

            return -1;
        }

        public void UpdateDeck(MetaDeckData metaDeckData,int deckIndex)
        {
            _decks[deckIndex].UpdateDeck(metaDeckData);
        }
    }
}