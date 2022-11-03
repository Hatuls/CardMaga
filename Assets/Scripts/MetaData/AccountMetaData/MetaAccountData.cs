using Account;
using Account.GeneralData;

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

        public MetaCharactersHandler CharacterDatas => _charactersHandler;

        public MetaCardData[] AccountCards => _accountCards;

        public AccountResources Resources => _accountResources;

        public AccountLevelData AccountLevel => _accountLevel;

        #endregion

        public MetaAccountData()
        {
            
        }
    }
}