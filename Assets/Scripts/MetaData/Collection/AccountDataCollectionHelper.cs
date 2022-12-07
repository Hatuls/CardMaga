using System.Collections.Generic;
using System.Linq;
using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.Collection
{
    public class AccountDataCollectionHelper : ISequenceOperation<MetaDataManager>
    {
        private AccountDataAccess _accountDataAccess;
        
        private List<MetaCollectionCardData> _collectionCardDatas;
        private List<MetaCollectionCardData> _currentCollectionCardDatas;
        private List<MetaCollectionDataCombo> _collectionComboDatas;

        public List<MetaCollectionCardData> CollectionCardDatas => _currentCollectionCardDatas;

        public List<MetaCollectionDataCombo> CollectionComboDatas => _collectionComboDatas;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _collectionCardDatas = new List<MetaCollectionCardData>();
            _collectionComboDatas = new List<MetaCollectionDataCombo>();
            
            _accountDataAccess = data.AccountDataAccess;
            InitializeData(_accountDataAccess.AccountData.AccountCards,_collectionCardDatas);
            InitializeData(_accountDataAccess.AccountData.AccountCombos,_collectionComboDatas);
        }

        public int Priority => 0;
        
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
        
        private void InitializeData(List<MetaComboData> comboDatas,List<MetaCollectionDataCombo> metaCollectionComboDatas)
        {
            foreach (var comboData in comboDatas)
            {
                metaCollectionComboDatas.Add(new MetaCollectionDataCombo(1, comboData));
            }
        }

        private List<MetaCollectionCardData> SetCollection()
        {
            if (_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.IsNewDeck)
                return GetCollectionCardDatasCopy();

            List<MetaCardData> metaCardDatas =
                _accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.Cards;

            List<MetaCollectionCardData> collectionCardDatas = GetCollectionCardDatasCopy();

            foreach (var collectionCardData in collectionCardDatas)
            {
                foreach (var cardData in metaCardDatas)
                {
                    if (collectionCardData.Equals(cardData))
                    {
                        collectionCardData.AddItemToCollection(cardData);
                    }
                }
            }

            return collectionCardDatas;
        }
        
        public void UpdateCollection()
        {
            _currentCollectionCardDatas = SetCollection();
        }

        private List<MetaCollectionCardData> GetCollectionCardDatasCopy()
        {
            List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();

            foreach (var cardData in _collectionCardDatas)
            {
                output.Add(new MetaCollectionCardData(cardData.NumberOfInstant,cardData.ItemReference));
            }

            return output;
        }

    }
}