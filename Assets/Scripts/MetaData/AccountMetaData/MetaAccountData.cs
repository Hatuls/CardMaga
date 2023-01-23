using System;
using System.Collections.Generic;
using Account;
using Account.GeneralData;
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
        [SerializeField] private List<CardInstance> _accountCards;
        [SerializeField] private List<ComboInstance> _accountCombos;
        
        #endregion
        
        #region Prop

        public string AccountName => AccountManager.Instance.DisplayName;
        public MetaCharactersHandler CharacterDatas => _charactersHandler;
        public List<CardInstance> AccountCards => _accountCards;
        public List<ComboInstance> AccountCombos => _accountCombos;
        public AccountResources Resources => _accountResources;//need to re work
        public AccountLevelData AccountLevel => _accountLevel; // need to re work

        #endregion

        public MetaAccountData(AccountData accountData)
        {
            _accountCards = Factory.GameFactory.Instance.CardFactoryHandler.CreateCardInstances(accountData.AllCards.CardsIDs);
            _accountCombos = Factory.GameFactory.Instance.ComboFactoryHandler.GetMetaComboInstance(accountData.AllCombos.CombosIDs);
            _accountData = accountData;
            _charactersHandler = new MetaCharactersHandler(_accountData.CharactersData.Characters,AccountCards,_accountCombos,_accountData.CharactersData.MainCharacterID);//need to re - done
            _accountResources = new AccountResources();//Need to have a way to add value
            //need to add _accountCard To add All the account cards
            //need to add accountLevel Support
            
            //Factory.GameFactory.Instance.CardFactoryHandler.GetMetaCardData()
        }
    }
}