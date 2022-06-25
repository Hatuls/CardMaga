using Battles;
using System;
using System.Collections.Generic;
namespace Account.GeneralData
{
    [Serializable]
    public class CharacterData
    {
        private int _id;
        private int _currentSkin;
        private int _exp;
        private int _skillPoint;
        private int _rank;
        private int _deckAmount = 1;

        private List<int> _availableSkins = new List<int>();
        private List<DeckData> _deck = new List<DeckData>();

        public CharacterData(CharacterSO newCharacter)
        {
            _id = newCharacter.ID;
            _currentSkin = 0;
            _exp = 0;
            _skillPoint = 0;
            _rank = 0;
            _deckAmount = 1;
            _deck.Add(new DeckData(_deck.Count, "Default Deck", newCharacter.Deck, newCharacter.Combos));
        }

        public int Id { get => _id; private set => _id = value; }
        public IReadOnlyList<int> AvailableSkins { get => _availableSkins; }
        public int CurrentSkin { get => _currentSkin; set => _currentSkin = value; }
        public int Exp { get => _exp; set => _exp = value; }
        public int SkillPoint { get => _skillPoint; set => _skillPoint = value; }
        public IReadOnlyList<DeckData> Deck { get => _deck; }
        public int Rank { get => _rank; set => _rank = value; }
        public int DeckLimit => _deckAmount;


        public bool AddNewDeck(CardInstanceID[] deckCards,Combo.Combo[] deckCombos)
        {
            CardInstanceID.CardCore[] cards = new CardInstanceID.CardCore[deckCards.Length];

            for (int i = 0; i < cards.Length; i++)
                cards[i] = deckCards[i].ToCardCore();

            return AddNewDeck(cards,deckCombos);
        }      
        public bool AddNewDeck(CardInstanceID.CardCore[] deckCards,Combo.Combo[] deckCombos)
        {
            bool _canAddDeck = _deck.Count < _deckAmount;
            if (_canAddDeck)
            {
                _deck.Add(new DeckData(_deck.Count, "New Deck", deckCards, deckCombos));
            }
            return _canAddDeck;
        }
        


        //  #region Field
        //  [SerializeField]
        //  CharacterEnum _characterEnum;
        //  [SerializeField]
        //  CharacterStats _stats;
        //  [SerializeField]
        //  AccountDeck[] _decks;
        //  [SerializeField]
        //  CombosAccountInfo[] _characterCombos;
        //  [SerializeField]
        //  byte _unlockAtLevel;

        //  #endregion

        //  #region Properties
        //  public CharacterEnum CharacterEnum => _characterEnum;
        //  public CharacterStats Stats => _stats;
        //  public AccountDeck[] Decks => _decks;
        //  public CombosAccountInfo[] CharacterCombos => _characterCombos;
        //  public byte UnlockAtLevel => _unlockAtLevel;
        //  #endregion

        //  #region PrivateMethods

        //  void AssignDeck(CardInstanceID[] cardAccountInfos, byte deckAmount)
        //  {

        //      _decks = new AccountDeck[deckAmount];
        //      CardInstanceID[] tempCards = new CardInstanceID[cardAccountInfos.Length];
        //      for (int i = 0; i < cardAccountInfos.Length; i++)
        //      {
        //          tempCards[i] = cardAccountInfos[i];
        //      }


        //      for (int i = 0; i < deckAmount; i++)
        //      {
        //          Decks[i] = new AccountDeck(tempCards);
        //          Decks[i].DeckName = $"Basic Deck {i}";
        //      }
        //  }
        //  void AssignCombos(Battles.CharacterSO characterSO)
        //  {
        //      CombosAccountInfo[] tempCombos = new CombosAccountInfo[characterSO.Combos.Length];
        //      for (int i = 0; i < characterSO.Combos.Length; i++)
        //      {
        //          tempCombos[i] = new CombosAccountInfo(characterSO.Combos[i].ComboRecipe.ID, characterSO.Combos[i].Level);
        //      }

        //      _characterCombos = tempCombos;
        //  }
        //  #endregion

        //  #region PublicMethods
        //  public CharacterData(CardInstanceID[] startingDeck ,CharacterEnum characterEnum, byte deckAmount = 4)
        //  {
        //      if (characterEnum == CharacterEnum.Enemy)
        //      {
        //          throw new Exception("CharacterData inserted an enemy instead of a player character");
        //      }
        //      var characterSO = Factory.GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(characterEnum);
        //      _characterEnum = characterEnum;
        //      _stats = characterSO.CharacterStats;
        //      _unlockAtLevel = characterSO.UnlockAtLevel;
        //      AssignDeck(startingDeck, deckAmount);
        //      AssignCombos(characterSO);

        //      AccountCards.OnUpgrade += CardUpgraded;
        //  }

        //  public CharacterData()
        //  {
        //      AccountCards.OnUpgrade += CardUpgraded;

        //  }
        //  ~CharacterData()
        //  {
        //      AccountCards.OnUpgrade -= CardUpgraded;
        //  }
        //  public AccountDeck GetDeckAt(int index)
        //=> _decks[index];
        //  public void CharacterAccount(CharacterEnum character, CharacterStats stats, AccountDeck[] decks,
        //      CombosAccountInfo[] combos, byte unlocksAtLevel)
        //  {

        //  }

        //  public void CardUpgraded(CardInstanceID cardCoreInfo)
        //  {
        //      for (int i = 0; i < _decks.Length; i++)
        //      {
        //          var cards = _decks[i].Cards;
        //          for (int j = 0; j < cards.Length; j++)
        //          {
        //              if (cards[j].InstanceID == cardCoreInfo.InstanceID)
        //              {
        //                  cards[j] = cardCoreInfo;
        //                  return;
        //              }
        //          }
        //      }
        //  }
        //  #endregion
    }

  
    public class DeckData
    {
        private int _id;
        private string _name;
        private CardInstanceID.CardCore[] _cards;
        private Combo.Combo[] _combos;

        public DeckData(int id, string name, CardInstanceID.CardCore[] cards, Combo.Combo[] combos)
        {
            _id = id;
            _name = name;
            _cards = cards;
            _combos = combos;
        }

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public CardInstanceID.CardCore[] Cards { get => _cards; set => _cards = value; }
        public Combo.Combo[] Combos { get => _combos; set => _combos = value; }
    }
}
