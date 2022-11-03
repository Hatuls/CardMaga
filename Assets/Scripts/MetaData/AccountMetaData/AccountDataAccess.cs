using Account;

namespace CardMaga.Meta.AccountMetaData
{
    public class AccountDataAccess
    {
        private AccountData _accountData;

        private MetaAccountData _metaAccountData;
        
        public AccountDataAccess(AccountData accountData)
        {
            _metaAccountData = new MetaAccountData();
        }
    }
}