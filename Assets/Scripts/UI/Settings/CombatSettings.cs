using Battle.Data;
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

        protected virtual void Awake()
        {
            _musicParameter.Init();
            _sfxParameter.Init();
            if (BattleData.Instance.BattleConfigSO.IsTutorial)
                Array.ForEach(_surrenderGO, x => x.SetActive(false));
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