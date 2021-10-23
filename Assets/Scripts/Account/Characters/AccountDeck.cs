namespace Account.GeneralData
{
    public class AccountDeck
    {
        #region Fields;
        CardAccountInfo[] _cards;
        string _deckName;
        #endregion
        #region Properties
        public CardAccountInfo[] Cards { get => _cards; private set => _cards = value; }
        public string DeckName { get => _deckName; set => _deckName = value; }
        #endregion
        #region PublicMethods
        public AccountDeck(CardAccountInfo[] cards)
        {

        }
        #endregion
    }
}
