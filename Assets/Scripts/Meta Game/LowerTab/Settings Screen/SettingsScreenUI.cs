using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI.Meta.Settings
{
    public class SettingsScreenUI : TabAbst
    {
        public static event Action<bool> OnAbandon;
        private const string OffTxt = "Off";
        private const string OnTxt = "On";


        [SerializeField]
        private Image _sfxBtnImage;
        [SerializeField]
        private Image _masterVolumeBtnImage;

        [SerializeField]
        private TextMeshProUGUI _sfxBtnText;
        [SerializeField]
        private TextMeshProUGUI _masterVolumeBtnText;

        [SerializeField]
        private Color _offColor = Color.red;
        [SerializeField]
        private Color _onColor = Color.green;


        [SerializeField]
        private GameObject _parent;
        [SerializeField]
        private GameObject[] settingsGOToWhenClicked;


        [SerializeField]
        private SceneIdentificationSO _networkScene;

        private ISceneHandler _sceneHandler;

        private void Inject(ISceneHandler sh)
            => _sceneHandler = sh;



        #region MonoBehaviour Callbacks

        private void Start()
        {
            Close();
        }
        private void Awake()
        {
            SceneHandler.OnSceneHandlerActivated += Inject;
            EndRunScreen.OnFinishGame += Close;
        }
        private void OnDestroy()
        {
            EndRunScreen.OnFinishGame -= Close;
           
            SceneHandler.OnSceneHandlerActivated -= Inject;
        }
        #endregion
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
        // Need To be Re-Done
        private void SetMasterVolumeSettings()
        {
            //if (Account.AccountManager.Instance.AccountSettingsData.MasterVolume)
            //{
            //    _masterVolumeBtnImage.color = _onColor;
            //    _masterVolumeBtnText.text = OnTxt;
            //}
            //else
            {
                _masterVolumeBtnImage.color = _offColor;
                _masterVolumeBtnText.text = OnTxt;
            }
        }
        // Need To be Re-Done
        private void SetVFXSettings()
        {
            //if (Account.AccountManager.Instance.AccountSettingsData.SFXEffect)
            //{
            //    _sfxBtnText.text = OnTxt;
            //    _sfxBtnImage.color = _onColor;
            //}
            //else
            {
                _sfxBtnText.text = OffTxt;
                _sfxBtnImage.color = _offColor;
            }
        }
        // Need To be Re-Done
        public void SwitchVFX()
        {
            //var settings = Account.AccountManager.Instance.AccountSettingsData;
            //settings.SFXEffect = !settings.SFXEffect;
            SetSettings();
        }
        // Need To be Re-Done
        public void SwitchMasterVolumeSound()
        {
            //var settings = Account.AccountManager.Instance.AccountSettingsData;
            //settings.MasterVolume = !settings.MasterVolume;
            SetSettings();
        }
        // Need To be Re-Done
        public void AbandonQuestInGame()
        {
            Close();
            OnAbandon?.Invoke(true);
           // Account.AccountManager.Instance.BattleData.IsFinishedPlaying = true;

        }

        public void ResetAccountSettings()
        {
            ResetDelay();
        }
        // Need To be Re-Done
        private void ResetDelay()
        {
         //   Account.AccountManager.Instance.ResetAccount();

            _sceneHandler.MoveToScene(_networkScene);
        }
    }
}
