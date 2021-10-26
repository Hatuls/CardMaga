using Meta.Resources;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class PlayScreenUI : MonoBehaviour
    {
        #region Fields
        ChooseLoadOutScreen _chooseLoadOutScreen;
        [SerializeField]
        ChooseCharacterScreen _chooseCharacterScreen;
        #endregion

        #region Public Methods
        public void Awake()
        {

        }
        public void OpenPlayScreen()
        {

        }
        public void ResetPlayScreen()
        {

        }
        public void OnPlayClicked()
        {
            EnergyHandler energyHandler = (EnergyHandler)ResourceManager.Instance.GetResourceHandler<ushort>(ResourceType.Energy);

            if (energyHandler.HasAmount(energyHandler.AmountToStartPlay))
            {
                _chooseCharacterScreen.ChooseCharacterSwitch();
            }
        }
        #endregion
    }
}
