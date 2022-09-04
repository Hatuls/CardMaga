using Battle;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Battle.UI;
using CardMaga.Card;
using CardMaga.Input;
using CardMaga.UI.Card;
using DG.Tweening;
using Managers;
using ReiTools.TokenMachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.UI
{
    #region HandUI

    public class HandUI : BaseHandUIState, ILockable, IGetCardsUI, ISequenceOperation<BattleManager>
    {
        #region Events

        public static event Action OnCardDrawnAndAlign;
        public static event Action OnDiscardAllCards;
        public static event Action<CardUI> OnCardSelect;
        public static event Action<CardUI> OnCardReturnToHand;

        public static event Action<IReadOnlyList<CardUI>>
            OnCardsAddToHand; //when new cards are added to the hand, passing the cards

        public static event Action<IReadOnlyList<CardUI>>
            OnCardsExecuteGetCards; //when card execute, and passes all the cards that are in the hand


        #endregion

        #region Fields

        [Header("TransitionPackSO")] 
        [SerializeField] private TransitionPackSO _drawMoveTransitionPackSo;
        [SerializeField] private TransitionPackSO _drawScaleTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardMoveTransitionPackSo;
        [SerializeField] private TransitionPackSO _discardScaleTransitionPackSo;
        [SerializeField] private TransitionPackSO _reAlignTransitionPackSo;
        [SerializeField] private TransitionPackSO _resetCardPositionPackSO;
        [SerializeField] private TransitionPackSO _dicardExecutePositionPackSO;
        
        [Header("RectTransforms")] 
        [SerializeField] private RectTransform _discardPos;
        [SerializeField] private RectTransform _drawPos;

        [Header("Scripts Reference")]
        [SerializeField] private CardUIManager _cardUIManager;
        [SerializeField] private ComboUIManager _comboUIManager;
        [SerializeField] private TableCardSlot _tableCardSlot;
        [SerializeField] private CardUiInputBehaviourHandler _cardUiInputBehaviourHandler;

        [Header("Parameters")]
        [SerializeField] private float _delayBetweenCardDrawn;
        [SerializeField] private float _delayBetweenCardDiscard;

        private Sequence _currentSequence;
        private DeckHandler _deckHandler;
        private bool _isCardSelected;
        private WaitForSeconds _waitForCardDiscardDelay;
        private GameTurn _leftPlayerGameTurn;
        private WaitForSeconds _waitForCardDrawnDelay;

        #endregion

        #region Prop

        public bool IsCardSelect
        {
            get => _isCardSelected;
        }

        public IReadOnlyList<CardUI> CardsUI
        {
            get => _tableCardSlot.GetCardUIsFromTable();
        }

        public int Priority => 0;

        #endregion

        #region UnityCallBack

        private void Awake()
        {
            _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);
            _waitForCardDiscardDelay = new WaitForSeconds(_delayBetweenCardDiscard);

            BattleManager.Register(this, OrderType.After);
            _comboUIManager.OnCardComboDone += GetCardsFromCombo;
            BattleManager.OnGameEnded += ForceDiscardCards;
  
            FollowCardUI.OnCardExecute += DiscardCard;
            _isCardSelected = false;
        }

        private void Start()
        {
            _inputBehaviour.OnClick += SetToZoomState;
            _inputBehaviour.OnBeginHold += SetToFollowState;

            CardUI[] temp = _tableCardSlot.GetCardUIsFromTable();
            
            for (int i = 0; i < temp.Length; i++)
            {
                _cardUiInputBehaviourHandler.SetState(CardUiInputBehaviourHandler.HandState.Hand,temp[i]);   
            }
        }

        private void OnDestroy()
        {
            _inputBehaviour.OnClick -= SetToZoomState;
            _inputBehaviour.OnBeginHold -= SetToFollowState;
            
            _comboUIManager.OnCardComboDone -= GetCardsFromCombo;
            BattleManager.OnGameEnded -= ForceDiscardCards;
            FollowCardUI.OnCardExecute -= DiscardCard;
            
            if(_leftPlayerGameTurn!=null)
                _leftPlayerGameTurn.OnTurnExit -= ForceDiscardCards;
            if(_deckHandler != null)
                _deckHandler.OnDrawCards -= DrawCardsFromDrawDeck;
        }

        #endregion

        #region Input

        public void LockInput()
        {
            _cardUiInputBehaviourHandler.LockAllTouchableItems(_tableCardSlot.GetCardUIInputsFromTable(),false);
        }

        public void UnLockInput()
        {
            _cardUiInputBehaviourHandler.LockAllTouchableItems(_tableCardSlot.GetCardUIInputsFromTable(),true);
        }

        #endregion

        #region Transitions

        private IEnumerator MoveCardsToHandPos(IReadOnlyList<CardSlot> cardSlots, Action onComplete = null)
        {
            for (var i = 0; i < cardSlots.Count; i++)
            {
                cardSlots[i].CardUI.Init();
                cardSlots[i].CardUI.transform.SetAsLastSibling();
                cardSlots[i].CardUI.RectTransform
                    .Transition(cardSlots[i].CardPos, _drawMoveTransitionPackSo, UnLockInput); //Plaster!!!
                cardSlots[i].CardUI.VisualsRectTransform.Transition(_drawScaleTransitionPackSo);
                yield return _waitForCardDrawnDelay;
            }

            onComplete?.Invoke();
        }

        private IEnumerator MoveCardToTheDiscardPosition(params CardUI[] cardUI)
        {
            for (var i = 0; i < cardUI.Length; i++)
            {
                cardUI[i].RectTransform.Transition(_discardPos, _discardMoveTransitionPackSo, cardUI[i].Dispose);
                cardUI[i].VisualsRectTransform.Transition(_discardScaleTransitionPackSo);
                yield return _waitForCardDiscardDelay;
            }
        }

        private void ReAlignCardUI(IReadOnlyList<CardSlot> cardSlots)
        {
            for (var i = 0; i < cardSlots.Count; i++)
                cardSlots[i].CardUI.RectTransform.Transition(cardSlots[i].CardPos, _reAlignTransitionPackSo);
        }

        private void ResetCardPosition(CardUI cardUI)
        {
            KillTween();
            Sequence temp = cardUI.RectTransform
                .Transition(_tableCardSlot.GetCardSlotFrom(cardUI).CardPos, _resetCardPositionPackSO);
        }

        private void SetCardAtDrawPos(params CardUI[] cards)
        {
            for (var i = 0; i < cards.Length; i++)
            {
                cards[i].RectTransform.SetPosition(_drawPos);
                cards[i].VisualsRectTransform.SetScale(0.1f);
            }
        }

        private void MoveCardToDiscardAfterExecute(CardUI cardUI)
        {
            cardUI.RectTransform.Transition(_discardPos, _discardMoveTransitionPackSo, cardUI.Dispose);
        }

        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }

        #endregion

        #region HandStateManagnent
        
        public override void ExitState(CardUI cardUI)
        {
            if (_isCardSelected)
                return;

            if (_tableCardSlot.ContainCardUIInSlots(cardUI, out CardSlot cardSlot))
            {
                cardUI.transform.SetAsLastSibling();
                _tableCardSlot.RemoveCardUI(cardUI);
                LockInput();
                _isCardSelected = true;
                OnCardSelect?.Invoke(cardUI);
            }
            
            base.ExitState(cardUI);
        }

        public override void EnterState(CardUI cardUI)
        {
            base.EnterState(cardUI);
            
            if (cardUI != null && !_tableCardSlot.ContainCardUIInSlots(cardUI, out CardSlot cardSlot))
            {
                _tableCardSlot.AddCardUIToCardSlot(cardUI);
                UnLockInput();
                ResetCardPosition(cardUI);
                OnCardReturnToHand?.Invoke(cardUI);
                _isCardSelected = false;
            }
        }

        
        private void SetToZoomState(CardUI cardUI)
        {
            _cardUiInputBehaviourHandler.SetState(CardUiInputBehaviourHandler.HandState.Zoom,cardUI);
        }

        private void SetToFollowState(CardUI cardUI)
        {
            _cardUiInputBehaviourHandler.SetState(CardUiInputBehaviourHandler.HandState.Follow,cardUI);
        }

        #endregion
        
        #region CardUIManagnent
        
        private void DrawCardsFromDrawDeck(params CardData[] cards)
        {
            CardUI[] _handCards;

            _handCards = GetCardsUI(cards);

            foreach (var cardUI in _handCards)
            {
                _cardUiInputBehaviourHandler.SetState(CardUiInputBehaviourHandler.HandState.Hand,cardUI);
            }
            
            OnCardsAddToHand?.Invoke(_handCards);
            AddCards(_handCards);
            SetCardAtDrawPos(_handCards);
            //ReAlignCardUI(_tableCardSlot.GetCardSlotsExceptFrom(_handCards));
            StartCoroutine(MoveCardsToHandPos(_tableCardSlot.GetCardSlotsFrom(_handCards), OnCardDrawnAndAlign));
        }

        private void GetCardsFromCombo(params CardUI[] cards)
        {
            OnCardsAddToHand?.Invoke(cards);
            AddCards(cards);
            //ReAlignCardUI(_tableCardSlot.GetCardSlotsExceptFrom(cards));
            StartCoroutine(MoveCardsToHandPos(_tableCardSlot.GetCardSlotsFrom(cards), OnCardDrawnAndAlign));
        }

        private CardUI[] GetCardsUI(params CardData[] cardDatas)
        {
            CardUI[] _handCards = _cardUIManager.GetCardsUI(cardDatas);

            return _handCards;
        }

        private void AddCards(params CardUI[] cards)
        {
            _tableCardSlot.AddCardUIToCardSlot(cards);
        }
        
        public void ForceReturnCardUIToHand(CardUI cardUI)
        {
            if (cardUI == null)
                return;

            _tableCardSlot.AddCardUIToCardSlot(cardUI);
            ResetCardPosition(cardUI);
            OnCardReturnToHand?.Invoke(cardUI);
            //DeckManager.Instance.TransferCard(true, DeckEnum.Selected, DeckEnum.Hand, cardUI.CardData);
            _isCardSelected = false;
        }

        private void DiscardCard(CardUI cardUI)
        {
            if (cardUI == null)
                return;

            cardUI.CardVisuals.SetExecutedCardVisuals();
            MoveCardToDiscardAfterExecute(cardUI);
            _isCardSelected = false;
            OnCardsExecuteGetCards?.Invoke(_tableCardSlot.GetCardUIsFromTable());
            //OnCardReturnToHand?.Invoke(cardUI);
        }

        private void DiscardCards(params CardUI[] cardUI)
        {
            StartCoroutine(MoveCardToTheDiscardPosition(cardUI));
        }

        private void ForceDiscardCards()
        {
            //_battleInputStateMachine.ForceChangeState(_lockState);

            CardUI[] tempCardUis = _tableCardSlot.GetCardUIsFromTable();

            _tableCardSlot.RemoveAllCardUI();
            OnDiscardAllCards?.Invoke();
            DiscardCards(tempCardUis);
        }

        public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
        {
            _deckHandler = data.PlayersManager.GetCharacter(true).DeckHandler;
        //    _deckHandler.OnDrawCards += DrawCardsFromDrawDeck;
            _leftPlayerGameTurn = data.TurnHandler.GetCharacterTurn(true);
            _leftPlayerGameTurn.EndTurnOperations.Register((x) => ForceDiscardCards(), 0, OrderType.Before);
        }

        #endregion
    }

    #endregion

    #region TableCardSlot

    [Serializable]
    public class TableCardSlot
    {
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] [Range(0, 25)] private float _cardPaddingPrecentage;
        [SerializeField] [Range(-10, 10)] private float _cardCenterOffSetPrecentage;
        [SerializeField] [ReadOnly] private List<CardSlot> _cardSlots = new List<CardSlot>();

        public IReadOnlyList<CardSlot> CardSlots => _cardSlots;


        private void AlignCardsSlots()
        {
            for (var i = 0; i < _cardSlots.Count; i++) _cardSlots[i].CardPos = CalculateCardPosition(i);
        }

        private Vector2 CalculateCardPosition(int index)
        {
            Vector2 startPos = _middleHandPos.position;

            float screenWidth = Screen.width;
            var screenBoundInPercentage = screenWidth * (_cardPaddingPrecentage / 100);
            var spaceBetweenCard = (screenWidth - screenBoundInPercentage) / _cardSlots.Count;
            var offSetFromCenter = screenWidth / 2 * (_cardCenterOffSetPrecentage / 100);

            var destination = startPos + index * spaceBetweenCard * Vector2.right;

            var xMaxDistance = _cardSlots.Count * spaceBetweenCard / 2;
            var offset = spaceBetweenCard / 2 + offSetFromCenter;
            destination += Vector2.left * (xMaxDistance - offset);

            destination += Vector2.right * _cardCenterOffSetPrecentage;
            return destination;
        }

        private float CalculateSpaceBetweenCards()
        {
            float screenWidth = Screen.width;

            var screenBoundInPercentage = screenWidth * (_cardPaddingPrecentage / 100);

            return (screenWidth - screenBoundInPercentage) / _cardSlots.Count;
        }

        public void AddCardUIToCardSlot(params CardUI[] cardUI)
        {
            var isNoMoreSpace = _cardSlots.Count == 0;

            for (var i = 0; i < cardUI.Length; i++) //loop over all the received CardUIs and Assign to  cardslot 
            {
                if (!isNoMoreSpace && !ContainCardUIInSlots(cardUI[i], out CardSlot cardSlot))
                    for (var j = 0;
                         j < _cardSlots.Count;
                         j++) //loop over all the cardslot and check if there is a available slot
                    {
                        if (!_cardSlots[j].IsHaveValue)
                        {
                            _cardSlots[j].AssignCardUI(cardUI[i]);
                            break;
                        }

                        //if (j == _cardSlots.Count - 1) // did not find a empty cardslot 
                        //isNoMoreSpace = true;
                    }

                if (isNoMoreSpace)
                {
                    AddCardSlot(cardUI[i]); //create a new cardslot and assign a cardui to it
                }

            }

            AlignCardsSlots();
        }

        public bool RemoveCardUI(CardUI cardUI)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    _cardSlots[i].RemoveCardUI();
                    return true;
                }

            return false;
        }

        public void RemoveAllCardUI()
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    _cardSlots[i].RemoveCardUI();
        }

        public bool ContainCardUIInSlots(CardUI cardUI, out CardSlot cardSlot)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsContainCardUI(cardUI))
                {
                    cardSlot = _cardSlots[i];
                    return true;
                }
            }

            cardSlot = null;
            return false;
        }

        public IReadOnlyList<CardSlot> GetCardSlotsFrom(params CardUI[] cardUIs)
        {
            var cardSlots = new List<CardSlot>();

            for (var i = 0; i < cardUIs.Length; i++)
            for (var j = 0; j < _cardSlots.Count; j++)
                if (_cardSlots[j].IsContainCardUI(cardUIs[i]))
                {
                    cardSlots.Add(_cardSlots[j]);
                    break;
                }

            return cardSlots;
        }

        public CardUI[] GetCardUIsFromTable()
        {
            var tempCardUI = new List<CardUI>();

            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    tempCardUI.Add(_cardSlots[i].CardUI);

            return tempCardUI.ToArray();
        }

        public TouchableItem<CardUI>[] GetCardUIInputsFromTable()
        {
            var tempCardUI = new List<TouchableItem<CardUI>>();

            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    tempCardUI.Add(_cardSlots[i].CardUI.Inputs);

            return tempCardUI.ToArray();
        }

        public CardSlot GetCardSlotFrom(CardUI cardUI)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsContainCardUI(cardUI))
                    return _cardSlots[i];

            Debug.LogError(cardUI.name + " is not found");
            return null;
        }


        /// <summary>
        ///     Get a List Of cardSlots Except the cardSlots that contain the params of cardUI
        /// </summary>
        /// <param name="cardUIs">The cardUI that will not be include</param>
        /// <returns></returns>
        public IReadOnlyList<CardSlot> GetCardSlotsExceptFrom(params CardUI[] cardUIs)
        {
            var cardSlots = new List<CardSlot>();
            var cardUIList = cardUIs.ToList(); //A list of cards we want to ignore and not send back

            var isFound = false;

            for (var i = 0;
                 i < _cardSlots.Count;
                 i++) //loop over all the cardslots and check if they contain the specific cardui
            {
                isFound = false;

                for (var j = 0; j < cardUIList.Count; j++)
                    if (_cardSlots[i].IsContainCardUI(cardUIList[j]))
                    {
                        isFound = true; //if the cardui was found we will update this variable to true
                        cardUIList.RemoveAt(
                            j); //If we found the card we will not have to keep checking if it is in another slot
                        break;
                    }

                if (!isFound) //If the card is not found it means that we did not ask to ignore it and we will add it to the list
                    cardSlots.Add(_cardSlots[i]);
            }

            return cardSlots;
        }

        public IReadOnlyList<CardUI> GetCardsUIExceptFrom(params CardUI[] exceptCardUIs)
        {
            var cardUIs = new List<CardUI>();
            var cardUIList = exceptCardUIs.ToList(); //A list of cards we want to ignore and not send back

            var isFound = false;

            for (var i = 0;
                 i < _cardSlots.Count;
                 i++) //loop over all the cardslots and check if they contain the specific cardui
            {
                isFound = false;

                for (var j = 0; j < cardUIList.Count; j++)
                    if (_cardSlots[i].IsContainCardUI(cardUIList[j]))
                    {
                        isFound = true; //if the cardui was found we will update this variable to true
                        cardUIList.RemoveAt(
                            j); //If we found the card we will not have to keep checking if it is in another slot
                        break;
                    }

                if (!isFound) //If the card is not found it means that we did not ask to ignore it and we will add it to the list
                    cardUIs.Add(_cardSlots[i].CardUI);
            }

            return cardUIs;
        }

        public void AdjustSize(int size)
        {
        }

        private void AddCardSlot(CardUI cardUI)
        {
            var cardSlot = new CardSlot();
            _cardSlots.Add(cardSlot);
            cardSlot.AssignCardUI(cardUI);
        }

        private void RemoveCardSlot(CardSlot cardSlot)
        {
        }

        public void ClearCardSlot()
        {
        }
    }

    #endregion

    #region CardSlot

    [Serializable]
    public class CardSlot
    {
        [SerializeField, ReadOnly] private CardUI _cardUI;
        private Vector2 _cardPos;

        public Vector2 CardPos
        {
            get => _cardPos;
            set => _cardPos = value;
        }

        public bool IsHaveValue => !ReferenceEquals(CardUI, null) || _cardUI != null;

        public CardUI CardUI
        {
            get => _cardUI;
            private set => _cardUI = value;
        }

        public void AssignCardUI(CardUI cardUI)
        {
            CardUI = cardUI;
        }

        public void RemoveCardUI()
        {
            CardUI = null;
        }

        public bool IsContainCardUI(CardUI cardUI)
        {
            if (ReferenceEquals(cardUI, null) || !IsHaveValue || cardUI == null)
                return false;

            if (cardUI.CardData.CardInstanceID == _cardUI.CardData.CardInstanceID)
                return true;

            return false;
        }
    }

        #endregion
    
    public interface IGetCardsUI
    {
        IReadOnlyList<CardUI> CardsUI { get; }
    }
}

    
    
    
   