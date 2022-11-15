using System.Collections.Generic;
using System.Linq;
using CardMaga.MetaData.AccoutData;

namespace CardMaga.MetaData.Collection
{
    public class AccountDataCollectionHelper
    {
        private AccountDataAccess _accountDataAccess;
        
        private List<MetaCollectionCardData> _collectionCardDatas;
        private List<MetaCollectionCardData> _deckCardDatas;

        public List<MetaCollectionCardData> DeckCardDatas => _deckCardDatas;

        public List<MetaCollectionCardData> CollectionCardDatas => _collectionCardDatas;

        public List<MetaCardData> DeckData => _accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards;

        public List<MetaComboData> MetaComboDatas =>
            _accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Combos;

        public AccountDataCollectionHelper(AccountDataAccess accountDataAccess)
        {
            _collectionCardDatas = new List<MetaCollectionCardData>();
            _deckCardDatas = new List<MetaCollectionCardData>();
            _accountDataAccess = accountDataAccess;
            InitializeData(_accountDataAccess.AccountData.AccountCards,_collectionCardDatas);
            InitializeData(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards,_deckCardDatas);
            SetCollection();
        }
        
        private void InitializeData(List<MetaCardData> cardDatas,List<MetaCollectionCardData> metaCollectionCardDatas)
        {
            var result = from cardData in cardDatas
                group cardData by cardData.CardInstance.ID
                into r
                select new { cardID = r.Key, count = r.Count(), metaCard = cardDatas.First(x=> r.Key == x.CardInstance.ID)};
            
            foreach (var x in result)
            {
                metaCollectionCardDatas.Add(new MetaCollectionCardData(x.count,x.metaCard));
            }
        }

        private void SetCollection()
        {
            foreach (var cardData in _collectionCardDatas)
            {
                if (_deckCardDatas.Contains(cardData))
                {
                   // cardData.NumberOfInstant -= 1;
                }
            }
        }
    }
}