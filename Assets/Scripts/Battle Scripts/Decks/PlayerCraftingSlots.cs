using Battle.Deck;
using Battle.UI;
using CardMaga.Card;

public class PlayerCraftingSlots : BaseDeck
    {
    CraftingUIHandler _playerCraftingUIHandler;
    CardData _lastCardEntered;
    public CardData LastCardEntered => _lastCardEntered;
    public PlayerCraftingSlots(bool isPlayer, int cardsLength) : base(isPlayer, cardsLength)
    {
        _playerCraftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);
    }

    private bool AddCardToEmptySlot(CardData card)
    {
        _lastCardEntered = card;
        var bodypartEnum = card.BodyPartEnum;
        if (bodypartEnum == CardMaga.Card.BodyPartEnum.Empty || bodypartEnum == CardMaga.Card.BodyPartEnum.None)
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
    public override bool AddCard(CardData card)
    {
        _lastCardEntered = card;
        var bodypartEnum = card.BodyPartEnum;
        if (!(bodypartEnum == CardMaga.Card.BodyPartEnum.Empty || bodypartEnum == CardMaga.Card.BodyPartEnum.None))
        {

            if (AddCardToEmptySlot(card) == false)
            {
                CardData removingCard = GetDeck[0];

                for (int i = 1; i < GetDeck.Length; i++)
                    GetDeck[i - 1] = GetDeck[i];

                GetDeck[GetDeck.Length - 1] = card;
                _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
            }
        }
        CountCards();
        Battle.ComboManager.StartDetection();
        return true;
    }

    public void PushSlots()
    {
        CardData removingCard = GetDeck[0];

        for (int i = 1; i < GetDeck.Length; i++)
            GetDeck[i - 1] = GetDeck[i];

        GetDeck[GetDeck.Length - 1] = null;
        _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
        CountCards();
    }
    public void AddCard(CardData card, bool toDetect)
    {
        _lastCardEntered = card;
        var bodypartEnum = card.BodyPartEnum;
        if (bodypartEnum == CardMaga.Card.BodyPartEnum.Empty || bodypartEnum == CardMaga.Card.BodyPartEnum.None)
            return;

        CardData lastCardInDeck = null;

        for (int i = GetDeck.Length - 1; i >= 1; i--)
        {
            if (i == GetDeck.Length - 1)
                lastCardInDeck = GetDeck[i];
            else GetDeck[i] = GetDeck[i - 1];
        }

        GetDeck[0] = card;


        CountCards();

        if (toDetect)
            Battle.ComboManager.StartDetection();
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

