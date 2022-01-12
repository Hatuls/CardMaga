﻿using System.Threading.Tasks;
using UnityEngine;
namespace Account.GeneralData
{
    [System.Serializable]
    public class AccountGeneralData : ILoadFirstTime
    {
        [SerializeField] bool _isFirstTime = false;
        public AccountGeneralData()
        {

        }

        #region Fields
        [SerializeField]
        private AccountInfoData _accountInfoData;
        [SerializeField]
        private AccountLevelData _accountLevelData;
        [SerializeField]
        private AccountResourcesData _accountResourcesData;
        [SerializeField]
        private AccountEnergyData _accountEnergyData;
        #endregion


        #region Properties
        public AccountInfoData AccountInfoData { get => _accountInfoData; private set => _accountInfoData = value; }
        public AccountLevelData AccountLevelData { get => _accountLevelData; private set => _accountLevelData = value; }
        public AccountResourcesData AccountResourcesData { get => _accountResourcesData; private set => _accountResourcesData = value; }
        public AccountEnergyData AccountEnergyData { get => _accountEnergyData; private set => _accountEnergyData = value; }
        public bool IsFirstTime { get => _isFirstTime; set => _isFirstTime = value; }


        public async Task NewLoad()
        {
            //_accountInfoData = new AccountInfoData(TimeManager.Instance.GetCurrentTime(),);
      
            IsFirstTime = false;


            _accountEnergyData = new AccountEnergyData();
           await _accountEnergyData.NewLoad();
            _accountLevelData = new AccountLevelData();
            await _accountLevelData.NewLoad();
            _accountResourcesData = new AccountResourcesData();
            await _accountResourcesData.NewLoad();
        }

        public bool IsCorrupted()
        {
            bool corrupted = false;
            //corrupted |= _accountEnergyData.IsCorrupted();
            //corrupted |= _accountLevelData.IsCorrupted();
            return corrupted;
        }
        #endregion
    }
}