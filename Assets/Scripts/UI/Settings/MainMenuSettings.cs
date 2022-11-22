using UnityEngine;
namespace CardMaga.UI.Settings
{
    public class MainMenuSettings : DefaultSettings
    {
        [SerializeField]
        protected FmodGlobalEventParameter _sfxParameter;
        [SerializeField]
        protected FmodGlobalEventParameter _musicParameter;

        public override void ShowSettings()
        {
            base.ShowSettings();
            _sfxParameter.Init();
            _musicParameter.Init();
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