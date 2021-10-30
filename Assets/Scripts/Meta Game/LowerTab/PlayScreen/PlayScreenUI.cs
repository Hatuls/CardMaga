using Meta.Resources;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class PlayScreenUI : MonoBehaviour
    {
        #region Singleton
        private static PlayScreenUI _instance;
        public static PlayScreenUI Instance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("CardManager is null!");

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
        ChooseDeckScreen _chooseDeckScreen = new ChooseDeckScreen();
        [SerializeField]
        ChooseCharacterScreen _chooseCharacterScreen;
        [SerializeField]
        GameObject _backgroundPanel;
        #endregion
        #region Properties
        public ChooseDeckScreen ChooseLoadOutScreen => _chooseDeckScreen;
        #endregion
        #region Public Methods
        public void OpenPlayScreen()
        {

        }
        public void ResetPlayScreen()
        {
            _chooseCharacterScreen.ChooseCharacterSetActiveState(false);
            _chooseDeckScreen.RestLoadOutScreen();
            BGPanelSetActiveState(false);
        }
        public void OnPlayClicked()
        {
            EnergyHandler energyHandler = (EnergyHandler)ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy);

            if (energyHandler.HasAmount(energyHandler.AmountToStartPlay))
            {
                Debug.Log("Activating Character Panel");
                BGPanelSetActiveState(true);
                _chooseCharacterScreen.Init();
            }
        }
        public void BGPanelSetActiveState(bool isOn)
        {
            _backgroundPanel.SetActive(isOn);
        }
        #endregion
    }
}
