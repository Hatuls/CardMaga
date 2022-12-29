using System.Collections.Generic;
using System.Linq;
using Account;
using Account.GeneralData;

namespace CardMaga.MetaData.AccoutData
{
    public class MetaAccountData
    {
        #region Fields

        private AccountData _accountData;
        private MetaCharactersHandler _charactersHandler;
        private AccountLevelData _accountLevel;
        private AccountResources _accountResources;
        private List<CardInstance> _accountCards;
        private List<MetaComboData> _accountCombos;
        
        #endregion
        
        #region Prop

        public string AccountName => AccountManager.Instance.DisplayName;
        public MetaCharactersHandler CharacterDatas => _charactersHandler;
        public List<CardInstance> AccountCards => _accountCards;
        public List<MetaComboData> AccountCombos => _accountCombos;
        public AccountResources Resources => _accountResources;//need to re work
        public AccountLevelData AccountLevel => _accountLevel; // need to re work

        #endregion

        public MetaAccountData(AccountData accountData)
        {
            _accountCards = Factory.GameFactory.Instance.CardFactoryHandler.CreateCardInstances(accountData.AllCards.CardsIDs);
            _accountCombos =
                Factory.GameFactory.Instance.ComboFactoryHandler.GetMetaComboData(accountData.AllCombos.CombosIDs.ToArray());
            _accountData = accountData;
            _charactersHandler = new MetaCharactersHandler(_accountData.CharactersData.Characters,1);//need to re - done
            _accountResources = new AccountResources();//Need to have a way to add value
            //need to add _accountCard To add All the account cards
            //need to add accountLevel Support
            
            //Factory.GameFactory.Instance.CardFactoryHandler.GetMetaCardData()
        }
    }
}