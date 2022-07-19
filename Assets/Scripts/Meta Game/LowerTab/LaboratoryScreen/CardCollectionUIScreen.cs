using UnityEngine;
using TMPro;
using CardMaga.UI.Card;

namespace UI.Meta.Laboratory
{
    public class CardCollectionUIScreen : MonoBehaviour
    {
        #region Fields
        TextMeshProUGUI _amountText;
        GameObject _infoPanel;
        CardUI[] _cardUI;
        #endregion
        #region Public Methods
        public void SortByLevel()
        {
            
        }
        public void CardUIClicked(CardUI _cardUI)
        {

        }
        #endregion
        #region Interface
        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
