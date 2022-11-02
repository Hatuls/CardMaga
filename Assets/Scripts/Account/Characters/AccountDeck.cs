using UnityEngine;
namespace Account.GeneralData
{
    [System.Serializable]
    public class AccountDeck
    {
        #region Fields
        [SerializeField]
        CardInstanceID[] _cards;
        [SerializeField]
        string _deckName;
        #endregion
        #region Properties
        public CardInstanceID[] Cards { get => _cards;  set => _cards = value; }
        public string DeckName { get => _deckName; set => _deckName = value; }
        #endregion
        #region PublicMethods
        public AccountDeck()
        {

        }
        public AccountDeck(CardInstanceID[] accountCards)
        {
            _cards = accountCards;
        }
        #endregion
    }
}
