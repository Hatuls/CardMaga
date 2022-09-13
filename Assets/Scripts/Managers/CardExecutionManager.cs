using Battle;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Card;
using CardMaga.UI.Card;
using Characters.Stats;
using Keywords;
using Managers;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Battle
{
    [Serializable]
    public class CardEvent : UnityEvent<CardData> { }

    public class CardExecutionManager : MonoSingleton<CardExecutionManager>, ISequenceOperation<IBattleManager>
    {
   //
        public  event Action OnPlayerCardExecute;
        public  event Action<CardData> OnEnemyCardExecute;
   //     public static event Action<List<KeywordData>> OnSortingKeywords;
   //     public static event Action<int> OnAnimationIndexChange;
   //     public static event Action OnInsantExecute;
   //     public static event Action<bool, KeywordData> OnKeywordExecute;
        //public static bool FinishedAnimation
        //{
        //    get => _isFinishedAnimation;
        //    set
        //    {
        //        _isFinishedAnimation = value;
        //        if (_isFinishedAnimation)
        //            Instance.DisposeEndTurnToken();
        //    }
        //}
    //    static List<KeywordData> _keywordData = new List<KeywordData>();
   //     private static bool _isFinishedAnimation;
    //    static int currentKeywordIndex;
        //[Sirenix.OdinInspector.ShowInInspector]
        //static Queue<CardData> _cardsQueue = new Queue<CardData>();

        //public static Queue<CardData> CardsQueue => _cardsQueue;

      //  public static int CurrentKeywordIndex { get => currentKeywordIndex; }


        private GameTurnHandler _turnHandler;
        private IPlayersManager _playersManager;
        private IDisposable _endTurnToken;
 



        [SerializeField] UnityEvent OnSuccessfullExecution;
        [SerializeField] UnityEvent OnFailedToExecute;
        public int Priority => 0;



        public bool TryExecuteCard(bool isLeft, CardData card)
        {
            if (card == null)
                throw new System.Exception("Card cannot be executed card is null\n Player " + isLeft + " Tried to play a null Card");
            if (CanPlayCard(isLeft, card) == false)
            {
                // not enough stamina 
                if (isLeft)
                {
                    //    _staminaBtn.PlayRejectAnimation();
                    OnFailedToExecute?.Invoke();
                }

                return false;
            }
            var currentPlayer = GetPlayer(isLeft);
            // execute card
            currentPlayer.StaminaHandler.ReduceStamina(card);
            OnSuccessfullExecution?.Invoke();


            var deckHandler = currentPlayer.DeckHandler;
            deckHandler.TransferCard(DeckEnum.Selected, card.IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard, card);

            if (isLeft)
            {
                OnPlayerCardExecute?.Invoke();
                //CardUIManager.Instance.LockHandCards(false);
            }
            else
            {
                OnEnemyCardExecute?.Invoke(card);
            }

            currentPlayer.ExecutionOrder.AddToQueue(card, 0);

            //RegisterCard(card, isLeft);

            //_playersManager.GetCharacter(isLeft).CraftingHandler.AddFront(card, true);

            return true;
        }

        public bool TryExecuteCard(CardUI cardUI)
        {
            CardData card = cardUI.CardData;
            bool isExecuted = TryExecuteCard(true, card);

            return isExecuted;
        }

        public void ExecuteCraftedCard(bool isPlayer, CardData card)
        {
            // add to crafting slot
            var currentCharacter = GetPlayer(isPlayer);
            currentCharacter.CraftingHandler.AddFront(card, false);

            //(_playersManager.GetCharacter(isPlayer).DeckHandler[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(card);
            currentCharacter.ExecutionOrder.AddToQueue(card, -1);

            currentCharacter.ExecutionOrder.MoveNext();
            //RegisterCard(card);

        }

        //public void RegisterCard(CardData card, bool isPlayer = true)
        //{

        //    if (BattleManager.isGameEnded)
        //        return;

        //    var currentCard = card;

        //    if (currentCard == null)
        //        throw new System.Exception($"Cannot Execute Card that is null!!!!");


        //    AddToQueue(card);
        //}

        //private void ExecuteInstantly()
        //{

        //    OnKeywordEvent();
        //    OnInsantExecute?.Invoke();
        //    _cardsQueue.Dequeue();
        //}




        //public void AddToQueue(CardData card)
        //{
        //    if (BattleManager.isGameEnded)
        //        return;
        //    EnqueueCard(card);
        //    bool firstCard = _cardsQueue.Count == 1;
        //    Debug.Log($"<a>Register card queue has {_cardsQueue.Count} cards in it\nIs First Card {firstCard}</a>");

        //    if (firstCard)
        //    {
        //        ActivateCard();
        //    }

        //}

        //private void EnqueueCard(CardData card)
        //{
        //    _cardsQueue.Enqueue(card);

        //}

        //public void ActivateCard()
        //{
        //    // play the card animation
        //    if (_cardsQueue.Count == 0 || BattleManager.isGameEnded)
        //    {
        //        DisposeEndTurnToken();
        //        return;
        //    }
        //    Debug.Log("<a>Activating Card</a>");

        //    SortKeywords();
        //    var card = _cardsQueue.Peek();
        //    currentKeywordIndex = 0;
        //    OnAnimationIndexChange?.Invoke(currentKeywordIndex);
        //    if (card.CardSO.AnimationBundle.AttackAnimation == string.Empty)
        //    {
        //        ExecuteInstantly();
        //    }
        //    else
        //    {

        //        _playersManager.GetCharacter(_turnHandler.IsLeftCharacterTurn).VisualCharacter.AnimatorController.PlayCrossAnimation();
        //    }

        //}

        internal bool CanDefendIncomingAttack(bool Reciever)
        {
            IPlayer currentPlayer = GetPlayer(Reciever);
            BaseStat shieldStat = currentPlayer.StatsHandler.GetStats(KeywordTypeEnum.Shield);
            CardsExecutionOrder executionOrder = currentPlayer.ExecutionOrder;
            IReadOnlyList<KeywordData> keywordList = executionOrder.KeywordDatas;

            for (int i = 0; i < keywordList.Count; i++)
            {
                if (executionOrder.CurrentKeywordIndex == keywordList[i].AnimationIndex)
                {
                    if (keywordList[i].KeywordSO.GetKeywordType == KeywordTypeEnum.Attack)
                        return shieldStat.Amount >= keywordList[i].GetAmountToApply;              
                }
            }
            return false;
        }

        //private void SortKeywords()
        //{
        //    if (_cardsQueue.Count > 0)
        //    {
        //        //clearing the list
        //        // registering the keywords
        //        // sorting it by the animation index
        //        //  Debug.Log("<a>Keywords Cleared</a>");
        //        _keywordData.Clear();

        //        var currentCard = _cardsQueue.Peek().CardKeywords;
        //        //   Debug.Log($"<a>sorting keywords {_cardsQueue.Count} cards left to be executed </a>");

        //        for (int i = 0; i < currentCard.Length; i++)
        //        {
        //            _keywordData.Add(currentCard[i]);

        //        }
        //        _keywordData.Sort();

        //        OnSortingKeywords?.Invoke(_keywordData);
        //    }

        //}
        ////public void CardFinishExecuting()
        ////{
        ////    if (_cardsQueue.Count > 0)
        ////    {
        ////        Debug.Log("<a>Activating Next card</a>");
        ////        ActivateCard();

        ////    }
        ////    FinishedAnimation = true;
        ////}

        //public void OnKeywordEvent()
        //{
        //    if (BattleManager.isGameEnded)
        //        return;


        //    Debug.Log($"<a>Executing Kewords with {_keywordData.Count} keywords to be executed</a>");
        //    bool currentTurn = _turnHandler.IsLeftCharacterTurn;


        //    for (int i = 0; i < _keywordData.Count; i++)
        //    {
        //        if (CurrentKeywordIndex == _keywordData[i].AnimationIndex)
        //        {
        //            var keyword = _keywordData[i];
        //            //remove from the list
        //            _keywordData.Remove(_keywordData[i]);
        //            i--;
        //            // activate the keyword
        //            OnKeywordExecute?.Invoke(currentTurn, keyword);

        //        }
        //    }
        //    currentKeywordIndex++;
        //    OnAnimationIndexChange?.Invoke(currentKeywordIndex);
        //}
        //private void ResetExecutionData()
        //{
        //    FinishedAnimation = true;
        //    _cardsQueue.Clear();
        //    _keywordData.Clear();
        //    _endTurnToken = null;
        //}
        //private void RecieveToken(ITokenReciever endTurnToken)
        //{
        //    if (_cardsQueue.Count > 0 && !BattleManager.isGameEnded)
        //        _endTurnToken = endTurnToken.GetToken();
        //}
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _turnHandler = data.TurnHandler;
            _playersManager = data.PlayersManager;

            //_turnHandler.GetCharacterTurn(true).EndTurnOperations.Register(RecieveToken);
            //_turnHandler.GetCharacterTurn(false).EndTurnOperations.Register(RecieveToken);
            data.OnBattleManagerDestroyed += FinishBattle;
        }

        private void FinishBattle(IBattleManager battleManager)
        {
            battleManager.OnBattleManagerDestroyed -= FinishBattle;
        }
        //private void DisposeEndTurnToken() => _endTurnToken?.Dispose();

        #region Private 
        private IPlayer GetPlayer(bool isLeft) => _playersManager.GetCharacter(isLeft);
        private bool CanPlayCard(bool isLeft, CardData card) => (card == null) ? false : GetPlayer(isLeft).StaminaHandler.CanPlayCard(card);



        #endregion

        #region Monobehaviour Callbacks 

        public override void Awake()
        {
            base.Awake();
            BattleManager.Register(this, OrderType.Default);
        }



        #endregion
    }

}