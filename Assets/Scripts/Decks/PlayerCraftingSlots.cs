using System;
using Account.GeneralData;
using Battle.Deck;
using Battle.UI;
using CardMaga.Card;

public class PlayerCraftingSlots : BaseDeck
{
    public static event Action<CardTypeData> OnCardExecute;
    public static event Action OnPushedSlots;
    public static event Action<bool> OnResetCrftingSlot;
    public override event Action OnResetDeck;
    // CraftingUIHandler _playerCraftingUIHandler;
    CardData _lastCardEntered;
    public CardData LastCardEntered => _lastCardEntered;
    public PlayerCraftingSlots(bool isPlayer, int cardsLength) : base(isPlayer, cardsLength)
    {
       // _playerCraftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);
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
                //_playerCraftingUIHandler.PlaceOnPlaceHolder(i, GetDeck[i]);
                break;
            }
        }
        return foundEmptySlots;
    }
    public override bool AddCard(CardData card)
    {
        _lastCardEntered = card;
        var cardBodyPartEnum = card.BodyPartEnum;
        if (!(cardBodyPartEnum == CardMaga.Card.BodyPartEnum.Empty || cardBodyPartEnum == CardMaga.Card.BodyPartEnum.None))
        {

            if (AddCardToEmptySlot(card) == false)
            {
                CardData removingCard = GetDeck[0];

                for (int i = 1; i < GetDeck.Length; i++)
                    GetDeck[i - 1] = GetDeck[i];

                GetDeck[GetDeck.Length - 1] = card;
                OnCardExecute?.Invoke(card.CardTypeData);
                //_playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
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
        //       _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
        OnPushedSlots?.Invoke();
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
        EmptySlots();
        OnResetDeck?.Invoke();
        OnResetCrftingSlot?.Invoke(isPlayer);
     //   _playerCraftingUIHandler.ResetAllSlots();

        //if(isPlayer) 
        //      Combo.ComboManager.StartDetection();
    }


  
}

