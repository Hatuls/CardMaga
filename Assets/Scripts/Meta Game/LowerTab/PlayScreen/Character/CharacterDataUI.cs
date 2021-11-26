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
        CharacterData _characterData;
        bool _isOpen = false;
        #endregion
        #region Public Methods
        public void Init(CharacterData characterData,ushort playerLevel,CharacterSO characterSO)
        {
            _characterData = characterData;
            Debug.Log($"Init {_characterData.CharacterEnum.ToString()} Character Data");
            gameObject.SetActive(true);
            CheckLevel(playerLevel, _characterData.UnlockAtLevel);
            SetLockedImage();
            _characterImage.sprite = characterSO.CharacterSprite;
            _characterName.text = characterSO.name;
        }
        private void CheckLevel(ushort playerLevel, byte unlocksAtLevel)
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
                Debug.Log("Disabling Locked Image");
                _lockedImage.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Enabling Locked Image");
                _lockedImage.gameObject.SetActive(true);
            }
        }
        public void Selected()
        {
            PlayScreenUI.Instance.CharacterChoosen(_characterData);
            

        }
        #endregion
    }
}
