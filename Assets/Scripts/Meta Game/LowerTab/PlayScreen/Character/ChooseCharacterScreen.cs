using UnityEngine;
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
        CharacterDataUI[] _characters;
        [SerializeField]
        GameObject _chooseCharacterPanel;
        #endregion
        #region Public Methods
        public void Init()
        {
            byte playerLevel = AccountManager.Instance.AccountGeneralData.AccountLevelData.Level.Value;
            if (playerLevel <= 0)
            {
                throw new Exception("ChooseCharacterScreen playerLevel is not logical");
            }

            CharacterSO[] characters = GameFactory.Instance.CharacterFactoryHandler.GetCharactersSO(CharacterTypeEnum.Player);
            for (int i = 0; i < characters.Length; i++)
            {
                CharacterData characterData = new CharacterData(characters[i].CharacterEnum);
                _characters[i].Init(characterData,playerLevel,characters[i]);
            }
        }
        //public void ChooseCharacterSwitch()
        //{
        //    if(_chooseCharacterPanel.activeSelf == true)
        //    {
        //        _chooseCharacterPanel.gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        _chooseCharacterPanel.gameObject.SetActive(true);
        //    }
        //}
        public void ChooseCharacterSetActiveState(bool toState)
        {
            _chooseCharacterPanel.SetActive(toState);
        }

        public void ResetCharacterScreen()
        {

        }
        #endregion
    }
}
