using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
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
        private List<MetaCollectionCardData> currentCurrentCollectionCardDatas;
        
        private List<MetaCollectionComboData> _collectionComboDatas;
        private List<MetaCollectionComboData> currentCurrentCollectionComboDatas;

        public List<MetaCollectionCardData> ALlCollectionCardDatas => _collectionCardDatas;

        public List<MetaCollectionComboData> AllCollectionComboDatas => _collectionComboDatas;

        public List<MetaCollectionCardData> CurrentCollectionCardDatas => currentCurrentCollectionCardDatas;

        public List<MetaCollectionComboData> CurrentCollectionComboDatas => currentCurrentCollectionComboDatas;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _collectionCardDatas = new List<MetaCollectionCardData>();
            _collectionComboDatas = new List<MetaCollectionComboData>();
            
            _accountDataAccess = data.AccountDataAccess;
            InitializeCardData();
            InitializeComboData();
        }

        public int Priority => 0;
        
        private void InitializeCardData()
        {
            List<CardInstance> temp = _accountDataAccess.AccountData.AccountCards;

            List<CardInstance> cardDatas = temp.OrderBy(x => x.CoreID).ToList();

            _collectionCardDatas = new List<MetaCollectionCardData>();

            foreach (var cardData in cardDatas)
            {
                foreach (var collectionCardData in _collectionCardDatas.Where(collectionCardData => collectionCardData.Equals(cardData)))
                {
                    collectionCardData.AddCardInstance(cardData);
                }

                _collectionCardDatas.Add(new MetaCollectionCardData(cardData));
            }

            MetaCharacterData[] metaCharacterDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterDatas;
            
            
        }
        
        private void InitializeComboData()
        {
         //   metaCollectionComboDatas.AddRange(comboDatas.Select(comboData => new MetaCollectionComboData(1, 1, comboData)));
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
                 //       collectionCardData.AddItemToCollectionSuccess(cardData);
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
                   //     collectionDataCombo.AddItemToCollectionSuccess(comboData);
                    }
                }
            }

            return collectionComboDatasCopy;
        }
        
        public void UpdateCollection()
        {
            currentCurrentCollectionCardDatas = SetCardCollection();
            currentCurrentCollectionComboDatas = SetComboCollection();
        }

        private List<MetaCollectionCardData> GetCollectionCardDatasCopy()
        {
            List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();

            foreach (var cardData in _collectionCardDatas)
            {
               // output.Add(new MetaCollectionCardData(cardData.NumberOfInstance,cardData.NumberOfInstance,cardData.ItemReference));
            }

            return output;
        }
        
        private List<MetaCollectionComboData> GetCollectionComboDatasCopy()
        {
            List<MetaCollectionComboData> output = new List<MetaCollectionComboData>();

            foreach (var data in _collectionComboDatas)
            {
              //  output.Add(new MetaCollectionComboData(data.NumberOfInstance,data.NumberOfInstance,data.ItemReference));
            }

            return output;
        }

    }
}