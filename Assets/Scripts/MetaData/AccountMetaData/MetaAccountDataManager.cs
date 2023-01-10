using System;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using CardMaga.SequenceOperation;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    [Serializable]
    public class MetaAccountDataManager : ISequenceOperation<MetaDataManager> //Managing to the account meta data
    {
        [SerializeField] private MetaAccountData _metaAccountData;

        private AccountDataCollectionHelper _accountDataCollection; //Managing to the account data collection
        private AccountDataAccess _accountDataAccess;//Managing to the server's Account data

        public int Priority => 0;

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = data.AccountDataAccess;
            _accountDataCollection = data.AccountDataCollectionHelper;

            _metaAccountData = _accountDataAccess.GetMetaAccountData();
        }
        
        /// <summary>
        /// Permanently delete card instance from all user data 
        /// </summary>
        /// <param name="metaCardInstanceInfo"></param>
        [Obsolete]
        public void RemoveCard(MetaCardInstanceInfo metaCardInstanceInfo)
        {
            _metaAccountData.AccountCards.Remove(metaCardInstanceInfo.CardInstance);
            
            foreach (var deckId in metaCardInstanceInfo.AssociateDeck)
            {
                var characters = _metaAccountData.CharacterDatas.CharacterDatas;
                
                foreach (var characterData in characters)
                {
                    characterData.Decks[deckId].RemoveCard(metaCardInstanceInfo.CardInstance);
                }
            }
            
            _accountDataAccess.RemoveCard(metaCardInstanceInfo.GetCoreID());

            _accountDataCollection.CollectionCardDatasHandler.TryRemoveCardInstance(metaCardInstanceInfo.InstanceID);
        }

        public void AddCard(CardInstance cardInstance)
        {
            if (_metaAccountData.AccountCards.Contains(cardInstance))
            {
                //update
                
            }
            
            //add new
        }

        private void UpdateCard(CardInstance cardInstance)
        {
            if (!_accountDataCollection.CollectionCardDatasHandler.TryGetCardInstanceInfo(x =>
                    x.InstanceID == cardInstance.InstanceID, out MetaCardInstanceInfo[] metaCardInstanceInfo))
            {
                throw new Exception($"MetaAccountDataManager: cant find cardInstance by id: {cardInstance.InstanceID}");
            }

            foreach (var instance in _metaAccountData.AccountCards)
            {
                if (instance.InstanceID == cardInstance.InstanceID)
                {
                    if (instance.CoreID != cardInstance.CoreID)
                    {
                        instance.UpdateCoreId(cardInstance.GetCoreId());
                    }
                }
            }

            var cardInstanceInfo = metaCardInstanceInfo[0];
            
            foreach (var deckId in cardInstanceInfo.AssociateDeck)
            {
                var characters = _metaAccountData.CharacterDatas.CharacterDatas;
                
                foreach (var characterData in characters)
                {
                    characterData.Decks[deckId].RemoveCard(cardInstanceInfo.CardInstance);
                }
            }
            
            //_accountDataAccess.UpgradeCard();
        }

        private void AddNewCard(CardInstance cardInstance)
        {
            _metaAccountData.AccountCards.Add(cardInstance);
            _accountDataCollection.CollectionCardDatasHandler.AddCardInstance(new MetaCardInstanceInfo(cardInstance));
            _accountDataAccess.AddCard(cardInstance);
        }

    }
}