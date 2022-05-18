﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Meta.Settings
{
    public class SettingsScreenUI : TabAbst
    {
        [SerializeField]
        Image _sfxBtnImage;
        [SerializeField]
        Image _masterVolumeBtnImage;

        [SerializeField]
        TextMeshProUGUI _sfxBtnText;
        [SerializeField]
        TextMeshProUGUI _masterVolumeBtnText;

        [SerializeField]
        Color _offColor = Color.red;
        [SerializeField]
        Color _onColor = Color.green;

        const string OnTxt = "On";
        const string OffTxt = "Off";

        [SerializeField]
        GameObject _parent;
        [SerializeField]
        GameObject[] settingsGOToWhenClicked;
        private void Start()
        {
            Close();
        }
        public override void Close()
        {
            if (_parent.activeSelf)
                _parent.SetActive(false);

            for (int i = 0; i < settingsGOToWhenClicked.Length; i++)
            {
                settingsGOToWhenClicked[i].SetActive(false);
            }
        }

        public override void Open()
        {
            SetSettings();
            if (!_parent.activeSelf)
                _parent.SetActive(true);

            for (int i = 0; i < settingsGOToWhenClicked.Length; i++)
            {
                settingsGOToWhenClicked[i].SetActive(true);
            }
        }
        public void SwitchState()
        {
            _parent.SwitchActiveState();
            if (_parent.activeSelf)
                Open();
            else
                Close();
        }
        private void SetSettings()
        {
            SetVFXSettings();
            SetMasterVolumeSettings();
        }

        private void SetMasterVolumeSettings()
        {
            if (Account.AccountManager.Instance.AccountSettingsData.MasterVolume)
            {
                _masterVolumeBtnImage.color = _onColor;
                _masterVolumeBtnText.text = OnTxt;
            }
            else
            {
                _masterVolumeBtnImage.color = _offColor;
                _masterVolumeBtnText.text = OnTxt;
            }
        }

        private void SetVFXSettings()
        {
            if (Account.AccountManager.Instance.AccountSettingsData.SFXEffect)
            {
                _sfxBtnText.text = OnTxt;
                _sfxBtnImage.color = _onColor;
            }
            else
            {
                _sfxBtnText.text = OffTxt;
                _sfxBtnImage.color = _offColor;
            }
        }

        public void SwitchVFX()
        {
            var settings = Account.AccountManager.Instance.AccountSettingsData;
            settings.SFXEffect = !settings.SFXEffect;
            SetSettings();
        }

        public void SwitchMasterVolumeSound()
        {
            var settings = Account.AccountManager.Instance.AccountSettingsData;
            settings.MasterVolume = !settings.MasterVolume;
            SetSettings();
        }
        public void AbandonQuestInGame()
        {
            Close();
            Battles.BattleManager.BattleEnded(true);
            Account.AccountManager.Instance.BattleData.IsFinishedPlaying = true;

        }

        public void ResetAccountSettings()
        {
            ResetDelay();
        }
        private void ResetDelay()
        {
            Account.AccountManager.Instance.ResetAccount();

          //  SceneHandler.LoadScene(SceneHandler.ScenesEnum.NetworkScene);

        }
    }
}
