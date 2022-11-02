using Battle.Deck;
using Battle.Turns;
using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.UI;
using CardMaga.Card;
using CardMaga.Input;
using CardMaga.SequenceOperation;
using CardMaga.UI.Card;
using DG.Tweening;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI
{
    #region HandUI

    public class HandUI : InputBehaviourHandler<CardUI>, ISequenceOperation<IBattleUIManager>
    {
        #region Events

        public static event Action<CardUI> OnCardExecute;
        public static event Action OnCardDrawnAndAlign;
        public static event Action OnDiscardAllCards;
        public static event Action<CardUI> OnCardSetToHandState;

        public static event Action<IReadOnlyList<CardUI>>
            OnCardsAddToHand; //when new cards are added to the hand, passing the cards

        public static event Action<IReadOnlyList<CardUI>>
            OnCardsExecuteGetCards; //when card execute, and passes all the cards that are in the hand

        public event Action<CardUI> OnCardSelect;
        public event Action OnHandUICardsUpdated;
        #endregion

        #region Field

        [Header("TransitionPackSO")]

        [SerializeField] private TransitionPackSO _discardMoveTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardScaleTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardExecutionMoveTransitionPackSo;


        [Header("RectTransforms")]
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;

        [Header("Scripts Reference")]
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private ComboUIManager _comboUIManager;
        [SerializeField] private ZoomCardUI _zoomUIState;
        [SerializeField] private FollowCardUI _followUIState;
        [SerializeField] private HandUIState _handUIState;

        [Header("Parameters")]
        [SerializeField] private float _delayBetweenCardDiscard;

        private CardExecutionManager _cardExecutionManager;
        private WaitForSeconds _waitForCardDiscardDelay;
        private DG.Tweening.Sequence _currentSequence;
        private DeckHandler _deckHandler;
        private GameTurn _leftPlayerGameTurn;

        #endregion

        #region Prop

        public IReadOnlyList<CardUI> GetCardUIFromHand()
        {
            return _handUIState.CardsUI;
        }

        public int Priority => 0;

        public HandUIState HandUIState { get => _handUIState; }

        public ZoomCardUI ZoomCardUI { get => _zoomUIState; }

        #endregion

        #region UnityCallBack

        private void Awake()
        {
            _waitForCardDiscardDelay = new WaitForSeconds(_delayBetweenCardDiscard);

            _handUIStates = new Dictionary<InputBehaviourState, BaseHandUIState>()
            {
                {InputBehaviourState.HandZoom, _zoomUIState}, {InputBehaviourState.HandFollow, _followUIState},
                { InputBehaviourState.Hand, _handUIState } ,{ InputBehaviourState.Default ,null }
            };

            _comboUIManager.OnCardComboDone += GetCardsFromCombo;
            BattleManager.OnGameEnded += DiscardAllCards;
            _handUIState.OnCardDrawnAndAlign += UnLockInput;
        }

        private void OnDestroy()
        {
            _comboUIManager.OnCardComboDone -= GetCardsFromCombo;
            BattleManager.OnGameEnded -= DiscardAllCards;
            _handUIState.OnCardDrawnAndAlign -= UnLockInput;

            if (_leftPlayerGameTurn != null)
                _leftPlayerGameTurn.OnTurnExit -= DiscardAllCards;
            if (_deckHandler != null)
                _deckHandler.OnDrawCards -= DrawCardsFromDrawDeck; //need to check rei

            KillTween();
        }

        #endregion

        #region InputIdentificationSO

        public void LockInput()
        {
            LockAndUnlockSystem.Instance.ChangeTouchableItemsState(_handUIState.CardUIsInput, false);
        }

        public void UnLockInput()
        {
            LockAndUnlockSystem.Instance.ChangeTouchableItemsState(_handUIState.CardUIsInput,true);
        }

        #endregion

        #region Transitions

        private void SetCardAtDrawPos(params CardUI[] cards)
        {
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].RectTransform.SetPosition(_drawPos);
                cards[i].VisualsRectTransform.SetScale(0.1f);
            }
        }

        private IEnumerator MoveCardToTheDiscardPosition(params CardUI[] cardUI)
        {
            for (var i = 0; i < cardUI.Length; i++)
            {
                if (cardUI[i] == null)
                    continue;

                var sequence = cardUI[i].VisualsRectTransform.Transition(_discardScaleTransitionPackSo);
                sequence.Join(cardUI[i].RectTransform.Transition(_discardPos, _discardMoveTransitionPackSo));
                sequence.OnComplete(cardUI[i].Dispose);
                yield return _waitForCardDiscardDelay;
            }
        }

        private void MoveCardToDiscardAfterExecute(CardUI cardUI)
        {
            //   cardUI.RectTransform.Transition(_discardPos, _dicardExecuteTransitionPackSo, cardUI.Dispose);
            var sequence = cardUI.VisualsRectTransform.Transition(_discardScaleTransitionPackSo);
            sequence.Join(cardUI.RectTransform.Transition(_discardPos, _discardExecutionMoveTransitionPackSo)); sequence.OnComplete(cardUI.Dispose);
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }

        #endregion

        #region HandStateManagnent

        public void SetToZoomState(CardUI cardUI)
        {
            SetState(InputBehaviourState.HandZoom, cardUI);
            OnCardSelect?.Invoke(cardUI);
        }

        public void SetToFollowState(CardUI cardUI)
        {
            SetState(InputBehaviourState.HandFollow, cardUI);
            OnCardSelect?.Invoke(cardUI);
        }

        public void SetToHandState(CardUI cardUI)
        {
            SetState(InputBehaviourState.Hand, cardUI);
            OnCardSetToHandState?.Invoke(cardUI);
        }

        #endregion

        #region CardUIManagnent

        private void DrawCardsFromDrawDeck(params CardData[] cards)
        {
            CardUI[] handCards;

            handCards = GetCardsUI(cards);

            SetCardAtDrawPos(handCards);

            for (int i = 0; i < handCards.Length; i++)
            {
                //_handCards[i].Inputs.ForceResetInputBehaviour();
                handCards[i].Init();
                SetState(InputBehaviourState.Hand, handCards[i]);
            }

           // OnCardsAddToHand?.Invoke(handCards);
            OnHandUICardsUpdated?.Invoke();
        }

        private void GetCardsFromCombo(params CardUI[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
                cards[i].Init();

            OnCardsAddToHand?.Invoke(cards);

            for (int i = 0; i < cards.Length; i++)
            {
                //cards[i].Inputs.ForceResetInputBehaviour();
                SetState(InputBehaviourState.Hand, cards[i]);
            }
            OnCardsAddToHand?.Invoke(cards);
            OnHandUICardsUpdated?.Invoke();
        }

        private CardUI[] GetCardsUI(params CardData[] cardDatas)
        => _cardUIManager.GetCardsUI(cardDatas);

        public bool TryExecuteCard(CardUI cardUI)
        {
            bool canPlayCard = _cardExecutionManager.CanPlayCard(true, cardUI.CardData);


            if (canPlayCard)
            {
                _deckHandler.TransferCard(DeckEnum.Hand, DeckEnum.Selected, cardUI.CardData);
                _handUIState.RemoveCardUI(cardUI);

                if (_cardExecutionManager.TryExecuteCard(cardUI))
                {
                    DiscardCardAfterExecute(cardUI);
                    OnCardExecute?.Invoke(cardUI);
                    OnHandUICardsUpdated?.Invoke();
                    return true;
                }
            }

            SetState(InputBehaviourState.Hand, cardUI);
            return false;
        }

        private void DiscardAllCards()
        {
            CardUI[] handCardUis = _handUIState.RemoveAllCardUIFromHand();
            CardUI[] combineCardUis = new CardUI[handCardUis.Length + 1];

            Array.Copy(handCardUis, combineCardUis, handCardUis.Length);

            CardUI tempZoom = _zoomUIState.ForceExitState();
            CardUI tempFollow = _followUIState.ForceExitState();

            if (tempZoom != null)
            {
                if (combineCardUis[combineCardUis.Length - 1] == null)
                    combineCardUis[combineCardUis.Length - 1] = tempZoom;
                tempZoom.Inputs.ForceResetInputBehaviour();
            }
            if (tempFollow != null)
            {
                if (combineCardUis[combineCardUis.Length - 1] == null)
                    combineCardUis[combineCardUis.Length - 1] = tempFollow;
                tempFollow.Inputs.ForceResetInputBehaviour();
            }

            StartCoroutine(MoveCardToTheDiscardPosition(combineCardUis));
        }

        private void DiscardCardAfterExecute(CardUI cardUI)
        {
            if (cardUI == null)
                return;

            //SetState(InputBehaviourState.Hand,cardUI);
            cardUI.Inputs.ForceResetInputBehaviour();
            MoveCardToDiscardAfterExecute(cardUI);
            cardUI.CardVisuals.SetExecutedCardVisuals();
            //OnCardsExecuteGetCards?.Invoke(_tableCardSlot.GetCardUIsFromTable());
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
        {
            var data = battleUIManager.BattleDataManager;
            _deckHandler = data.PlayersManager.GetCharacter(true).DeckHandler;
            _deckHandler.OnDrawCards += DrawCardsFromDrawDeck;
            _leftPlayerGameTurn = data.TurnHandler.GetCharacterTurn(true);
            _leftPlayerGameTurn.EndTurnOperations.Register((x) => DiscardAllCards(), 0, OrderType.Before);
            _cardExecutionManager = data.CardExecutionManager;
        }

        #endregion
    }
    #endregion
}




