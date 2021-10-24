using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class ChooseCharacterScreen : MonoBehaviour
    {
        #region Singleton
        private static ChooseCharacterScreen _instance;
        public static ChooseCharacterScreen GetInstance
        {
            get
            {
                if (_instance == null)
                    Debug.LogError("ChooseCharacterScreen is null!");

                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion

        #region Fields
        CharacterDataUI _characters;
        GameObject _chooseCharacterPanel;
        #endregion
        #region Public Methods
        public void Init()
        {

        }
        public void Selected(CharacterEnum characterEnum)
        {
            
        }
        public void ChooseCharacterSwitch()
        {

        }
        public void ResetCharacterScreen()
        {

        }
        #endregion
    }
}
