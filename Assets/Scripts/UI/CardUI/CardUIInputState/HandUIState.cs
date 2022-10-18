using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardMaga.Input;
using CardMaga.UI.Card;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardMaga.UI
{
    public class HandUIState : BaseHandUIState , IGetCardsUI
    { 
        public event Action OnCardDrawnAndAlign;

    [Header("TransitionPackSO")] 
    [SerializeField] private TransitionPackSO _drawMoveTransitionPackSo;
    [SerializeField] private TransitionPackSO _drawScaleTransitionPackSo;
    [SerializeField] private TransitionPackSO _reAlignTransitionPackSo;
    [SerializeField] private TransitionPackSO _resetCardPositionPackSO;

    [Header("Parameters")]
    [SerializeField] private float _delayBetweenCardDrawn;

    [Header("Scripts Reference")]
    [SerializeField] private TableCardSlot _tableCardSlot;
    [SerializeField] private HandUI _handUI;
    
    private WaitForSeconds _waitForCardDrawnDelay;

    private DG.Tweening.Sequence _currentSequence;

        #region Prop

        public TableCardSlot TableCardSlot { get => _tableCardSlot; }

        #endregion

        private void Start()
    {
        _waitForCardDrawnDelay = new WaitForSeconds(_delayBetweenCardDrawn);

        _inputBehaviour.OnClick += _handUI.SetToZoomState;
        _inputBehaviour.OnBeginHold += _handUI.SetToFollowState;
    }

    private void OnDestroy()
    {
        _inputBehaviour.OnClick -= _handUI.SetToZoomState;
        _inputBehaviour.OnBeginHold -= _handUI.SetToFollowState;
    }

    public override void EnterState(CardUI cardUI)
    {
        if (!_tableCardSlot.ContainCardUIInSlots(cardUI , out CardSlot cardSlot))
        {
            _tableCardSlot.AddCardUIToCardSlot(cardUI);
        }
        
        if (!cardUI.Inputs.TrySetInputBehaviour(_inputBehaviour))
        {
            Debug.LogError(name + "Failed To Set Input Behaviour");
            return;
        }
        
        _tableCardSlot.ClearCardSlot();
        StartCoroutine(MoveCardsToHandPos(_tableCardSlot.GetCardSlotsFrom(_tableCardSlot.GetCardUIsFromTable()), OnCardDrawnAndAlign));//need to Rei-Done
    }

    public override void ExitState(CardUI cardUI)
    {
        if (_tableCardSlot.ContainCardUIInSlots(cardUI, out CardSlot cardSlot))
            cardUI.transform.SetAsLastSibling();
    }
    
    public CardUI[] RemoveAllCardUIFromHand()
    {
        CardUI[] cardUis = _tableCardSlot.GetCardUIsFromTable();
        
        _tableCardSlot.RemoveAllCardUI();
        
        for (int i = 0; i < cardUis.Length; i++)
        {
            cardUis[i].Inputs.ForceResetInputBehaviour();
        }

        return cardUis;
    }

    public void RemoveCardUI(CardUI cardUI)
    {
        _tableCardSlot.RemoveCardUI(_tableCardSlot.GetCardSlotFrom(cardUI).CardUI);
    }

    #region Transitions

        private IEnumerator MoveCardsToHandPos(IReadOnlyList<CardSlot> cardSlots, Action onComplete = null)
        {
            for (var i = 0; i < cardSlots.Count; i++)
            {
                if (!cardSlots[i].IsHaveValue)
                    continue;
                
                cardSlots[i].CardUI.transform.SetAsLastSibling();
                cardSlots[i].CardUI.RectTransform.Transition(cardSlots[i].CardPos, _drawMoveTransitionPackSo, OnCardDrawnAndAlign); //Plaster!!!
                cardSlots[i].CardUI.VisualsRectTransform.Transition(_drawScaleTransitionPackSo);
                yield return _waitForCardDrawnDelay;
            }

            onComplete?.Invoke();
        }

        private void ResetCardPosition(CardUI cardUI)
        {
            KillTween();
            DG.Tweening.Sequence temp = cardUI.RectTransform
                .Transition(_tableCardSlot.GetCardSlotFrom(cardUI).CardPos, _resetCardPositionPackSO);
        }
        
        private void KillTween()
        {
            if (_currentSequence != null) _currentSequence.Kill();
        }

        #endregion

        public IReadOnlyList<CardUI> CardsUI
        {
            get => _tableCardSlot.GetCardUIsFromTable();
        }

        public TouchableItem<CardUI>[] CardUIsInput
        {
            get => _tableCardSlot.GetCardUIInputsFromTable();
        }
    }


    #region TableCardSlot

    [Serializable]
    public class TableCardSlot
    {
        [SerializeField] private RectTransform _middleHandPos;
        [SerializeField] [Range(0, 25)] private float _cardPaddingPrecentage;
        [SerializeField] [Range(-10, 10)] private float _cardCenterOffSetPrecentage;
        [SerializeField] [ReadOnly] private List<CardSlot> _cardSlots = new List<CardSlot>();

        public IReadOnlyList<CardSlot> CardSlots => _cardSlots;
        
        
        public void AlignCardsSlots()
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
                    for (var j = 0; j < _cardSlots.Count; j++) //loop over all the cardslot and check if there is a available slot
                    {
                        if (!_cardSlots[j].IsHaveValue)
                        {
                            Debug.Log("CardAddToHandSlot");
                            _cardSlots[j].AssignCardUI(cardUI[i]);
                            break;
                        }

                        if (j == _cardSlots.Count - 1) // did not find a empty cardslot 
                            isNoMoreSpace = true;
                    }

                if (isNoMoreSpace && _cardSlots.Count < 4)
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
            for (int i = 0; i < _cardSlots.Count; i++)
            {
                if (!_cardSlots[i].IsHaveValue)
                {
                    _cardSlots.Remove(_cardSlots[i]);
                }
            }
        }
    }

    #endregion

    #region CardSlot

    [Serializable]
    public class CardSlot
    {
        [SerializeField,ReadOnly] private CardUI _cardUI;
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
        }

        public void AssignCardUI(CardUI cardUI)
        {
            _cardUI = cardUI;
        }

        public void RemoveCardUI()
        {
            _cardUI = null;
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
