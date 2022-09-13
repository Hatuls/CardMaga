using CardMaga.Card;
using System;
using System.Collections.Generic;

//public class PlayerCraftingSlots : BaseDeck
//{
//    public static event Action<CardTypeData> OnCardExecute;
//    public static event Action OnPushedSlots;
//    public event Action OnResetCraftingSlot;
//    public override event Action OnResetDeck;
//    public static event Action OnDetectComboRequire;
//    // CraftingUIHandler _playerCraftingUIHandler;
//    CardData _lastCardEntered;
//    public CardData LastCardEntered => _lastCardEntered;
//    public PlayerCraftingSlots(int cardsLength) : base(cardsLength)
//    {
//        // _playerCraftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);
//    }

//    private bool AddCardToEmptySlot(CardData card)
//    {
//        _lastCardEntered = card;
//        var bodypartEnum = card.BodyPartEnum;
//        if (bodypartEnum == CardMaga.Card.BodyPartEnum.Empty || bodypartEnum == CardMaga.Card.BodyPartEnum.None)
//            return true;

//        bool foundEmptySlots = false;
//        for (int i = 0; i < GetDeck.Length; i++)
//        {
//            if (GetDeck[i] == null)
//            {
//                foundEmptySlots = true;
//                GetDeck[i] = card;
//                //_playerCraftingUIHandler.PlaceOnPlaceHolder(i, GetDeck[i]);
//                break;
//            }
//        }
//        return foundEmptySlots;
//    }
//    public override bool AddCard(CardData card)
//    {
//        _lastCardEntered = card;
//        var cardBodyPartEnum = card.BodyPartEnum;
//        if (!(cardBodyPartEnum == CardMaga.Card.BodyPartEnum.Empty || cardBodyPartEnum == CardMaga.Card.BodyPartEnum.None))
//        {

//            if (AddCardToEmptySlot(card) == false)
//            {
//                CardData removingCard = GetDeck[0];

//                for (int i = 1; i < GetDeck.Length; i++)
//                    GetDeck[i - 1] = GetDeck[i];

//                GetDeck[GetDeck.Length - 1] = card;
//                OnCardExecute?.Invoke(card.CardTypeData);
//                //_playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
//            }
//        }
//        CountCards();
//        OnDetectComboRequire?.Invoke();
//        return true;
//    }

//    public void PushSlots()
//    {
//        CardData removingCard = GetDeck[0];

//        for (int i = 1; i < GetDeck.Length; i++)
//            GetDeck[i - 1] = GetDeck[i];

//        GetDeck[GetDeck.Length - 1] = null;
//        //       _playerCraftingUIHandler.ChangeSlotsPos(GetDeck, removingCard);
//        OnPushedSlots?.Invoke();
//        CountCards();
//    }
//    public void AddCard(CardData card, bool toDetect)
//    {
//        _lastCardEntered = card;
//        var bodypartEnum = card.BodyPartEnum;
//        if (bodypartEnum == CardMaga.Card.BodyPartEnum.Empty || bodypartEnum == CardMaga.Card.BodyPartEnum.None)
//            return;

//        CardData lastCardInDeck = null;

//        for (int i = GetDeck.Length - 1; i >= 1; i--)
//        {
//            if (i == GetDeck.Length - 1)
//                lastCardInDeck = GetDeck[i];
//            else GetDeck[i] = GetDeck[i - 1];
//        }

//        GetDeck[0] = card;


//        CountCards();

//        if (toDetect)
//            OnDetectComboRequire?.Invoke();

//    }
//    public override void ResetDeck()
//    {
//        EmptySlots();
//        OnResetDeck?.Invoke();
//        OnResetCraftingSlot?.Invoke();
//        //   _playerCraftingUIHandler.ResetAllSlots();

//        //if(isPlayer) 
//        //      Combo.ComboManager.StartDetection();
//    }



//}



public class CraftingHandler
{
    public event Action OnCraftingSlotsReset;
    public event Action OnComboDetectionRequired;
    public event Action<CardTypeData, IEnumerable<CardTypeData>> OnSlotMoved;


    private CraftingSlot[] _craftingSlots;
    private CardTypeData _lastCardTypeData;
    public CardTypeData LastCardTypeData => _lastCardTypeData;
    public IReadOnlyList<CraftingSlot> CraftingSlots { get => _craftingSlots; }
    public int LengthSize => CraftingSlots.Count;
    public int CountFullSlots
    {
        get
        {
            int count = 0;
            for (int i = 0; i < CraftingSlots.Count; i++)
            {
                if (CraftingSlots[i].IsEmpty== false)
                    count++;
            }
            return count;
        }
    }
    public int CountEmpty
    {
        get
        {
            int count = 0;
            for (int i = 0; i < CraftingSlots.Count; i++)
            {
                if (CraftingSlots[i].IsEmpty )
                    count++;
            }
            return count;
        }
    }
    public IEnumerable<CardTypeData> CardsTypeData
    {
        get
        {
            for (int i = 0; i < CraftingSlots.Count; i++)
                yield return CraftingSlots[i].CardType;
        }
    }

