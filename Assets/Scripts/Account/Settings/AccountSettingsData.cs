namespace Account.Settings
{
    public class AccountSettingsData
    {
        #region Fields
        bool _musicSwitch;
        bool _sfxSwitch;
        bool _screenShake;
        #endregion
        #region Properties
        public bool MusicSwitch { get => _musicSwitch; set => _musicSwitch = value; }
        public bool SFXSwitch { get => _sfxSwitch; set => _sfxSwitch = value; }
        public bool ScreenShake { get => _screenShake; set => _screenShake = value; }
        #endregion
        #region PublicMethods
        public void InitData(AccountSettingsData accountSettingsData = null)
        {

        }
        #endregion
    }
}
