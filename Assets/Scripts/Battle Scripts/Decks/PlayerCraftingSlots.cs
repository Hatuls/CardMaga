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
        var bodypartEnum = card.BodyPartEnum;
        if (bodypartEnum == Cards.BodyPartEnum.Empty || bodypartEnum == Cards.BodyPartEnum.None)
            return true;

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
    public override bool AddCard(Card card)
    {
        var bodypartEnum = card.BodyPartEnum;
        if (bodypartEnum == Cards.BodyPartEnum.Empty || bodypartEnum == Cards.BodyPartEnum.None)
            return true;

        if (AddCardToEmptySlot(card) == false)
        {
            Card removingCard = GetDeck[0];

            for (int i = 1; i < GetDeck.Length; i++)
                GetDeck[i - 1] = GetDeck[i];

            GetDeck[GetDeck.Length - 1] = card;
            _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
        }

        CountCards();
        Combo.ComboManager.StartDetection();
        return true;
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

        var bodypartEnum = card.BodyPartEnum;
        if (bodypartEnum == Cards.BodyPartEnum.Empty || bodypartEnum == Cards.BodyPartEnum.None)
            return;

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

  
}

