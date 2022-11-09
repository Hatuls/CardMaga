using UnityEngine;
namespace Account.GeneralData
{
    [System.Serializable]
    public class AccountDeck
    {
        #region Fields
        [SerializeField]
        CardInstance[] _cards;
        [SerializeField]
        string _deckName;
        #endregion
        #region Properties
        public CardInstance[] Cards { get => _cards;  set => _cards = value; }
        public string DeckName { get => _deckName; set => _deckName = value; }
        #endregion
        #region PublicMethods
        public AccountDeck()
        {

        }
        public AccountDeck(CardInstance[] accountCards)
        {
            _cards = accountCards;
        }
        #endregion
    }
}
