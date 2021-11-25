﻿using UnityEngine;
using Factory;
using Battles;
using Account.GeneralData;
using Account;
using System;



namespace UI.Meta.PlayScreen
{
    public class ChooseCharacterScreen : MonoBehaviour
    {
        #region Singleton
        private static ChooseCharacterScreen _instance;
        public static ChooseCharacterScreen GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("ChooseCharacterScreen is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion

        #region Fields
        [SerializeField]
        CharacterDataUI[] _characters;
        [SerializeField]
        GameObject _chooseCharacterPanel;
        #endregion
        #region Public Methods
        public void Init()
        {
            Debug.Log("InitingCharacterPanel");
            ResetCharacterScreen();
            ChooseCharacterSetActiveState(true);
            InitCharactersData();
        }
        public void ChooseCharacterSetActiveState(bool toState)
        {
            _chooseCharacterPanel.SetActive(toState);
        }
        #endregion
        public void ResetCharacterScreen()
        {
            Debug.Log("ResetingCharacterScreen");
            for (int i = 0; i < _characters.Length; i++)
            {
                _characters[i].gameObject.SetActive(false);
            }
            ChooseCharacterSetActiveState(false);
        }
        private void InitCharactersData()
        {
            ushort playerLevel = AccountManager.Instance.AccountGeneralData.AccountLevelData.Level.Value;
            if (playerLevel <= 0)
            {
                throw new Exception("ChooseCharacterScreen playerLevel is not logical");
            }

            CharacterSO[] characters = GameFactory.Instance.CharacterFactoryHandler.GetCharactersSO(CharacterTypeEnum.Player);
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterData characterData = new CharacterData(characters[i].CharacterEnum);
                _characters[i].Init(characterData, playerLevel, characters[i]);
            }
        }
    }
}
