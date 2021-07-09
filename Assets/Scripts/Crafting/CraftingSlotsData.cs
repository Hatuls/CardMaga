using Battles.UI;
using Cards;

public class CraftingSlotsData: Battles.Deck.DeckAbst
{
    #region Fields
    static PlaceHolderHandler _placeHolderHandler;
    #endregion
    #region Properties
    public static PlaceHolderHandler SetPlaceHolderHandler { set => _placeHolderHandler = value; }
    #endregion
    public CraftingSlotsData(Card[] cards):base(cards)
    {

    }
    public override void AddCard(Card card)
    {
        for (int i = GetDeck.Length - 1; i >= 0; i--)
        {
            if (i == GetDeck.Length - 1)
            {
                DiscardCard(GetDeck[i]);
            }
            else if (i > 0)
            {
                MoveToNextSlot(i);
            }
            else
            {
                MoveToNextSlot(i);
                GetDeck[0] = card;
            }
        }
    }
    void MoveToNextSlot(int i)
    {
        GetDeck[i + 1] = GetDeck[i];
        //_placeHolderHandler
    }
}
