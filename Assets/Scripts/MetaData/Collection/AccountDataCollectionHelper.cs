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
        
        private List<MetaCollectionComboData> _collectionComboDatas;
        private List<MetaCollectionComboData> _currentCollectionComboDatas;

        public List<MetaCollectionCardData> CollectionCardDatas => _currentCollectionCardDatas;

        public List<MetaCollectionComboData> CollectionComboDatas => _currentCollectionComboDatas;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _collectionCardDatas = new List<MetaCollectionCardData>();
            _collectionComboDatas = new List<MetaCollectionComboData>();
            
            _accountDataAccess = data.AccountDataAccess;
            InitializeCardData(_accountDataAccess.AccountData.AccountCards,_collectionCardDatas);
            InitializeComboData(_accountDataAccess.AccountData.AccountCombos,_collectionComboDatas);
        }

        public int Priority => 0;
        
        private void InitializeCardData(List<MetaCardData> cardDatas,List<MetaCollectionCardData> metaCollectionCardDatas)
        {
            var result = from cardData in cardDatas
                group cardData by cardData.CardInstance.ID
                into r
                select new { cardID = r.Key, count = r.Count(), metaCard = cardDatas.First(x=> r.Key == x.CardInstance.ID)};
            
            foreach (var x in result)
            {
                metaCollectionCardDatas.Add(new MetaCollectionCardData(x.count,x.count,x.metaCard));
            }

            MetaCharacterData[] metaCharacterDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterDatas;

            Dictionary<int, int> cache = new Dictionary<int, int>();

            foreach (var characterData in metaCharacterDatas)
            {
                foreach (var deckData in characterData.Decks)
                {
                    foreach (var collectionCardData in deckData.Cards.SelectMany(cardData => metaCollectionCardDatas.Where(collectionCardData => collectionCardData.CardId == cardData.CardInstance.ID)))
                    {
                        collectionCardData.AddItemToAssociateDeck(deckData.DeckId);
                    }
                }
            }


           
        }
        
        private void InitializeComboData(List<MetaComboData> comboDatas,List<MetaCollectionComboData> metaCollectionComboDatas)
        {
            metaCollectionComboDatas.AddRange(comboDatas.Select(comboData => new MetaCollectionComboData(1, 1, comboData)));
        }

        private List<MetaCollectionCardData> SetCardCollection()
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
        
        private List<MetaCollectionComboData> SetComboCollection()
        {
            if (_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.IsNewDeck)
                return GetCollectionComboDatasCopy();

            List<MetaComboData> metaComboDatas =
                _accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.Combos;

            List<MetaCollectionComboData> collectionComboDatasCopy = GetCollectionComboDatasCopy();

            foreach (var collectionDataCombo in collectionComboDatasCopy)
            {
                foreach (var comboData in metaComboDatas)
                {
                    if (collectionDataCombo.Equals(comboData))
                    {
                        collectionDataCombo.AddItemToCollection(comboData);
                    }
                }
            }

            return collectionComboDatasCopy;
        }
        
        public void UpdateCollection()
        {
            _currentCollectionCardDatas = SetCardCollection();
            _currentCollectionComboDatas = SetComboCollection();
        }

        private List<MetaCollectionCardData> GetCollectionCardDatasCopy()
        {
            List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();

            foreach (var cardData in _collectionCardDatas)
            {
                output.Add(new MetaCollectionCardData(cardData.NumberOfInstant,cardData.NumberOfInstant,cardData.ItemReference));
            }

            return output;
        }
        
        private List<MetaCollectionComboData> GetCollectionComboDatasCopy()
        {
            List<MetaCollectionComboData> output = new List<MetaCollectionComboData>();

            foreach (var data in _collectionComboDatas)
            {
                output.Add(new MetaCollectionComboData(data.NumberOfInstant,data.NumberOfInstant,data.ItemReference));
            }

            return output;
        }

    }
}