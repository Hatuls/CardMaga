using Account.GeneralData;
using UnityEngine.UI;
using UnityEngine;
using Battles;
using TMPro;

namespace UI.Meta.PlayScreen
{
    public class CharacterDataUI : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        Image _characterImage;
        [SerializeField]
        Image _lockedImage;
        [SerializeField]
        TextMeshProUGUI _characterName;
        CharacterEnum _characterEnum;
        bool _isOpen = false;
        #endregion
        #region Public Methods
        public void Init(CharacterData characterData,byte playerLevel,CharacterSO characterSO)
        {
            CheckLevel(playerLevel,characterData.UnlockAtLevel);
            SetLockedImage();
            _characterEnum = characterData.CharacterEnum;
            _characterImage.sprite = characterSO.CharacterSprite;
            _characterName.text = characterSO.name;
        }
        private void CheckLevel(byte playerLevel, byte unlocksAtLevel)
        {
            Debug.Log($"Player level is {playerLevel}");
            Debug.Log($"Character unlocks at level {unlocksAtLevel}");
            if(playerLevel >= unlocksAtLevel)
            {
                Debug.Log($"Character is unlocked");
                _isOpen = true;
            }
        }
        private void SetLockedImage()
        {
            if(_isOpen)
            {
                _lockedImage.gameObject.SetActive(false);
            }
            else
            {
                _lockedImage.gameObject.SetActive(true);
            }
        }
        public void Selected()
        {
            PlayScreenUI playScreenUI = PlayScreenUI.Instance;
            playScreenUI.ResetPlayScreen();
            playScreenUI.ChooseLoadOutScreen.InitLoadOutScreen(_characterEnum);
        }
        #endregion
    }
}
