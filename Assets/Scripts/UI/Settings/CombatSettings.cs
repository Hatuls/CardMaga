using Account;
using Battle.Data;
using CardMaga.Battle.UI;
using FMODUnity;
using System;
using UnityEngine;
namespace CardMaga.UI.Settings
{

    public class CombatSettings : DefaultSettings
    {
        [SerializeField]
        private GameObject[] _surrenderGO;

        [SerializeField]
        protected FmodGlobalEventParameter _sfxParameter;
        [SerializeField]
        protected FmodGlobalEventParameter _musicParameter;

        private void Start()
        {
            SurrenderScreen.OnSurrenderPressed += Hide;

            _musicParameter.Init();
            _sfxParameter.Init();
            int surrenderTutorialIndex = 2;

            bool isBattleConfigIsTutorial = BattleData.Instance.BattleConfigSO.IsTutorial;
            bool isAccountActive = AccountManager.Instance != null;

            // if ( AccountManager.Instance.Data.AccountTutorialData.TutorialProgress>= surrenderTutorialIndex)
            if (isBattleConfigIsTutorial && isAccountActive && AccountManager.Instance.Data.AccountTutorialData.TutorialProgress < surrenderTutorialIndex)
                Array.ForEach(_surrenderGO, x => x.SetActive(false));
        }

        private void OnDestroy()
        {
            SurrenderScreen.OnSurrenderPressed -= Hide;
        }
        public override void ShowSettings()
        {
            base.ShowSettings();
            _musicParameter.Init();
            _sfxParameter.Init();
        }
        public void ToggleSFXSettings()
        {
            _sfxParameter.Toggle();
        }
        public void ToggleMusicSettings()
        {
            _musicParameter.Toggle();
        }
     
    }

    [Serializable]
    public class FmodGlobalEventParameter
    {
        [SerializeField]
        private string _playerPrefName;
        [SerializeField]
        private StudioGlobalParameterTrigger _fmodGlobalParameterTrigger;

        [SerializeField]
        ToggleButton _button;
        private float _value;
        public void Init()
        {
            if (!PlayerPrefs.HasKey(_playerPrefName))
                _value = 0;
            else
                _value = PlayerPrefs.GetFloat(_playerPrefName);
            _button.SetToggleState(_value == 0);
            AssignValue();
        }
        public void Toggle()
        {
            _value = (_value == 0) ? 1 : 0;
            AssignValue();
            SaveToDevice();
        }

        private void AssignValue()
        {
            _fmodGlobalParameterTrigger.value = _value;
            _fmodGlobalParameterTrigger.TriggerParameters();
        }

        private void SaveToDevice()
        {
            PlayerPrefs.SetFloat(_playerPrefName, _value);
        }
    }



}