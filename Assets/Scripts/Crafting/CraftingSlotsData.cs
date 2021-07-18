using Battles.UI;
using Cards;

public class CraftingSlotsData: Battles.Deck.DeckAbst
{
    public CraftingSlotsData(int cardsLength):base(cardsLength)
    {
    }
    public override void AddCard(Card card)
    {
        for (int i = GetDeck.Length - 1; i >= 0; i--)
        {
            if (i != 0)
            {
                GetDeck[i] = GetDeck[i - 1];
            }
            else
            {
                GetDeck[i] = card;
            }
            if(i == GetDeck.Length - 1)
            {
                GetDeck[i] = null;
            }
            PlaceHolderHandler.PlaceOnPlaceHolder(i, GetDeck[i]);
        }
        PlaceHolderHandler.ChangeSlotsPos(GetDeck);
        CountCards();
        Relics.RelicManager.StartDetection();
    }
    public override void ResetDeck()
    {
        base.ResetDeck();
        PlaceHolderHandler.ResetAllSlots();
        Relics.RelicManager.StartDetection();
    }
    void ResetPlaceHolderUI(int i)
    {
        PlaceHolderHandler.ResetPlaceHolderUI(i);
    }

    //when getting a card I move all other cards first
    //if card is on the 5th index I turn his UI off and reset the slot
    //if a card in on the second to the 4th slot I move him by 1 with leen tween and his data

    //assign his data to the first slot
    //turn on his UI on the first slot
    //than move him to the next slot and data
    //clear the first slot
}
