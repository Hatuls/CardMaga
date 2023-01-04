using System;
using Account;
using CardMaga.SequenceOperation;
using CardMaga.Server.Request;
using MetaData;
using ReiTools.TokenMachine;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    [Serializable]
    public class AccountDataAccess : ISequenceOperation<MetaDataManager>
    {
        [NonSerialized]
        private AccountData _accountData;

        [SerializeField] private MetaAccountData _metaAccountData;

        public MetaAccountData AccountData => _metaAccountData;
        
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
            _metaAccountData = new MetaAccountData(AccountManager.Instance.Data);//plaster!!!!! need to not by mono and get the data from AccountDataAccess
        }

        public int Priority => 0;
    }
}