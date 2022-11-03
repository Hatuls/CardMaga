using System.Collections.Generic;
using Account.GeneralData;

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
        
        private List<int> _availableSkins = new List<int>();
        private List<MetaDeckData> _decks = new List<MetaDeckData>();

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
        public int DeckLimit => _deckAmount;

        #endregion

        public MetaCharacterData(Character character)
        {
            
        }
    }
}