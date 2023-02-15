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

    public class HandUI : InputBehaviourHandler<BattleCardUI>, ISequenceOperation<IBattleUIManager>
    {
        #region Events
        public event Action OnCardExecutionFailed;
        public event Action OnCardExecutionSuccess;
        public static event Action<BattleCardUI> OnCardExecute;
        public static event Action OnCardDrawnAndAlign;
        public static event Action OnDiscardAllCards;
        public static event Action<BattleCardUI> OnCardSetToHandState;

        public static event Action<IReadOnlyList<BattleCardUI>>
            OnCardsAddToHand; //when new cards are added to the hand, passing the cards

        public static event Action<IReadOnlyList<BattleCardUI>>
            OnCardsExecuteGetCards; //when battleCard execute, and passes all the cards that are in the hand

        public event Action<BattleCardUI> OnCardSelect;
        public event Action OnHandUICardsUpdated;
        #endregion

        #region Field

        [Header("TransitionPackSO")]

        [SerializeField] private TransitionPackSO _discardMoveTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardScaleTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardExecutionMoveTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardExecutionScaleTransitionPackSo;


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
        private EndBattleHandler _endBattleHandler;
        #endregion

        #region Prop

        public IReadOnlyList<BattleCardUI> GetCardUIFromHand()
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
            HandUIState.OnCardDrawnAndAlign += UnLockInput;
        }

        private void OnDestroy()
        {
            _comboUIManager.OnCardComboDone -= GetCardsFromCombo;
            BattleManager.OnGameEnded -= DiscardAllCards;
            HandUIState.OnCardDrawnAndAlign -= UnLockInput;

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
            LockAndUnlockSystem.Instance.ChangeTouchableItemsState(_handUIState.CardUIsInput, true);
        }

        #endregion

        #region Transitions

        private void SetCardAtDrawPos(params BattleCardUI[] cards)
        {
            for (var i = 0; i < cards.Length; i++)
            {
               // Debug.Log(cards[i].BattleCardData.CardInstance.InstanceID);
                BattleCardUI battleCardUI = cards[i];
                battleCardUI.DOKill(false);
                battleCardUI.RectTransform.SetPosition(_drawPos);
                battleCardUI.VisualsRectTransform.SetScale(0.1f);
            }
        }

        private IEnumerator MoveCardToTheDiscardPosition(params BattleCardUI[] cardUI)
        {
            for (var i = 0; i < cardUI.Length; i++)
            {
                if (cardUI[i] == null)
                    continue;

                Transition(cardUI[i]);
                yield return _waitForCardDiscardDelay;
            }

            void Transition(BattleCardUI card)
            {
                TokenMachine tokenMachine = new TokenMachine(card.Dispose);
                IDisposable token = tokenMachine.GetToken();
                DOTween.Kill(card, false);
                card.VisualsRectTransform.Transition(_discardScaleTransitionPackSo)
                .Join(card.RectTransform.Transition(_discardPos, _discardMoveTransitionPackSo))
                .OnComplete(token.Dispose);
            }
        }

        private void MoveCardToDiscardAfterExecute(BattleCardUI battleCardUI)
        {
            //   cardUI.RectTransform.Transition(_discardPos, _dicardExecuteTransitionPackSo, cardUI.Dispose);
            var sequence = battleCardUI.VisualsRectTransform.Transition(_discardExecutionScaleTransitionPackSo)
            .Join(battleCardUI.RectTransform.Transition(_discardPos, _discardExecutionMoveTransitionPackSo))
            .OnComplete(battleCardUI.Dispose);
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }

        #endregion

        #region HandStateManagnent

        public void SetToZoomState(BattleCardUI battleCardUI)
        {
            SetState(InputBehaviourState.HandZoom, battleCardUI);
            OnCardSelect?.Invoke(battleCardUI);
        }

        public void SetToFollowState(BattleCardUI battleCardUI)
        {
            SetState(InputBehaviourState.HandFollow, battleCardUI);
            OnCardSelect?.Invoke(battleCardUI);
        }

        public void SetToHandState(BattleCardUI battleCardUI)
        {
            SetState(InputBehaviourState.Hand, battleCardUI);
            OnCardSetToHandState?.Invoke(battleCardUI);
        }

        #endregion

        #region CardUIManagnent

        private void DrawCardsFromDrawDeck(params BattleCardData[] cards)
        {
            if (_endBattleHandler.IsGameEnded)
                return;

            BattleCardUI[] handCards = GetCardsUI(cards);

            SetCardAtDrawPos(handCards);

            for (int i = 0; i < handCards.Length; i++)
            {
                //_handCards[i].Inputs.ForceResetInputBehaviour();
                BattleCardUI battleCardUI = handCards[i];
                battleCardUI.Inputs.Lock();
                battleCardUI.Init();
                battleCardUI.DOKill(false);
                SetState(InputBehaviourState.Hand, battleCardUI);
            }

            // OnCardsAddToHand?.Invoke(handCards);
            OnHandUICardsUpdated?.Invoke();
        }

        private void GetCardsFromCombo(params BattleCardUI[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
                cards[i].Init();

            OnCardsAddToHand?.Invoke(cards);

            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].DOKill(false);
                //cards[i].Inputs.ForceResetInputBehaviour();
                SetState(InputBehaviourState.Hand, cards[i]);
            }
            OnCardsAddToHand?.Invoke(cards);
            OnHandUICardsUpdated?.Invoke();
        }

        private BattleCardUI[] GetCardsUI(params BattleCardData[] cardDatas)
        => _cardUIManager.GetCardsUI(cardDatas);

        public bool TryExecuteCard(BattleCardUI battleCardUI)
        {
            bool canPlayCard = _cardExecutionManager.CanPlayCard(true, battleCardUI.BattleCardData);
            
            if (canPlayCard)
            {
                _deckHandler.TransferCard(DeckEnum.Hand, DeckEnum.Selected, battleCardUI.BattleCardData);
                _handUIState.RemoveCardUI(battleCardUI);

                if (_cardExecutionManager.TryExecuteCard(battleCardUI))
                {
                    DiscardCardAfterExecute(battleCardUI);
                    OnCardExecute?.Invoke(battleCardUI);
                    OnHandUICardsUpdated?.Invoke();
                    OnCardExecutionSuccess?.Invoke();
                    return canPlayCard; // true
                }
            }

            OnCardExecutionFailed?.Invoke();
            SetToHandState(battleCardUI);
            return canPlayCard; // false
        }

        private void DiscardAllCards()
        {
            BattleCardUI[] handCardUis = _handUIState.RemoveAllCardUIFromHand();
            BattleCardUI[] combineCardUis = new BattleCardUI[handCardUis.Length + 1];

            Array.Copy(handCardUis, combineCardUis, handCardUis.Length);

            BattleCardUI tempZoom = _zoomUIState.ForceExitState();
            BattleCardUI tempFollow = _followUIState.ForceExitState();

                int lengthsMinusOne = combineCardUis.Length - 1;
            if (tempZoom != null)
            {
                if (combineCardUis[lengthsMinusOne] == null)
                    combineCardUis[lengthsMinusOne] = tempZoom;
                tempZoom.Inputs.ForceResetInputBehaviour();
            }
            if (tempFollow != null)
            {
                if (combineCardUis[lengthsMinusOne] == null)
                    combineCardUis[lengthsMinusOne] = tempFollow;
                tempFollow.Inputs.ForceResetInputBehaviour();
            }

            StartCoroutine(MoveCardToTheDiscardPosition(combineCardUis));
        }

        private void DiscardCardAfterExecute(BattleCardUI battleCardUI)
        {
            if (battleCardUI == null)
                return;

            //SetState(InputBehaviourState.Hand,cardUI);
            _currentSequence.Kill(battleCardUI);
            battleCardUI.DOKill(false);
            battleCardUI.Inputs.ForceResetInputBehaviour();
            MoveCardToDiscardAfterExecute(battleCardUI);
            battleCardUI.CardVisuals.SetExecutedCardVisuals();
            //OnCardsExecuteGetCards?.Invoke(_tableCardSlot.GetCardUIsFromTable());
        }

        public void ExecuteTask(ITokenReceiver tokenMachine, IBattleUIManager battleUIManager)
        {
            var data = battleUIManager.BattleDataManager;
            _deckHandler = data.PlayersManager.GetCharacter(true).DeckHandler;
            _deckHandler.OnDrawCards += DrawCardsFromDrawDeck;
            _leftPlayerGameTurn = data.TurnHandler.GetCharacterTurn(true);
            _leftPlayerGameTurn.EndTurnOperations.Register((x) => DiscardAllCards(), 0, OrderType.Before);
            _cardExecutionManager = data.CardExecutionManager;
            _endBattleHandler = data.EndBattleHandler;
        }

        #endregion
    }
    #endregion
}




