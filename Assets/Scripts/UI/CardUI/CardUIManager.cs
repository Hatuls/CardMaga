
using UnityEngine;

using System.Collections;
using Battles.Deck;
using Cards;
using Unity.Events;
using System;

namespace Battles.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>, ITouchable
    {
        #region Field
        [Tooltip("List of cards that UI Know About")]
        [SerializeField] CardUI[] _cardUIArr;

        [Tooltip("Current Clicked Card UI")]
        [SerializeField] CardUI _clickedCardUI;

        [Tooltip("Hand Middle Position")]
        [SerializeField] RectTransform _handMiddlePosition;

        [Tooltip("Draw Card position")]
        [SerializeField] RectTransform _drawDeckPosition;

        [Tooltip("Discard Card position")]
        [SerializeField] RectTransform _discardDeckPosition;

        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _exhaustDeckPosition;    
        
        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _craftingBtnPosition;

        [Tooltip("Card UI Settings")]
        [SerializeField] CardUISO _cardUISettings;

        private HandUI _handUI;
        private CardUI _zoomedCard;

        private CardUITransition[] _transitions;

        bool _isTryingToPlace;

        [SerializeField]
        Art.ArtSO _artSO;
        #endregion

        #region Events
        [SerializeField]
        Unity.Events.SoundsEvent _soundEvent;
        #endregion

        #region Properties
        public CardUI[] CardUIArr => _cardUIArr;

        public RectTransform GetDrawDeckPosition => _drawDeckPosition;
        public RectTransform GetDiscardDeckPosition => _discardDeckPosition;
        public RectTransform GetExhaustDeckPosition => _exhaustDeckPosition;
        public RectTransform GetHandMiddlePosition => _handMiddlePosition;
        public CardUI ClickedCardUI
        {
            get => _clickedCardUI;

            set
            {

                (value == null ? _clickedCardUI : value)?.GFX.GlowCard(value != null);

                _clickedCardUI = value;

            }
        }

        public bool IsTryingToPlace { get => _isTryingToPlace; set { _isTryingToPlace = value; } }

        #endregion

        #region MonoBehaiviour callbacks


        #endregion

        #region Private Methods
        public CardUI ActivateCard(Card card, Location loc)
            => ActivateCard(card, GetLocation(loc));
        
       
        internal void CreateCardUI(Card addedCard, DeckEnum toDeck)
        {
            var cardui  = ActivateCard(addedCard, addedCard.GetSetCard.ComboCraftingSettings._startPosition);
            var handler = GetCardUIHandler<CraftCardUIHandler>();
            StartCoroutine(handler.MoveCardsUI(new CardUI [1] { cardui }, GetLocation(addedCard.GetSetCard.ComboCraftingSettings._startPosition) , GetDeckPosition(toDeck)));
        }

        public CardUI ActivateCard(Card cardData, Vector2 pos)
        {
            if (cardData != null)
            {
                //get one cardUI
                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                        break;
                    if (CardUIArr[i].gameObject.activeInHierarchy == false || CardUIArr[i].gameObject.activeSelf == false)
                    {

                        CardUIArr[i].CardTranslations?.SetScale(_cardUISettings.StartScaleSize*Vector3.one, Time.deltaTime);
                        CardUIArr[i].gameObject.SetActive(true);
                        CardUIArr[i].CardTranslations?.SetPosition(pos);
                        CardUIArr[i].CardTranslations.SetRotation(Vector3.zero);
                        AssignDataToCardUI(CardUIArr[i], cardData);

                        if (CardUIArr[i].Inputs != null)
                            CardUIArr[i].Inputs.GetCanvasGroup.blocksRaycasts = true;

                        return CardUIArr[i];
                    }
                }
            }
            Debug.LogError("CardUI is NUll");
            return null;

            //turn it on
            //position of turning on
            //add data
        }
        public void AssignDataToCardUI(CardUI card, Cards.Card cardData)
        {
            card.GFX.SetCardReference(cardData, _artSO);

        }
        private Vector3 GetDeckPosition(DeckEnum fromDeck)
        {
            switch (fromDeck)
            {
                case DeckEnum.PlayerDeck:
                    return GetDrawDeckPosition.anchoredPosition3D;
                case DeckEnum.Hand:
                    return GetHandMiddlePosition.anchoredPosition3D;
                case DeckEnum.Disposal:
                    return GetDiscardDeckPosition.anchoredPosition3D;
                case DeckEnum.Exhaust:
                    return GetExhaustDeckPosition.anchoredPosition3D;
                default:
                    return Vector3.zero;
            }
        }
        public Vector2 GetLocation(Location loc)
        {
            Vector2 location = Vector2.zero;
            switch (loc)
            {
                case Location.Hand:
                    location = _handMiddlePosition.anchoredPosition3D;
                    break;
                case Location.Discard:
                    location = _discardDeckPosition.anchoredPosition3D;
                    break;
                case Location.Exhaust:
                    location = _exhaustDeckPosition.anchoredPosition3D;
                    break;
                case Location.Drawpile:
                    location = _drawDeckPosition.anchoredPosition3D;
                    break;
                case Location.Crafting:
                    location = _craftingBtnPosition.anchoredPosition3D;
                    break;
                case Location.MiddleScreenPosition:
                    location = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight) / 2;
                    break;
                default:
                    break;
            }
            return location;
        }
        #endregion


        #region Public Methods
        public void InitCardUI()
        {

            if (CardUIArr == null && CardUIArr.Length == 0)
            {
                Debug.LogError("Error in CardUIArr");
                return;
            }
            else
            {
                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                    {
                        Debug.LogError("CardUI is NUll");
                        return;
                    }
                    CardUIArr[i].gameObject.SetActive(false);
                }
            }
        }
        // remove
        public void DrawCards(Card[] cardData, DeckEnum fromDeck)
        {       
            Vector2 deckPos = GetDeckPosition(fromDeck);
              CardUI[] cards = new CardUI[cardData.Length];
            for (int j = 0; j < cardData.Length; j++)
            {
                if (_handUI.GetAmountOfCardsInHand >= _cardUISettings.GetAmountOfCardsUIInHand)
                {
                    Debug.Log("Tried to draw card but couldnt because hand is full");
                    break;
                }
                if (cardData[j] != null)
                {
                cards[j] = ActivateCard(cardData[j], deckPos);     
                }
            }
            StartCoroutine(GetCardUIHandler<DrawCardUIHandler>().MoveCardsUI(cards, GetDeckPosition(DeckEnum.Hand), deckPos));
        }
       

        public void RemoveHands()
        {
            if (_zoomedCard != null)
            {
                AddToHandUI(_zoomedCard);
                _zoomedCard = null;
            }
            if (ClickedCardUI != null)
            {
                AddToHandUI(ClickedCardUI);
                ClickedCardUI = null;
            }
            CardUI[] array = _handUI.GetHandCards;
            var removal = GetCardUIHandler<DiscardHandHandler>();
         //   StopCoroutine(removal.MoveCardsUI(_handUI.GetHandCards, GetDeckPosition(DeckEnum.Disposal), GetDeckPosition(DeckEnum.Hand)));
            StartCoroutine(removal.MoveCardsUI(array, GetDeckPosition(DeckEnum.Disposal), GetDeckPosition(DeckEnum.Hand)));
     
        }
        public override void Init()
        {
            InitCardUI();
            _handUI = new HandUI(ref _cardUISettings.GetAmountOfCardsUIInHand, GetHandMiddlePosition.anchoredPosition, _cardUISettings);

            _transitions = new CardUITransition[3]
            {
                new DrawCardUIHandler(this,_handUI,_soundEvent,_cardUISettings),
                new RemoveCardAfterACtivated(this, _cardUISettings, _soundEvent),
                new DiscardHandHandler(this,_cardUISettings,_soundEvent)
            };
        }

        private T GetCardUIHandler<T>() where T : CardUITransition
        {
            for (int i = 0; i < _transitions.Length; i++)
            {
                if (_transitions[i].GetType() == typeof(T) )
                {
                    return _transitions[i] as T;
                }
            }
            return null;
        }




        public void SetCardUI(CardUI cardUI)
        {
            if (cardUI == null)
            {
                Debug.LogError("Error in Set Card UI");
                return;
            }
            //  Debug.Log("Card Was Set");
            InputManager.Instance.AssignObjectFromTouch(this);
            DeckManager.Instance.TransferCard(DeckEnum.Hand, DeckEnum.Selected, cardUI.GFX.GetCardReference);
            ClickedCardUI = cardUI;
            _soundEvent?.Raise(SoundsNameEnum.SelectCard);
        }
        public void AddToHandUI(CardUI cache)
        {
            cache?.GFX.GlowCard(false);
            _handUI.Add(cache);
        }
        public void TryRemoveFromHandUI(CardUI cache)
        {

            if (_zoomedCard != null)
            {
                AddToHandUI(_zoomedCard);
                _zoomedCard.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
                _zoomedCard = null;
            }
            _handUI.TryRemove(cache);
        }
        public void OnClickedCardUI(CardUI card)
        {

            if (card.GFX.GetRectTransform.localScale == _cardUISettings.GetCardUIZoomedScale)
            {
                _zoomedCard = null;
                AddToHandUI(card);
                card.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
            }
            else
            {
                TryRemoveFromHandUI(card);
                card.CardTranslations?.MoveCard(true, Vector2.zero, _cardUISettings.GetCardMoveToDeckDelay);
                card.CardTranslations?.SetScale(_cardUISettings.GetCardUIZoomedScale, _cardUISettings.GetCardScaleDelay);
                _zoomedCard = card;
                _zoomedCard.CardTranslations?.SetRotation(Vector3.zero);
            }

            _soundEvent?.Raise(SoundsNameEnum.TapCard);
        }

        public void StartRemoveProcess()
        {
            if (ClickedCardUI == null)
                return;

            TryRemoveFromHandUI(ClickedCardUI);
            CardUI card = ClickedCardUI;
            ClickedCardUI = null;

            var handler = GetCardUIHandler<RemoveCardAfterACtivated>();
            StartCoroutine(handler.MoveCardsUI(new CardUI[1] { card }, GetDeckPosition(DeckEnum.Disposal),GetDeckPosition(DeckEnum.Hand)));


        }
        #endregion

        #region Touching Cards
        public void OnFirstTouch(in Vector2 touchPos)
        {
            if (ClickedCardUI == null)
                return;
            Debug.Log("Card UI: OnFirstTouch, scaling card");

            TryRemoveFromHandUI(ClickedCardUI);
        }

        public void OnReleaseTouch(in Vector2 touchPos)
        {
            if (ClickedCardUI == null)
                return;

            if (_isTryingToPlace) // placed on placement Destination
            {
                Debug.Log("Placed On");
                /*
                _handUI.Remove(ref GetClickedCardUI);
                _placementUI.GetCurrentSlot.SetImage(
                    Battles.Deck.DeckManager.Instance.GetCardFromDeck(Battles.Deck.DeckEnum.Placement)[_placementUI.GetCurrentSlot.GetSlotID].GetSetCard.GetCardImage);
            */
                _isTryingToPlace = false;
            }
            else
            {
                AddToHandUI(ClickedCardUI);

                if (ClickedCardUI.Inputs != null)
                    ClickedCardUI.Inputs.GetCanvasGroup.blocksRaycasts = true;

                ClickedCardUI.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, Time.deltaTime);
                ClickedCardUI = null;
            }



        }

        public void OnHoldTouch(in Vector2 touchPos)
        {
            if (ClickedCardUI != null)
            {
                ClickedCardUI.CardTranslations?.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
                ClickedCardUI.CardTranslations?.MoveCard(false, touchPos, _cardUISettings.GetCardFollowDelay);
            }
        }

        #endregion

    }



    public abstract class CardUITransition
    {
        protected CardUIManager _cardUIManager;
        protected SoundsEvent _soundEvent;
        protected CardUISO _cardSettings;
      
        public CardUITransition(CardUIManager cuim,CardUISO so, SoundsEvent soundEvent)
        {
            _cardUIManager = cuim;
            _soundEvent = soundEvent;
            _cardSettings = so;
        }
        public abstract IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination,Vector2 startPos);

    }

    public class DiscardHandHandler : CardUITransition
    {
        public DiscardHandHandler(CardUIManager cuim, CardUISO so, SoundsEvent soundEvent) : base(cuim, so, soundEvent)
        {
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination, Vector2 startPos)
        {
            if (cards == null || cards.Length == 0)
                yield break;


            for (int i = cards.Length - 1; i >= 0; i--)
            { 
                if (cards[i] == null)
                    continue;
       _cardUIManager.StartCoroutine(DiscardCard(cards[i], destination));

                yield return new WaitForSeconds(_cardSettings.DelayBetweenCardsIsDiscarded);

                _cardUIManager.TryRemoveFromHandUI(cards[i]);
            }

        }

        IEnumerator DiscardCard(CardUI card, Vector2 destination)
        {

            _soundEvent?.Raise(SoundsNameEnum.DisacrdCard);
            card.GFX?.SetAlpha(_cardSettings.EndDiscardAlphaAmount, _cardSettings.AlphaDiscardTime, _cardSettings.AlphaDiscardTweenType);
            var translation = card.CardTranslations;
            if (translation!= null)
            {
                translation.SetScale(Vector3.one * _cardSettings.DiscardEndScaleSize, _cardSettings.ScaleDiscardTime, _cardSettings.ScaleDiscardTweenType);

            yield return new WaitForSeconds(_cardSettings.DelayBetweenVisualAndMoving);
                translation.MoveCardX(destination.x, _cardSettings.DiscardTransitionXTime, null,_cardSettings.DiscardMoveOnYLeanTweenType);
                translation.MoveCardY(destination.y, _cardSettings.DiscardTransitionYTime,false,_cardSettings.DiscardMoveOnYLeanTweenType);

            }
       
        }
    }

    public class RemoveCardAfterACtivated : CardUITransition
    {
        public RemoveCardAfterACtivated(CardUIManager _manager, CardUISO so, SoundsEvent soundEvent) : base(_manager, so, soundEvent)
        {
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination, Vector2 startPos)
        {
            if (cards == null || cards.Length == 0)
                yield break;

    //CardUI card = ClickedCardUI;
    //            ClickedCardUI = null;
                InputManager.Instance.RemoveObjectFromTouch();
            for (int i = 0; i < cards.Length; i++)
            { 
                if (cards[i] == null)
                    continue;
                var translation = cards[i].CardTranslations;

                cards[i].GFX.SetAlpha(_cardSettings.GetAlphaRemovalAmount, _cardSettings.GetAlphaRemovalTime,_cardSettings.GetAlphaLeanTween);
                translation.SetScale(Vector3.one * _cardSettings.GetScaleSizeForRemoval, _cardSettings.GetRemovalTimeForRemoval, _cardSettings.GetScaleRemovalLeanTweenType);
                 yield return new WaitForSeconds(_cardSettings.GetDelayTillStartMovement);
                translation.MoveCardX(destination.x, _cardSettings.GetRemovalTransitionXTime, null, _cardSettings.GetMoveOnXLeanTween);
                translation.MoveCardY(destination.y, _cardSettings.GetRemovalTransitionYTime, false, _cardSettings.GetMoveOnYLeanTween);
            }
        }

    }


    public class CraftCardUIHandler : CardUITransition
    {
        HandUI _hand;
        public CraftCardUIHandler(HandUI hand,CardUIManager cuim, CardUISO so, SoundsEvent soundEvent) : base(cuim, so, soundEvent)
        {
            _hand = hand;
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination, Vector2 startPos)
        {
            throw new NotImplementedException();
        }
    }



    public class DrawCardUIHandler : CardUITransition
    {
        HandUI _hand;
        public DrawCardUIHandler(CardUIManager _manager,HandUI hand,SoundsEvent _soundEvent, CardUISO so) : base(_manager,so, _soundEvent)
        {
            _hand = hand;
        }

        public override IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination,Vector2 startPos)
        {
            // check if its valid array
            if (cards == null || cards.Length == 0)
                yield break;

            
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] == null)
                    continue;
                MoveCardUI(cards[i], destination);

                yield return new WaitForSeconds(_cardSettings.DelayTillDrawNextCard);

            }
        }

        private void MoveCardUI(CardUI card, Vector2 destination)
        {
            // cache rectTransform
            RectTransform cardUIRect = card.GFX.GetRectTransform;
            // disable glow
            card.GFX.GlowCard(false);
            // set their starting scale
            cardUIRect.localScale = Vector3.one * _cardSettings.StartScaleSize;

            //play sound
            _soundEvent?.Raise(SoundsNameEnum.DrawCard);

            //set their alpha
            card.GFX.SetAlpha(_cardSettings.StartAlphaAmount, 0.001f, LeanTweenType.notUsed, () => { card.GFX.SetAlpha(1, _cardSettings.AlphaDrawTime, _cardSettings.AlphaDrawTweenType); }); 
            //LeanTween.alpha(cardUIRect, _cardSettings.StartAlphaAmount, 0.001f).setOnComplete(() =>
            //LeanTween.alpha(cardUIRect, 1, _cardSettings.AlphaDrawTime).setEase(_cardSettings.AlphaDrawTweenType));

            // set 
            card.CardTranslations?.SetScale(Vector3.one, _cardSettings.ScaleDrawTime);
            // Move cards and then add it to hand
            card.CardTranslations?.MoveCardY(destination.y, _cardSettings.DrawTransitionYTime, null, _cardSettings.DrawMoveOnYLeanTweenType);
          //  card.CardTranslations?.MoveCardX(destination.x, _cardSettings.DrawTransitionXTime, null, _cardSettings.DrawMoveOnXLeanTweenType);
            LeanTween.moveX(cardUIRect, destination.x, _cardSettings.DrawTransitionXTime).setEase(_cardSettings.DrawMoveOnXLeanTweenType)
                .setOnComplete(() => _hand.Add(card));
        }
        
    }
}