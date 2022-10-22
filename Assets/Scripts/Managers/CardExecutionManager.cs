using Battle.Deck;
using Battle.Turns;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
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

        public event Action OnPlayerCardExecute;
        public event Action<CardData> OnEnemyCardExecute;
        [SerializeField] UnityEvent OnSuccessfullExecution;
        [SerializeField] UnityEvent OnFailedToExecute;

        private KeywordManager _keywordManager;
        private GameTurnHandler _turnHandler;
        private IPlayersManager _playersManager;
        private IDisposable _endTurnToken;

        //Handling the data
        private CommandHandler<ICommand> _dataCommands;
        //Handling the animation of the character
        private VisualCommandHandler _animationCommands;
        //Handling the keywords animation
        private KeywordVisualCommandHandler _keywordVisualCommandsHandler;

        #region Properties
        public int Priority => 0;
        public CommandHandler<ICommand> DataCommands => _dataCommands;
        public KeywordVisualCommandHandler KeywordVisualCommandHandler => _keywordVisualCommandsHandler;
        public VisualCommandHandler AnimationCommands => _animationCommands;

        #endregion


        public bool CanPlayCard(bool isLeft, CardData card) => (card == null) ? false : GetPlayer(isLeft).StaminaHandler.CanPlayCard(card);
        public bool TryExecuteCard(bool isLeft, CardData card)
        {
            if (card == null)
                throw new System.Exception("Card cannot be executed card is null\n LeftPlayer " + isLeft + " Tried to play a null Card");
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
            // currentPlayer.StaminaHandler.ReduceStamina(card);
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
            card.InitCommands(isLeft, _playersManager, _keywordManager);

            _dataCommands.AddCommand(card.CardDataCommand);
            _animationCommands.AddCommand(card.AnimationVisualCommand);
            //  currentPlayer.StaminaHandler.ReduceStamina(card);
            // currentPlayer.ExecutionOrder.AddToQueue(card, 0,true);

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

        public void ForceExecuteCard(bool isPlayer, CardData card)
        {
            // add to crafting slot


            card.InitCommands(isPlayer, _playersManager, _keywordManager);

            _dataCommands.AddCommand(card.CardDataCommand);
            _animationCommands.AddCommand(card.AnimationVisualCommand);

            //(_playersManager.GetCharacter(isPlayer).DeckHandler[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(card);
            //currentCharacter.ExecutionOrder.AddToQueue(card,0,false);
            //RegisterCard(card);

        }
        // Remake it so it has based the visual stats
        internal bool CanDefendIncomingAttack(bool Reciever)
        {
            IPlayer currentPlayer = GetPlayer(Reciever);
            BaseStat shieldStat = currentPlayer.StatsHandler.GetStats(KeywordTypeEnum.Shield);
            Debug.LogError("No visual stats yet");
            //IReadOnlyList<KeywordData> keywordList = executionOrder.KeywordDatas;

            //for (int i = 0; i < keywordList.Count; i++)
            //{
            //    if (executionOrder.CurrentKeywordIndex == keywordList[i].AnimationIndex)
            //    {
            //        if (keywordList[i].KeywordSO.GetKeywordType == KeywordTypeEnum.Attack)
            //            return shieldStat.Amount >= keywordList[i].GetAmountToApply;
            //    }
            //}
            return false;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _turnHandler = data.TurnHandler;
            _playersManager = data.PlayersManager;
            _keywordManager = data.KeywordManager;
            //_turnHandler.GetCharacterTurn(true).EndTurnOperations.Register(RecieveToken);
            //_turnHandler.GetCharacterTurn(false).EndTurnOperations.Register(RecieveToken);
            data.OnBattleManagerDestroyed += FinishBattle;
        }
        public void ResetAll()
        {
            _dataCommands.ResetCommands();
            _animationCommands.ResetCommands();
            _keywordVisualCommandsHandler.ResetCommands();
        }
        private void FinishBattle(IBattleManager battleManager)
        {
            battleManager.OnBattleManagerDestroyed -= FinishBattle;
        }
        //private void DisposeEndTurnToken() => _endTurnToken?.Dispose();

        #region Private 
        private IPlayer GetPlayer(bool isLeft) => _playersManager.GetCharacter(isLeft);




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