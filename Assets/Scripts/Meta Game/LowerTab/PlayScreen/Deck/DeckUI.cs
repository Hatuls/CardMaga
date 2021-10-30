using System.Collections;
using System.Collections.Generic;
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
        #endregion

        #region Public Method
        public void init(Sprite firsCardSprite, string deckName)
        {
            _image.sprite = firsCardSprite;
            _deckName.text = deckName;
        }
        #endregion
    }
}
