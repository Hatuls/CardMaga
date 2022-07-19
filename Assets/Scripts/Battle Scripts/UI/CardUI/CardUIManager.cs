<<<<<<< HEAD
﻿using Battles.Deck;
=======
﻿
using Battle.Deck;
using Battle.UI.CardUIAttributes;
>>>>>>> WithOutMapScene
using Cards;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>
    {
        #region Field

        // [SerializeField] private DeckManager _deckManager;
        //[SerializeField] private RectTransform _middleHandPos;
        [SerializeField] private CardUIPool _cardPool;

        private List<CardUI> _handCards;
        private HandUI _handUI;

        [SerializeField]
        Art.ArtSO _artSO;
        #endregion

        #region Events
        [SerializeField]
        UnityEvent OnPlayerRemoveHand;
        [SerializeField]
        UnityEvent OnDrawCard;
        #endregion

        #region Properties

        [SerializeField]
        private CardUI _enemyCardUI;
        internal void UpdateHand()
        {
            GetCardsUI(DeckManager.Instance.GetCardsFromDeck(true, DeckEnum.Hand));
        }

        internal void PlayEnemyCard(Card card)
        {
            if (_enemyCardUI.gameObject.activeSelf == false)
                ActivateEnemyCardUI(true);
            AssignDataToCardUI(_enemyCardUI, card);

        }

        public void ActivateEnemyCardUI(bool state)
            => _enemyCardUI.gameObject.SetActive(state);

        #endregion


        #region Monobehaviour Callbacks 
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }

        #endregion

        #region Private Methods

        // internal void CraftCardUI(Card addedCard, DeckEnum toBaseDeck)
        // {
        //     if (toBaseDeck == DeckEnum.Hand)
        //         AssignDataToCardUI(_handUI.CurrentlyHolding, addedCard);
        //
        // }

        public void AssignDataToCardUI(CardUI card, Cards.Card cardData)
        {
            card.AssignCard(cardData);
        }

        #endregion


        #region Public Methods

        public CardUI[] GetCardsUI(params Card[] cardData)
        {
            if (cardData == null)
            {
                throw new Exception(name + " CardData is null");
            }

            List<CardUI> tempCardUI = new List<CardUI>();

            for (int i = 0; i < cardData.Length; i++)
            {
                if (cardData[i] == null)
                {
                    Debug.LogError(name + " CardData in index " + i + " in null");
                }

                CardUI cache = _cardPool.Pull();

                AssignDataToCardUI(cache, cardData[i]);

                tempCardUI.Add(cache);
            }

            // for (int i = 0; i < cardData.Length; i++)
            // {
            //     var card = _handUI.GetHandCardUIFromIndex(i);
            //
            //     if (cardData[i] == null)
            //     {
            //         string cardsDrawn = "";
            //         for (int j = 0; j < cardData.Length; j++)
            //         {
            //             if (cardData[j] != null)
            //                 cardsDrawn += cardData[j].ToString() + " " + cardData[j].CardSO.CardName + "\n";
            //             else
            //                 cardsDrawn += "Card at index " + i + " Is null!\n";
            //         }
            //         Debug.LogError($"Drawn Card is null!!\n {cardsDrawn} ,\n");
            //     }
            // }

            return tempCardUI.ToArray();
        }


        public void RemoveHands()
        {
            //_handUI.DiscardHand();
            OnPlayerRemoveHand?.Invoke();
        }

        public override void Init(ITokenReciever token)
        {
            using (token.GetToken())
            {
                //_handUI = new HandUI(_middleHandPos.rect.position);
            }
        }

        public void ExecuteCardUI(CardUI card)
        {
            if (card == null)
                return;
        }

        #endregion
    }
