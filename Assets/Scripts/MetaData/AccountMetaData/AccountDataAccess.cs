using System;
using Account;
using CardMaga.SequenceOperation;
using CardMaga.Server.Request;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;
using Account.GeneralData;

namespace CardMaga.MetaData.AccoutData
{
    [Serializable]
    public class AccountDataAccess : ISequenceOperation<MetaDataManager>
    {
        [NonSerialized]
        private AccountData _accountData;

        [SerializeField] private MetaAccountData _metaAccountData;

        public MetaAccountData AccountData => _metaAccountData;

        public void RemoveCard(CoreID coreId)
        {
            _accountData.AllCards.RemoveCard(coreId);

            var characters = _accountData.CharactersData.Characters;
            
            foreach (var character in characters)
            {
                foreach (var deckData in character.Deck)
                {
                    deckData.RemoveCoreID(coreId);
                    break;
                }
            }
        }

        public void AddCard(CardInstance cardInstance)
        {
            _accountData.AllCards.AddCard(cardInstance.GetCoreId());
        }

        public void UpgradeCard(CoreID oldCoreId,CoreID newCoreId)
        {
            _accountData.AllCards.RemoveCard(oldCoreId);
            _accountData.AllCards.AddCard(newCoreId);
        }
        
        public void UpdateDeck(MetaDeckData metaDeckData,ITokenReciever tokenMachine)
        {
            _metaAccountData.CharacterDatas.CharacterData.UpdateDeck(metaDeckData,metaDeckData.DeckIndex);
            BaseServerRequest serverRequest;

            if (metaDeckData.IsNewDeck)
                serverRequest = new AddDeckServerRequest(metaDeckData,_metaAccountData.CharacterDatas.CharacterData.Id);
            else
                serverRequest = new UpdateDeckDataRequest(metaDeckData, _metaAccountData.CharacterDatas.CharacterData.Id);//need to get a characterid
            
            serverRequest.SendRequest(tokenMachine);//need to add character support
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _accountData = AccountManager.Instance.Data;
            _metaAccountData = new MetaAccountData(AccountManager.Instance.Data);
       }

        public MetaAccountData GetMetaAccountData()
        {
            return new MetaAccountData(AccountManager.Instance.Data);//plaster!!!!! need to not by mono and get the data from AccountDataAccess
        }

        public int Priority => 0;
    }
}