using System;
using System.Collections.Generic;
using System.Linq;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    [Serializable]
    public class AccountDataCollectionHelper : ISequenceOperation<MetaDataManager>
    {
        private AccountDataAccess _accountDataAccess;
        
        [SerializeField] private List<MetaCollectionCardData> _collectionCardDatas;
        [SerializeField] private List<MetaCollectionComboData> _collectionComboDatas;

        public List<MetaCollectionCardData> ALlCollectionCardDatas => _collectionCardDatas;
        public List<MetaCollectionComboData> AllCollectionComboDatas => _collectionComboDatas;
        
        public int Priority => 0;
        
        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _collectionCardDatas = new List<MetaCollectionCardData>();
            _collectionComboDatas = new List<MetaCollectionComboData>();
            
            _accountDataAccess = data.AccountDataAccess;
            _collectionCardDatas = InitializeCardData();
            _collectionComboDatas = InitializeComboData();
        }
        
        private List<MetaCollectionCardData> InitializeCardData()
        {
            List<CardInstance> temp = _accountDataAccess.AccountData.AccountCards;

            List<CardInstance> cardDatas = temp.OrderBy(x => x.CoreID).ToList();

            List<MetaCollectionCardData> output = new List<MetaCollectionCardData>();

            foreach (var cardData in cardDatas)
            {
                bool isAdded = false;
                
                foreach (var collectionCardData in output.Where(collectionCardData => collectionCardData.Equals(cardData)))
                {
                    collectionCardData.AddCardInstance(cardData);
                    isAdded = true;
                }

                if (isAdded)
                    continue;
                
                output.Add(new MetaCollectionCardData(cardData));
            }
            
            SortCardByDeck(output);

            return output;
        }

        private void SortCardByDeck(List<MetaCollectionCardData> metaCollectionCardDatas)
        {
            MetaCharacterData[] metaCharacterDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterDatas;

            foreach (var characterData in metaCharacterDatas)
            {
                foreach (var deckData in characterData.Decks)       
                {
                    foreach (var cardData in deckData.Cards)
                    {
                        foreach (var collectionCardData in metaCollectionCardDatas)
                        {
                            if (collectionCardData.FindCardInstance(cardData.InstanceID,out MetaCardInstanceInfo metaCardInstanceInfo))
                            {
                                metaCardInstanceInfo.AddToDeck(deckData.DeckId);
                            }
                        }
                    }
                }
            }
        }

        private void SortComboByDeck(List<MetaCollectionComboData> comboDatas)
        {
            MetaCharacterData[] metaCharacterDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterDatas;

            foreach (var characterData in metaCharacterDatas)
            {
                foreach (var deckData in characterData.Decks)       
                {
                    foreach (var comboData in deckData.Combos)
                    {
                        foreach (var collectionCardData in comboDatas)
                        {
                            if (collectionCardData.ComboID == comboData.ID)
                            {
                                collectionCardData.AddDeckReference(deckData.DeckId);
                            }
                        }
                    }
                }
            }
        }
        
        private List<MetaCollectionComboData> InitializeComboData()
        {
            List<ComboCore> comboCores = _accountDataAccess.AccountData.AccountCombos;
            List<MetaCollectionComboData> output = new List<MetaCollectionComboData>();
            
            output.AddRange(comboCores.Select(comboData => new MetaCollectionComboData(comboData)));
            
            SortComboByDeck(output);

            return output;
        }

        public List<MetaCollectionCardData> SetCardCollection(int deckId)
        {
            if (_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.IsNewDeck)
                return GetCollectionCardDatasCopy();

            List<CardInstance> cardDatas =
                _accountDataAccess.AccountData.CharacterDatas.CharacterData.GetDeckById(deckId).Cards;

            List<MetaCollectionCardData> collectionCardDatas = GetCollectionCardDatasCopy();

            foreach (var collectionCardData in _collectionCardDatas)
            {
                foreach (var cardInstanceInfo in collectionCardData.CardInstances)
                {
                    foreach (var cardData in cardDatas)
                    {
                        if (cardInstanceInfo.InstanceID == cardData.InstanceID)
                        {
                            collectionCardDatas.RemoveCardInstance(cardInstanceInfo.InstanceID);
                        }
                    }   
                }
            }

            return collectionCardDatas;
        }
        
        public List<MetaCollectionComboData> SetComboCollection(int deckId)
        {
            if (_accountDataAccess.AccountData.CharacterDatas.CharacterData.MainDeck.IsNewDeck)
                return GetCollectionComboDatasCopy();

            List<ComboCore> metaComboDatas =
                _accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[deckId].Combos;

            List<MetaCollectionComboData> collectionComboDatasCopy = GetCollectionComboDatasCopy();

            foreach (var collectionDataCombo in collectionComboDatasCopy)
            {
                foreach (var comboData in metaComboDatas)
                {
                    if (collectionDataCombo.ComboID == comboData.ID)
                    {
                        collectionDataCombo.RemoveComboFromCollection();
                    }
                }
            }

            return collectionComboDatasCopy;
        }
        
        private List<MetaCollectionCardData> GetCollectionCardDatasCopy()
        {
            return InitializeCardData();
        }
        
        private List<MetaCollectionComboData> GetCollectionComboDatasCopy()
        {
            return InitializeComboData();
        }

    }
}