    public CraftingHandler(int length = 3)
    {
        _lastCardTypeData = new CardTypeData()
        {
            BodyPart = CardMaga.Card.BodyPartEnum.Empty,
            CardType = CardMaga.Card.CardTypeEnum.None
        };
        _craftingSlots = new CraftingSlot[length];
        for (int i = 0; i < length; i++)
            _craftingSlots[i] = new CraftingSlot();
    }


    public void ResetCraftingSlots()
    {
        for (int i = 0; i < CraftingSlots.Count; i++)
            _craftingSlots[i].Reset();
        OnCraftingSlotsReset?.Invoke();
    }


    #region Add
    public void AddFront(CardData card, bool toNotify) => AddFront(card?.CardTypeData, toNotify);
    public void AddFront(CardTypeData cardTypeData, bool toNotify)
    {
        if (cardTypeData == null)
            throw new Exception($"CraftingHandler: cardTypeData is null!");

        bool foundPlacement = false;
        for (int i = 0; i < CraftingSlots.Count; i++)
        {
            if (!CraftingSlots[i].IsEmpty)
                continue;

            foundPlacement = true;
            _craftingSlots[i].CardType = cardTypeData;
            break;
        }


        if (!foundPlacement)
        {
            PushBack(true);
            _craftingSlots[_craftingSlots.Length-1].CardType = cardTypeData;
        }

            OnSlotMoved?.Invoke(cardTypeData, CardsTypeData);


        _lastCardTypeData = cardTypeData;
        if (toNotify)
            DetectCombo();
    }
    public void AddBack(CardData cardTypeData, bool toNotify) => AddBack(cardTypeData?.CardTypeData, toNotify);
    public void AddBack(CardTypeData cardTypeData, bool toDetectCombo)
    {
        if (cardTypeData == null)
            throw new Exception($"CraftingHandler: cardTypeData is null!");


        bool foundPlacement = false;
        for (int i = CraftingSlots.Count - 1; i >= 0; i--)
        {
            //pass the not empty slots
            if (!CraftingSlots[i].IsEmpty)
                continue;

            // found empty slot
            foundPlacement = true;
            _craftingSlots[i].CardType = cardTypeData;
            break;
        }

        // no empty slots found so we push one backward and assign its value to the last slot
        if (!foundPlacement)
        {
            PushFront(true);
            _craftingSlots[CraftingSlots.Count - 1].CardType = cardTypeData;
        }

         OnSlotMoved?.Invoke(cardTypeData, CardsTypeData);


        if (toDetectCombo)
            DetectCombo();
    }
    #endregion

    #region Push
    /// <summary>
    /// Push multiple time the card type data from the start to the end
    /// </summary>
    /// <param name="toNotify"></param>
    /// <param name="length"></param>
    public void PushFront(bool toNotify, int length)
    {
        for (int i = 0; i < length; i++)
            PushFront(false);

        if (toNotify) { }
    }
    /// <summary>
    /// Push once the card type data from teh start to the end
    /// </summary>
    /// <param name="toNotify"></param>
    public void PushFront(bool toNotify)
    {
        int count = CraftingSlots.Count;
        CardTypeData lastSlot = CraftingSlots[count - 1].CardType;

        for (int i = 0; i < count - 1; i++)
            _craftingSlots[i + 1].CardType = _craftingSlots[i].CardType;

        _craftingSlots[0].Reset();

        if (toNotify)
            OnSlotMoved?.Invoke(lastSlot, CardsTypeData);
    }


    /// <summary>
    /// Push once the card type data from the end to the start
    /// </summary>
    /// <param name="toNotify"></param>
    public void PushBack(bool toNotify)
    {
        int count = CraftingSlots.Count;
        CardTypeData firstSlot = CraftingSlots[count - 1].CardType;

        for (int i = count - 1; i >= 1; i--)
            _craftingSlots[i - 1].CardType = _craftingSlots[i].CardType;

        _craftingSlots[count - 1].Reset();

        if (toNotify)
            OnSlotMoved?.Invoke(firstSlot, CardsTypeData);

    }
    /// <summary>
    /// Push multiple times the card type data from the end to the start
    /// </summary>
    /// <param name="toNotify"></param>
    /// <param name="length"></param>
    public void PushBack(bool toNotify, int length)
    {
        for (int i = 0; i < length; i++)
            PushBack(toNotify);

        if (toNotify) { }

    }
    #endregion

    public void DetectCombo() => OnComboDetectionRequired?.Invoke();
    public override string ToString()
    {
        if (_craftingSlots == null)
            return base.ToString();
        string result = base.ToString() + "\nContaining:\n";

        int index = 0;
        foreach (var item in CardsTypeData)
        {
            result += $"Index: {index}, CardType: {item.CardType}, BodyPart: {item.BodyPart}\n";
            index++;
        }

        return result;
    }
}

public class CraftingSlot
{
    private CardTypeData _cardType;
    public CardTypeData CardType { get => _cardType; set => _cardType = value; }
    public bool IsEmpty => CardType == null;
    public void Reset()
    {
        _cardType = null;
    }
}