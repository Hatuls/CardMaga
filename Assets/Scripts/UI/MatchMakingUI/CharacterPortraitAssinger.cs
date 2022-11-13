using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.MatchMMaking
{
    public class CharacterPortraitAssinger : MonoBehaviour
    {
        [SerializeField] private Image _characterImage;
        [SerializeField] private TMP_Text _characterName;

        public void AssignCharacter(Sprite characterImage,string characterName)
        {
            _characterImage.sprite = characterImage;
            _characterName.text = characterName;
        }
    }

}
