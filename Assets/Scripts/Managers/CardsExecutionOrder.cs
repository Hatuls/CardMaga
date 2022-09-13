using CardMaga.Card;
using Keywords;
using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class CardsExecutionOrder
    {
        public event Action OnQueueEmpty;
        public event Action<CardData, ITokenReciever> OnCardExecute;
        public event Action<CardData> OnCardInstantExecute;
        public event Action<IReadOnlyList<KeywordData>> OnKeywordsSorted;
        public event Action<bool, KeywordData> OnKeywordExecute;
        public event Action<int> OnAnimationIndexChange;

        private readonly CardActionPool _cardActionPool;
        private readonly TokenMachine _cardExecutedTokenMachine;
        private readonly IPlayer _character;

        private int _currentKeywordIndex;
        private SortedQueue<CardAction> _queue;
        private List<KeywordData> _keywordData;
        private bool _isExecuting;
        private IDisposable _endTurnToken;
        public ITokenReciever CardExecutedTokenMachine => _cardExecutedTokenMachine;
        public bool IsQueueEmpty => _queue.IsEmpty;

        public IReadOnlyList<KeywordData> KeywordDatas => _keywordData;
        public int CurrentKeywordIndex => _currentKeywordIndex;

        public CardsExecutionOrder(IPlayer player)
        {
            _character = player;
            _cardExecutedTokenMachine = new TokenMachine(MoveNext);

            _queue = new SortedQueue<CardAction>();
            _keywordData = new List<KeywordData>();
            _cardActionPool = new CardActionPool(poolSize: 5);
            _character.MyTurn.EndTurnOperations.Register(FinishExecution, -1, OrderType.Before);
        }
        public void AddToQueue(CardData card, int priority)
        {
            _queue.Add(_cardActionPool.GetAction(card, priority));
            MoveNext();
        }
        public void MoveNext()
        {
            if (IsQueueEmpty)
            {
                _isExecuting = false;
                _endTurnToken?.Dispose();
                return;
            }
            else if (_isExecuting)
                return;
            else
                _isExecuting = true;

            CardAction nextAction = _queue.Pop();

            _character.CraftingHandler.AddFront(nextAction.CardData, true);

            if (nextAction.CardData.CardSO.AnimationBundle.AttackAnimation == string.Empty)
                InstantExecution(nextAction);
            else
                AnimationExcecution(nextAction);
        }

        public void ResetExecutions()
        {
            _queue.Reset();
            _cardExecutedTokenMachine.ForceRelease();
            OnQueueEmpty?.Invoke();
        }

        public void FinishExecution(ITokenReciever endTurnTokenMachine)
        {
            _endTurnToken = endTurnTokenMachine.GetToken();

            MoveNext();
        }

        private void SortKeywords(CardData cardData)
        {
            if (!IsQueueEmpty && cardData != null)
            {
                //clearing the list
                // registering the keywords
                // sorting it by the animation index
                //  Debug.Log("<a>Keywords Cleared</a>");
                _keywordData.Clear();

                KeywordData[] currentCard = cardData.CardKeywords;
                //   Debug.Log($"<a>sorting keywords {_cardsQueue.Count} cards left to be executed </a>");

                for (int i = 0; i < currentCard.Length; i++)
                    _keywordData.Add(currentCard[i]);

                _keywordData.Sort();

                OnKeywordsSorted?.Invoke(_keywordData);
            }
        }

        public void OnKeywordEvent()
        {
            if (BattleManager.isGameEnded)
                return;


            Debug.Log($"<a>Executing Kewords with {_keywordData.Count} keywords to be executed</a>");
            bool currentTurn = _character.IsLeft;
            KeywordData keyword = null;

            for (int i = 0; i < _keywordData.Count; i++)
            {
                if (CurrentKeywordIndex == _keywordData[i].AnimationIndex)
                {
                    keyword = _keywordData[i];
                    //remove from the list
                    _keywordData.Remove(_keywordData[i]);
                    i--;
                    // activate the keyword
                    OnKeywordExecute?.Invoke(currentTurn, keyword);

                }
            }
            _currentKeywordIndex++;
            OnAnimationIndexChange?.Invoke(_currentKeywordIndex);
        }

        internal void CheckExcecution()
        {
            if (IsQueueEmpty)
                OnQueueEmpty?.Invoke();

        }

        private void FinishAnimation()
        {
            _isExecuting = false;
            MoveNext();
        }
        private void InstantExecution(CardAction nextAction)
        {
            OnKeywordEvent();
            OnCardInstantExecute?.Invoke(nextAction.CardData);
            _cardActionPool.AddToPool(nextAction);
            FinishAnimation();
        }
        private void AnimationExcecution(CardAction nextAction)
        {
            SortKeywords(nextAction.CardData);
            using (CardExecutedTokenMachine.GetToken())
            {
                OnCardExecute?.Invoke(nextAction.CardData, CardExecutedTokenMachine);
            }
            _cardActionPool.AddToPool(nextAction);
        }
    }

    public class CardActionPool
    {
        public Queue<CardAction> _availableActions;

        public CardActionPool(int poolSize)
        {
            _availableActions = new Queue<CardAction>(poolSize);
            for (int i = 0; i < poolSize; i++)
                GenerateNewCardAction();
        }
        public CardAction GetAction(CardData cardData, int priority)
        {
            if (_availableActions.Count == 0)
                GenerateNewCardAction();

            CardAction cardAction = _availableActions.Dequeue();

            cardAction.Init(cardData, priority);

            return cardAction;

        }

        private CardAction GenerateNewCardAction()
        {
            var current = new CardAction();
            AddToPool(current);
            return current;
        }

        public void AddToPool(CardAction action)
        {
            action.Dispose();
            _availableActions.Enqueue(action);
        }
    }
    public class CardAction : IComparable<CardAction>, IDisposable
    {
        public CardData CardData;
        public int Priority;

        public CardAction()
      => Dispose();
        public void Init(CardData cardData, int priority)
        {
            Dispose();
            CardData = cardData;
            Priority = priority;
        }

        public int CompareTo(CardAction other)
        {
            if (other.Priority < Priority)
                return -1;
            else if (other.Priority > Priority)
                return 1;
            else
                return 0;
        }

        public void Dispose()
        {
            CardData = null;
            Priority = 0;
        }
    }

}