using Account;
using TMPro;
using UnityEngine;
namespace CardMaga.UI.Settings
{
    public class MainMenuSettings : DefaultSettings
    {
        private const string PLAYFAB_ID = "User ID: ";
        private const string APP_ID= "Version: ";
        [SerializeField]
        private TextMeshProUGUI _playfabIDText;
        [SerializeField]
        private TextMeshProUGUI _versionIDText;
        [SerializeField]
        protected FmodGlobalEventParameter _sfxParameter;
        [SerializeField]
        protected FmodGlobalEventParameter _musicParameter;

        public override void ShowSettings()
        {
            base.ShowSettings();
            _sfxParameter.Init();
            _musicParameter.Init();
            _playfabIDText.text = string.Concat(PLAYFAB_ID, AccountManager.Instance.PlayfabID);
            _versionIDText.text = string.Concat(APP_ID, Application.version);
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



}