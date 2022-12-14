using CardMaga.Input;
using CardMaga.UI.Card;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardMaga.UI
{
    public class HandUIState : BaseHandUIState, IGetCardsUI
    {
        public static event Action OnCardDrawnAndAlign;
        public event Action OnAllCardsDrawnAndAlign;
        
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

        public override void EnterState(BattleCardUI battleCardUI)
        {
            _tableCardSlot.AddCardUIToCardSlot(battleCardUI);
            
            if (!battleCardUI.Inputs.TrySetInputBehaviour(_inputBehaviour))
            {
                Debug.LogError(name + "Failed To Set InputIdentificationSO Behaviour");
                return;
            }

            //   _tableCardSlot.ClearCardSlot();
            StartCoroutine(MoveCardsToHandPos(_tableCardSlot.GetCardSlotsFrom(_tableCardSlot.GetCardUIsFromTable()), AlignCards));//need to Rei-Done
        }

        public override void ExitState(BattleCardUI battleCardUI)
        {
            if (_tableCardSlot.ContainCardUIInSlots(battleCardUI, out CardSlot cardSlot))
                battleCardUI.transform.SetAsLastSibling();
        }

        public BattleCardUI[] RemoveAllCardUIFromHand()
        {
            BattleCardUI[] cardUis = _tableCardSlot.GetCardUIsFromTable();

            _tableCardSlot.RemoveAllCardUI();

            for (int i = 0; i < cardUis.Length; i++)
            {
                cardUis[i].Inputs.ForceResetInputBehaviour();
            }

            return cardUis;
        }

        public void RemoveCardUI(BattleCardUI battleCardUI)
        {
            _tableCardSlot.RemoveCardUI(_tableCardSlot.GetCardSlotFrom(battleCardUI).BattleCardUI);
        }

        #region Transitions

        private IEnumerator MoveCardsToHandPos(IReadOnlyList<CardSlot> cardSlots, Action onComplete = null)
        {
            for (var i = 0; i < cardSlots.Count; i++)
            {
                CardSlot currentSlot = cardSlots[i];
                if (!currentSlot.IsHaveValue)
                    continue;

                BattleCardUI battleCardUI = currentSlot.BattleCardUI;
                battleCardUI.transform.SetAsLastSibling();
                var sequence = battleCardUI.RectTransform.Transition(currentSlot.CardPos, _drawMoveTransitionPackSo)//Plaster!!!
                .Join(battleCardUI.VisualsRectTransform.Transition(_drawScaleTransitionPackSo))
                .OnComplete(AlignCards);

                if (battleCardUI.CurrentSequence != null && !battleCardUI.CurrentSequence.IsComplete())
                    battleCardUI.CurrentSequence.Join(sequence);
                else
                    battleCardUI.CurrentSequence = sequence;

                yield return _waitForCardDrawnDelay;
            }

            onComplete?.Invoke();
            yield return null;
            if (OnAllCardsDrawnAndAlign != null)
                OnAllCardsDrawnAndAlign.Invoke();
        }
        
        private IEnumerator MoveCardToHandPos(CardSlot cardSlots, Action onComplete = null)
        {
            CardSlot currentSlot = cardSlots;
            
                if (!currentSlot.IsHaveValue)
                    yield break;

                BattleCardUI battleCardUI = currentSlot.BattleCardUI;
                battleCardUI.transform.SetAsLastSibling();
                var sequence = battleCardUI.RectTransform.Transition(currentSlot.CardPos, _drawMoveTransitionPackSo)//Plaster!!!
                    .Join(battleCardUI.VisualsRectTransform.Transition(_drawScaleTransitionPackSo))
                    .OnComplete(AlignCards);

                if (battleCardUI.CurrentSequence != null && !battleCardUI.CurrentSequence.IsComplete())
                    battleCardUI.CurrentSequence.Join(sequence);
                else
                    battleCardUI.CurrentSequence = sequence;

                yield return _waitForCardDrawnDelay;
            

            onComplete?.Invoke();
            yield return null;
            if (OnAllCardsDrawnAndAlign != null)
                OnAllCardsDrawnAndAlign.Invoke();
        }

     
        private void AlignCards() => OnCardDrawnAndAlign?.Invoke();
        #endregion

        public IReadOnlyList<BattleCardUI> CardsUI
        {
            get => _tableCardSlot.GetCardUIsFromTable();
        }

        public TouchableItem<BattleCardUI>[] CardUIsInput
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

        public void AddCardUIToCardSlot(params BattleCardUI[] cardUI)
        {
            var isNoMoreSpace = _cardSlots.Count == 0;

            for (var i = 0; i < cardUI.Length; i++) //loop over all the received CardUIs and Assign to  cardslot 
            {
                if (!isNoMoreSpace && !ContainCardUIInSlots(cardUI[i], out CardSlot cardSlot))
                    for (var j = 0; j < _cardSlots.Count; j++) //loop over all the cardslot and check if there is a available slot
                    {
                        if (!_cardSlots[j].IsHaveValue)
                        {
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

        public bool RemoveCardUI(BattleCardUI battleCardUI)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsContainCardUI(battleCardUI))
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

        public bool ContainCardUIInSlots(BattleCardUI battleCardUI, out CardSlot cardSlot)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
            {
                if (_cardSlots[i].IsContainCardUI(battleCardUI))
                {
                    cardSlot = _cardSlots[i];
                    return true;
                }
            }

            cardSlot = null;
            return false;
        }

        public IReadOnlyList<CardSlot> GetCardSlotsFrom(params BattleCardUI[] cardUIs)
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

        public BattleCardUI[] GetCardUIsFromTable()
        {
            var tempCardUI = new List<BattleCardUI>();

            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    tempCardUI.Add(_cardSlots[i].BattleCardUI);

            return tempCardUI.ToArray();
        }

        public TouchableItem<BattleCardUI>[] GetCardUIInputsFromTable()
        {
            var tempCardUI = new List<TouchableItem<BattleCardUI>>();

            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsHaveValue)
                    tempCardUI.Add(_cardSlots[i].BattleCardUI.Inputs);

            return tempCardUI.ToArray();
        }

        public CardSlot GetCardSlotFrom(BattleCardUI battleCardUI)
        {
            for (var i = 0; i < _cardSlots.Count; i++)
                if (_cardSlots[i].IsContainCardUI(battleCardUI))
                    return _cardSlots[i];

            Debug.LogError(battleCardUI.name + " is not found");
            return null;
        }


        /// <summary>
        ///     Get a List Of cardSlots Except the cardSlots that contain the params of cardUI
        /// </summary>
        /// <param name="cardUIs">The cardUI that will not be include</param>
        /// <returns></returns>
        public IReadOnlyList<CardSlot> GetCardSlotsExceptFrom(params BattleCardUI[] cardUIs)
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
                            j); //If we found the battleCard we will not have to keep checking if it is in another slot
                        break;
                    }

                if (!isFound) //If the battleCard is not found it means that we did not ask to ignore it and we will add it to the list
                    cardSlots.Add(_cardSlots[i]);
            }

            return cardSlots;
        }

        public IReadOnlyList<BattleCardUI> GetCardsUIExceptFrom(params BattleCardUI[] exceptCardUIs)
        {
            var cardUIs = new List<BattleCardUI>();
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
                            j); //If we found the battleCard we will not have to keep checking if it is in another slot
                        break;
                    }

                if (!isFound) //If the battleCard is not found it means that we did not ask to ignore it and we will add it to the list
                    cardUIs.Add(_cardSlots[i].BattleCardUI);
            }

            return cardUIs;
        }

        public void AdjustSize(int size)
        {
        }

        private void AddCardSlot(BattleCardUI battleCardUI)
        {
            var cardSlot = new CardSlot();
            _cardSlots.Add(cardSlot);
            cardSlot.AssignCardUI(battleCardUI);
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
        [SerializeField, ReadOnly] private BattleCardUI battleCardUI;
        private Vector2 _cardPos;

        public Vector2 CardPos
        {
            get => _cardPos;
            set => _cardPos = value;
        }

        public bool IsHaveValue => !ReferenceEquals(BattleCardUI, null) || battleCardUI != null;

        public BattleCardUI BattleCardUI
        {
            get => battleCardUI;
        }

        public void AssignCardUI(BattleCardUI battleCardUI)
        {
            this.battleCardUI = battleCardUI;
        }

        public void RemoveCardUI()
        {
            battleCardUI = null;
        }

        public bool IsContainCardUI(BattleCardUI battleCardUI)
        {
            if (ReferenceEquals(battleCardUI, null) || !IsHaveValue || battleCardUI == null)
                return false;

            if (battleCardUI.BattleCardData.Equals(this.battleCardUI.BattleCardData))
                return true;

            return false;
        }
    }

    #endregion

    public interface IGetCardsUI
    {
        IReadOnlyList<BattleCardUI> CardsUI { get; }
    }

}
