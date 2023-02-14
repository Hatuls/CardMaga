﻿using Battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Account.GeneralData
{
    [Serializable]
    public class CharactersData
    {
        [NonSerialized]
        public const string PlayFabKeyName = "CharactersData";

        public List<Character> Characters = new List<Character>();
        public int MainCharacterID = 1;

        public Character GetMainCharacter() => Characters.FirstOrDefault(x => x.ID == MainCharacterID);
        public void AddCharacter(Character character)
        {
            if (!Characters.Contains(character) && !ContainID(character.ID))
            {
                Characters.Add(character);
            }
            bool ContainID(int id)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    if (Characters[i].ID == id)
                        return true;
                }
                return false;
            }
        }
        public bool IsValid()
       => Characters.Count > 0;

        public CharactersData()
        {
            
        }
    }
    [Serializable]
    public class Character
    {
        public int ID;
        public int CurrentModel;
        public int CurrentColor;
        public int EXP;
        public int SkillPoints;
        public int Rank;
        public int DeckAmount;
        public int MainDeck;

        public List<int> AvailableSkins = new List<int>();
        public List<DeckData> Deck = new List<DeckData>();

        public Character(CharacterSO newCharacter)
        {
            ID = newCharacter.ID;
            CurrentColor = 0;
            EXP = 0;
            SkillPoints = 0;
            Rank = 0;
            DeckAmount = 4;
            CurrentModel = 0;
            MainDeck = 0;
        }
        
        public bool AddNewDeck(CardInstance[] deckCards, ComboCore[] deckCombos)
        {
            bool canAddDeck = Deck.Count < DeckAmount;//do the check twins

            if (canAddDeck)
            {
                CoreID[] cards = new CoreID[deckCards.Length];

                for (int i = 0; i < cards.Length; i++)
                    cards[i] = new CoreID(deckCards[i].CoreID);
                
                return AddNewDeck(cards, deckCombos);
            }
            
            return false;
        }
        
        public bool AddNewDeck(DeckData deckData)
        {
            bool canAddDeck = Deck.Count < DeckAmount;
            
            if (canAddDeck)
            {
                Deck.Add(deckData);
                MainDeck = Deck.Count - 1;
            }
            return canAddDeck;
        }

        public bool AddNewDeck(CoreID[] deckCards, ComboCore[] deckCombos)
        {
            bool canAddDeck = Deck.Count < DeckAmount;
            
            if (canAddDeck)
            {
                Deck.Add(new DeckData(Deck.Count, "New Deck", deckCards, deckCombos));
            }
            return canAddDeck;
        }
        
        public void SetMainDeck(int deckId)
        {
            MainDeck = deckId;
            
            // for (int i = 0; i < Deck.Count; i++)
            // {
            //     if (Deck[i].Id == deckId )
            //     {
            //         MainDeck = i;//need to check rei!
            //     }
            // }
        }

        public bool TryRemoveDeck(int deckIndex)
        {
            if (deckIndex >= Deck.Count)
                return false;

            Deck.RemoveAt(deckIndex);
            AccountManager.Instance.UpdateDataOnServer();//plaster!!!
            return true;
        }

        public Character()
        {

        }
    }

    [Serializable]
    public class DeckData
    {
        private const string DEFAULT_DECK_NAME = "New Deck";
        private const int NUMBER_OF_CARDS_IN_DECK = 8;
        private const int NUMBER_OF_COMBO_IN_DECK = 3;

        public int Id;
        public string Name;
        public CoreID[] Cards;
        public ComboCore[] Combos;

        public DeckData(int id, string name, CoreID[] cards, ComboCore[] combos)
        {
            Id = id;
            Name = name;
            Cards = cards;
            Combos = combos;
        }

        public DeckData(int id)
        {
            Id = id;
            Name = DEFAULT_DECK_NAME;
            Cards = new CoreID[NUMBER_OF_CARDS_IN_DECK];
            Combos = new ComboCore[NUMBER_OF_COMBO_IN_DECK];
        }
        public DeckData()
        {

        }

        public bool AddCoreId(CoreID coreID)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if (Cards[i] != null) continue;
                Cards[i] = coreID;
                return true;
            }

            return false;
        }

        public void RemoveCoreID(CoreID coreID)
        {
            for (int i = 0; i < Cards.Length; i++)
            {
                if (Cards[i].ID != coreID.ID) continue;
                Cards[i] = null;
                return;
            }
        }
    }
}
