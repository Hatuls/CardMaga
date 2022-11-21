using UnityEngine;
namespace CardMaga.UI.Settings
{
    public class MainMenuSettings : DefaultSettings
    {
        [SerializeField]
        protected FmodGlobalEventParameter _sfxParameter;
        [SerializeField]
        protected FmodGlobalEventParameter _musicParameter;

        protected virtual void Awake()
        {
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


   
}