<<<<<<< HEAD
=======










    public class CardUIHandler
    {
        public static Action<Vector2, CardUI> OnExecuteCardUI;
        public static CardUIHandler Instance;
        public CardUI _selectedCardUI;
        CardUISO _cardUISettings;
        CardUI _OriginalCard;
        HandUI _handUI;


        public CardUIHandler(HandUI hand, CardUI firstCardUI, CardUISO cardUISettings)
        {
            Instance = this;
            _handUI = hand;
            _selectedCardUI = firstCardUI;
            this._cardUISettings = cardUISettings;
            _selectedCardUI.gameObject.SetActive(false);
            Turns.TurnHandler.OnFinishTurn += OnFinishTurn;
        }

        ~CardUIHandler()
        {
            Turns.TurnHandler.OnFinishTurn -= OnFinishTurn;

        }

        private void OnFinishTurn()
        {

            CardUITouchedReleased(false, null);
        }
        internal void CardUITouched(CardUI cardReference)
        {
            if (cardReference == _selectedCardUI || BattleManager.isGameEnded)
                return;



            DeckManager.Instance.TransferCard(true, DeckEnum.Hand, DeckEnum.Selected, cardReference.GFX.GetCardReference);
            _handUI.ReplaceCard(_selectedCardUI, cardReference);

            _selectedCardUI.gameObject.SetActive(true);
            cardReference.GFX.GlowCard(true);
            GameEventsInvoker.Instance.OnSelectCard?.Invoke();
            _selectedCardUI = cardReference;
            _handUI.LockCardsInput(true);


        }
        internal void CardUITouchedReleased(bool ExecuteSucceded, CardUI cardReference)
        {

            var card = _selectedCardUI;
            if (card == null)
                return;

            card?.GFX.GlowCard(false);

            if (ExecuteSucceded == false)   
                DeckManager.Instance.TransferCard(true, DeckEnum.Selected, DeckEnum.Hand, _selectedCardUI.GFX.GetCardReference);

            InputManager.Instance.RemoveObjectFromTouch();
            card.CardAnimator.ResetAllAnimations();
            cardReference?.CardAnimator.ResetAllAnimations();
            card.CardTranslations.SetScale(Vector2.one, 0);
            cardReference?.CardTranslations.SetScale(Vector2.one, 0);
            _handUI.LockCardsInput(false);
            cardReference?.GFX.GlowCard(false);
            card.gameObject.SetActive(false);
            card.CardStateMachine.MoveToState(CardStateMachine.CardUIInput.None);

            if (ExecuteSucceded) DeckManager.Instance.DrawHand(true, 1);
            // card.CardTranslations.CancelAllTweens();
            //     _selectedCardUI = null;

        }

        internal void ToZoomCardUI()
        {
            var card = _selectedCardUI;
            if (card == null)
                return;

            GameBattleDescriptionUI.Instance.CheckCardUI(card);
            GameEventsInvoker.Instance.OnZoomCard?.Invoke();
            //     card.Inputs.GetCanvasGroup.blocksRaycasts = false;
            // card.CardTranslations?.SetPosition( Vector2.zero);

            // card.CardTranslations?.MoveCard(false, Vector2.zero, _cardUISettings.GetCardScaleDelay);
            card.CardAnimator.ScaleAnimation(true);
            card.CardTranslations?.SetRotation(0, _cardUISettings.RotationTimer);
            card.GFX.GlowCard(true);
        }

        internal void ToUnZoomCardUI(in Vector2 location)
        {
            var card = _selectedCardUI;
            if (card == null)
                return;
            GameBattleDescriptionUI.Instance.CloseCardUIInfo();
            card.GFX.GlowCard(false);

            card.CardAnimator.ScaleAnimation(false);

        }


        internal bool TryExecuteCardUI(CardUI thisCardUI)
        {
            if (BattleManager.isGameEnded)
                return false;
            bool succeded = false;
            var card = DeckManager.Instance.GetCardFromDeck(true, 0, DeckEnum.Selected);
            if (card != null)
            {

                 succeded = CardExecutionManager.Instance.TryExecuteCard(true, card);
                if (succeded)
                {
                    //              _OriginalCard.Inputs.InHandInputState.HasValue = false;

                    UnityAnalyticHandler.SendEvent("Card Played", new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"Card", card.CardSO.CardName},
                        {"Level", card.CardLevel},
                    });

                    OnExecuteCardUI?.Invoke(_selectedCardUI.transform.localPosition, _OriginalCard);
                    int index = _handUI.GetCardIndex(_selectedCardUI);
                }
     
            }
            else
            {
                OnFinishTurn();
                Debug.LogError("CardUIHandler - Selected Card  is null !");
            }
          
           return succeded;
        }

    }

















    //public abstract class CardUITransition
    //{
    //    protected CardUIManager _cardUIManager;
    //    protected SoundsEvent _soundEvent;
    //    protected CardUISO _cardSettings;

    //    public CardUITransition(CardUIManager cuim,CardUISO so, SoundsEvent soundEvent)
    //    {
    //        _cardUIManager = cuim;
    //        _soundEvent = soundEvent;
    //        _cardSettings = so;
    //    }
    //    public abstract IEnumerator MoveCardsUI(CardUI[] cards, Vector2 destination,Vector2 startPos);

    //}

>>>>>>> WithOutMapScene
}