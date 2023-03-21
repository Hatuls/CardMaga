using System;
using System.Collections.Generic;
using Account;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaData.AccoutData
{
    [Serializable]
    public class MetaAccountData
    {
        #region Fields

        private AccountData _accountData;
        [SerializeField] private MetaCharactersHandler _charactersHandler;
        [SerializeField] private AccountLevelData _accountLevel;
        [SerializeField] private AccountResources _accountResources;
        [SerializeField] private List<MetaCardInstanceInfo> _accountCards;
        [SerializeField] private List<MetaComboInstanceInfo> _accountCombos;
        
        #endregion
        
        #region Prop

        public string AccountName => AccountManager.Instance.DisplayName;
        public MetaCharactersHandler CharacterDatas => _charactersHandler;
        public List<MetaCardInstanceInfo> AccountCards => _accountCards;
        public List<MetaComboInstanceInfo> AccountCombos => _accountCombos;
        public AccountResources Resources => _accountResources;//need to re work
        public AccountLevelData AccountLevel => _accountLevel; // need to re work

        #endregion

        public MetaAccountData(AccountData accountData)
        {
            var cardInstances = Factory.GameFactory.Instance.CardFactoryHandler.CreateCardInstances(accountData.AllCards.CardsIDs);
            _accountCards = new List<MetaCardInstanceInfo>(cardInstances.Count);
            
            foreach (var instance in cardInstances)
            {
                _accountCards.Add(new MetaCardInstanceInfo(instance));
            }
            
            var comboInstances = Factory.GameFactory.Instance.ComboFactoryHandler.GetMetaComboInstance(accountData.AllCombos.CombosIDs);
            _accountCombos = new List<MetaComboInstanceInfo>(comboInstances.Count);
            
            foreach (var instance in comboInstances)
            {
                _accountCombos.Add(new MetaComboInstanceInfo(instance));
            }
            
            _accountData = accountData;
            _charactersHandler = new MetaCharactersHandler(_accountData.CharactersData.Characters,_accountCards,_accountCombos,_accountData.CharactersData.MainCharacterID);//need to re - done
            _accountResources = new AccountResources();//Need to have a way to add value
            //need to add _accountCard To add All the account cards
            //need to add accountLevel Support
            
            //Factory.GameFactory.Instance.CardFactoryHandler.GetMetaCardData()
        }
    }
}