using UnityEngine;
namespace Account.GeneralData
{
    [System.Serializable]
    public class AccountDeck
    {
        #region Fields
        [SerializeField]
        CardAccountInfo[] _cards;
        [SerializeField]
        string _deckName;
        #endregion
        #region Properties
        public CardAccountInfo[] Cards { get => _cards; private set => _cards = value; }
        public string DeckName { get => _deckName; set => _deckName = value; }
        #endregion
        #region PublicMethods
        public AccountDeck()
        {

        }
        public AccountDeck(CardAccountInfo[] accountCards)
        {
            _cards = accountCards;
        }
        #endregion
    }
}
