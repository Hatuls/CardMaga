using Account.GeneralData;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class DeckUI: MonoBehaviour
    {
        #region Field
        [SerializeField]
        Image _image;
        [SerializeField]
        TextMeshProUGUI _deckName;
        AccountDeck _deckData;
        #endregion

        #region Public Method
        public void Init(Sprite firsCardSprite, AccountDeck deckData)
        {
            _image.sprite = firsCardSprite;
            _deckName.text = deckData.DeckName;
            _deckData = deckData;
        }
        public void SelectDeck()
        {
            PlayScreenUI.Instance.DeckChoosen(_deckData);
        }
        #endregion
    }
}
