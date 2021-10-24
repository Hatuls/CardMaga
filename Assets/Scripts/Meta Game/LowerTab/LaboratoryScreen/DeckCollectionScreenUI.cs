using Battles.UI;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Cards;

namespace UI.Meta.Laboratory
{
    public class DeckCollectionScreenUI : IOpenCloseUIHandler
    {
        #region Fields
        CardUI[] _deckCards;
        TextMeshProUGUI _deckName;
        CardUI _cardUI;
        Image _characterImage;
        CardUIInfoScreen _cardUIinfoScreen;
        CharacterDeckSelection _characteDeckSelection;
        GameObject _deckCollectionScreenPanel;
        #endregion
        #region Public Methods
        public void InitCardsUI(int index = 0)
        {

        }
        public void OpenInfoPanel(Card card)
        {

        }
        public void Use(Card card)
        {

        }
        public void Replace(Card card)
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
