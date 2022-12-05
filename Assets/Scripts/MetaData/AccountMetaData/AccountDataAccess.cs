using System;
using Account;
using CardMaga.SequenceOperation;
using CardMaga.Server.Request;
using MetaData;
using ReiTools.TokenMachine;

namespace CardMaga.MetaData.AccoutData
{
    public class AccountDataAccess : ISequenceOperation<MetaDataManager>
    {
        [NonSerialized]
        private AccountData _accountData;

        private MetaAccountData _metaAccountData;

        public MetaAccountData AccountData => _metaAccountData;
        
        public void UpdateDeck(MetaDeckData metaDeckData,ITokenReciever tokenMachine)
        {
            _metaAccountData.CharacterDatas.CharacterData.UpdateDeck(metaDeckData,0);
            UpdateDeckDataRequest deckDataRequest = new UpdateDeckDataRequest(metaDeckData, 0);//need to get a characrerid
            deckDataRequest.SendRequest(tokenMachine);//need to add charecter support
        }

        public void ExecuteTask(ITokenReciever tokenMachine, MetaDataManager data)
        {
            _metaAccountData = new MetaAccountData(AccountManager.Instance.Data);//plaster!!!!! need to not by mono and get the data from AccountDataAccess
        }

        public int Priority => 0;
    }
}