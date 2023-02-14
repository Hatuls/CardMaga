using System;
using System.Collections.Generic;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using CardMaga.Rewards;
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

        public MetaAccountData MetaAccountData => _metaAccountData;

        public AccountDataCollectionHelper AccountDataCollection => _accountDataCollection;

        public void ExecuteTask(ITokenReceiver tokenMachine, MetaDataManager data)
        {
            _accountDataAccess = new AccountDataAccess();
            _accountDataCollection = new AccountDataCollectionHelper(_accountDataAccess.AccountData);

            _metaAccountData = _accountDataAccess.AccountData;

            PackRewardScreen.OnCardsGifted += GetGiftCards;
        }

        ~MetaAccountDataManager()
        {
            PackRewardScreen.OnCardsGifted -= GetGiftCards;
        }
        
        /// <summary>
        /// Permanently delete card instance from all user data 
        /// </summary>
        /// <param name="metaCardInstanceInfo"></param>
        // public void RemoveCard(MetaCardInstanceInfo metaCardInstanceInfo)
        // {
        //     _metaAccountData.AccountCards.Remove(metaCardInstanceInfo);
        //     
        //     foreach (var deckId in metaCardInstanceInfo.AssociateDeck)
        //     {
        //         var characters = _metaAccountData.CharacterDatas.CharacterDatas;
        //         
        //         foreach (var characterData in characters)
        //         {
        //             characterData.Decks[deckId].RemoveCard(metaCardInstanceInfo);
        //         }
        //     }
        //     
        //     _accountDataAccess.RemoveCard(metaCardInstanceInfo.GetCoreID());
        //
        //     _accountDataCollection.CollectionCardDatasHandler.TryRemoveCardInstance(metaCardInstanceInfo.InstanceID,true);
        // }
        //
        public void RemoveCard(MetaCardInstanceInfo cardInstance)
        {
            _metaAccountData.AccountCards.Remove(cardInstance);

            _accountDataAccess.RemoveCard(cardInstance.CardInstance.GetCoreId());

            _accountDataCollection.CollectionCardDatasHandler.TryRemoveCardInstance(cardInstance.InstanceID,true);
        }

        public void AddCard(MetaCardInstanceInfo cardInstance)
        {
            if (_metaAccountData.AccountCards.Contains(cardInstance))
            {
                //update
                
            }
            
            //add new
        }

        public void UpdateDeckAssociate(int deckID)
        {
            var deck = _metaAccountData.CharacterDatas.MainCharacterData.Decks[deckID];

            foreach (var instanceInfo in deck.Cards)
            {
                if (instanceInfo.IsInDeck(deckID))
                    continue;
                
                instanceInfo.AddToDeck(deckID);
            }
        }

        public void UpdateDeck(MetaDeckData metaDeckData,ITokenReceiver tokenReceiver)
        {
            _metaAccountData.CharacterDatas.MainCharacterData.UpdateDeck(metaDeckData,metaDeckData.DeckId);
            _accountDataCollection.UpdateCollection();
            _accountDataAccess.UpdateDeck(metaDeckData,tokenReceiver);
        }

        private void UpdateCard(CardInstance cardInstance)
        {
            // if (!_accountDataCollection.CollectionCardDatasHandler.TryGetCardInstanceInfo(x =>
            //         x.InstanceID == cardInstance.InstanceID, out MetaCardInstanceInfo[] metaCardInstanceInfo))
            // {
            //     throw new Exception($"MetaAccountDataManager: cant find cardInstance by id: {cardInstance.InstanceID}");
            // }
            //
            // foreach (var instance in _metaAccountData.AccountCards)
            // {
            //     if (instance.InstanceID == cardInstance.InstanceID)
            //     {
            //         if (instance.CoreID != cardInstance.CoreID)
            //         {
            //             instance.UpdateCoreId(cardInstance.GetCoreId());
            //         }
            //     }
            // }
            //
            // var cardInstanceInfo = metaCardInstanceInfo[0];
            //
            // foreach (var deckId in cardInstanceInfo.AssociateDeck)
            // {
            //     var characters = _metaAccountData.CharacterDatas.CharacterDatas;
            //     
            //     foreach (var characterData in characters)
            //     {
            //         characterData.Decks[deckId].RemoveCard(cardInstanceInfo.CardInstance);
            //     }
            // }
            
            //_accountDataAccess.UpgradeCard();
        }

        private void GetGiftCards(IEnumerable<CoreID> cards)
        {
            var cardFactory = Factory.GameFactory.Instance.CardFactoryHandler;
            
            foreach (var coreID in cards)
            {
                AddNewCard(cardFactory.CreateCardInstance(coreID));
            }
        }

        private void AddNewCard(CardInstance cardInstance)
        {
            var metaCardInstanceInfo = new MetaCardInstanceInfo(cardInstance);
            
            _metaAccountData.AccountCards.Add(metaCardInstanceInfo);
            _accountDataCollection.CollectionCardDatasHandler.AddCardInstance(metaCardInstanceInfo);
        }

    }
}