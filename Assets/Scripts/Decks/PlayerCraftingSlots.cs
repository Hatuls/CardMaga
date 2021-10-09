using Battles.UI;
using Cards;

public class PlayerCraftingSlots : Battles.Deck.DeckAbst
{
    CraftingUIHandler _playerCraftingUIHandler;
    public PlayerCraftingSlots(bool isPlayer, int cardsLength) : base(isPlayer, cardsLength)
    {
        _playerCraftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);
    }

    private bool AddCardToEmptySlot(Card card)
    {
        bool foundEmptySlots = false;
        for (int i = 0; i < GetDeck.Length; i++)
        {
            if (GetDeck[i] == null)
            {
                foundEmptySlots = true;
                GetDeck[i] = card;
                _playerCraftingUIHandler.PlaceOnPlaceHolder(i, GetDeck[i]);
                break;
            }
        }
        return foundEmptySlots;
    }
    public override void AddCard(Card card)
    {

        if (AddCardToEmptySlot(card) == false)
        {
            Card removingCard = GetDeck[0];

            for (int i = 1; i < GetDeck.Length; i++)
                GetDeck[i - 1] = GetDeck[i];

            GetDeck[GetDeck.Length - 1] = card;
            _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);

            //for (int i = 0; i < GetDeck.Length; i++)
            //    _playerCraftingUIHandler.PlaceOnPlaceHolder(i, GetDeck[i]);


        }

        CountCards();
        Combo.ComboManager.StartDetection();
    }

    public void PushSlots()
    {
        Card removingCard = GetDeck[0];

        for (int i = 1; i < GetDeck.Length; i++)
            GetDeck[i - 1] = GetDeck[i];

        GetDeck[GetDeck.Length - 1] = null;
        _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
        CountCards();
    }
    public void AddCard(Card card , bool toDetect)
    {
        Card lastCardInDeck = null;
        
        for (int i = GetDeck.Length - 1; i >= 1; i--)
        {
            if (i == GetDeck.Length - 1)
                lastCardInDeck = GetDeck[i];
            else GetDeck[i] = GetDeck[i - 1];
        }

        GetDeck[0] = card;


        CountCards();

        if(toDetect)
        Combo.ComboManager.StartDetection();
    }
    public override void ResetDeck()
    {
        base.ResetDeck(); 
        _playerCraftingUIHandler.ResetAllSlots();

      //if(isPlayer) 
      //      Combo.ComboManager.StartDetection();
    }
    void ResetPlaceHolderUI(int i)
    {
        _playerCraftingUIHandler.ResetPlaceHolderUI(i);
    }

    //when getting a card I move all other cards first
    //if card is on the 5th index I turn his UI off and reset the slot
    //if a card in on the second to the 4th slot I move him by 1 with leen tween and his data

    //assign his data to the first slot
    //turn on his UI on the first slot
    //than move him to the next slot and data
    //clear the first slot
}


//public class OpponentCraftingSlot : Battles.Deck.DeckAbst
//{
//    CraftingUIHandler _opponentCraftingUIHandler;
//    public OpponentCraftingSlot(int cardsLength) : base(cardsLength)
//    {
//        _opponentCraftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(false);
//    }
//    public override void AddCard(Card card)
//    {
//        for (int i = GetDeck.Length - 1; i >= 0; i--)
//        {
//            if (i != 0)
//            {
//                GetDeck[i] = GetDeck[i - 1];
//            }
//            else
//            {
//                GetDeck[i] = card;
//            }
//            if (i == GetDeck.Length - 1)
//            {
//                GetDeck[i] = null;
//            }


//            _opponentCraftingUIHandler.PlaceOnPlaceHolder(i, GetDeck[i]);
//        }

//        _opponentCraftingUIHandler.ChangeSlotsPos(GetDeck);
//       CountCards();
//     //  Combo.ComboManager.StartDetection();
//    }
//    public override void ResetDeck()
//    {
//        base.ResetDeck();
//        _opponentCraftingUIHandler.ResetAllSlots();
//    //    Combo.ComboManager.StartDetection();
//    }
//    void ResetPlaceHolderUI(int i)
//    {
//        _opponentCraftingUIHandler.ResetPlaceHolderUI(i);
//    }


//}