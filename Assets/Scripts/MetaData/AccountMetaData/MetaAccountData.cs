using Account;
using Account.GeneralData;
using UnityEditor.Graphs;

namespace CardMaga.Meta.AccountMetaData
{
    public class MetaAccountData
    {
        #region Fields

        private AccountData _accountData;
        private MetaCharactersHandler _charactersHandler;
        private AccountLevelData _accountLevel;
        private AccountResources _accountResources;
        private MetaCardData[] _accountCards;
        
        #endregion
        
        #region Prop

        public string AccountName => _accountData.DisplayName;
        
        public MetaCharactersHandler CharacterDatas => _charactersHandler;

        public MetaCardData[] AccountCards => _accountCards;

        public AccountResources Resources => _accountResources;

        public AccountLevelData AccountLevel => _accountLevel;

        #endregion

        public MetaAccountData(AccountData accountData)
        {
            _accountData = accountData;
            _charactersHandler = new MetaCharactersHandler(_accountData.CharactersData.Characters);
            _accountResources = new AccountResources();//Need to have a way to add value
            //need to add _accountCard To add All the account cards
            //need to add accountLevel Support
        }
    }
}