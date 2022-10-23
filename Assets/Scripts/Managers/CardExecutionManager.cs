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

        private IPlayersManager _playersManager;
        private GameTurnHandler _turnHandler;
        private IDisposable _endTurnToken;
        private GameCommands _gameCommands;


        #region Properties
        public int Priority => 0;

        public GameCommands GameCommands { get => _gameCommands; private set => _gameCommands = value; }


        #endregion


        public bool CanPlayCard(bool isLeft, CardData card) => (card == null) ? false : GetPlayer(isLeft).StaminaHandler.CanPlayCard(card);
        public bool TryExecuteCard(CardUI cardUI)
        {
            CardData card = cardUI.CardData;
            bool isExecuted = TryExecuteCard(true, card);

            return isExecuted;
        }
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
            var transferCommand = new TransferSingleCardCommand(deckHandler, DeckEnum.Selected, card.IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard, card);
            //  deckHandler.TransferCard(DeckEnum.Selected, card.IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard, card);
            //Transfer Card Command;
            GameCommands.DataCommands.AddCommand(transferCommand);

            //Logic Commands
            GameCommands.InsertCardsCommands(isLeft, card);

            //Crafting Command
            var cardTypeCommand = card.CardCommands.CardTypeCommand;
            cardTypeCommand.ToNotify = true;
            GameCommands.DataCommands.AddCommand(cardTypeCommand);
            if (isLeft)
            {
                OnPlayerCardExecute?.Invoke();
                //CardUIManager.Instance.LockHandCards(false);
            }
            else
            {
                OnEnemyCardExecute?.Invoke(card);
            }
            //  currentPlayer.StaminaHandler.ReduceStamina(card);
            // currentPlayer.ExecutionOrder.AddToQueue(card, 0,true);

            //RegisterCard(card, isLeft);

            //_playersManager.GetCharacter(isLeft).CraftingHandler.AddFront(card, true);

            return true;
        }


        public void ForceExecuteCard(bool isPlayer, CardData card)
        {

            GameCommands.InsertCardsCommands(isPlayer, card);
            var cardTypeCommand = card.CardCommands.CardTypeCommand;
            cardTypeCommand.ToNotify = false;
            GameCommands.DataCommands.AddCommand(cardTypeCommand);
            // add to crafting slot
            //(_playersManager.GetCharacter(isPlayer).DeckHandler[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(card);
            //currentCharacter.ExecutionOrder.AddToQueue(card,0,false);
            //RegisterCard(card);

        }
        // Remake it so it has based the visual stats
        internal bool CanDefendIncomingAttack(bool Reciever)
        {
            IPlayer currentPlayer = GetPlayer(Reciever);
            BaseStat shieldStat = currentPlayer.StatsHandler.GetStat(KeywordTypeEnum.Shield);
          //  Debug.LogError("No visual stats yet");
            var historyCommands = _gameCommands.KeywordCommandHandler.CommandStack;

            foreach (var item in historyCommands)
            {
                if (item is KeywordCommand keywordCommand && keywordCommand.KeywordType == KeywordTypeEnum.Attack)
                    return keywordCommand.KeywordData.GetAmountToApply <= shieldStat.Amount;
            }
            return false;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _turnHandler = data.TurnHandler;
            _playersManager = data.PlayersManager;
            GameCommands = data.GameCommands;
            //_turnHandler.GetCharacterTurn(true).EndTurnOperations.Register(RecieveToken);
            //_turnHandler.GetCharacterTurn(false).EndTurnOperations.Register(RecieveToken);
            data.OnBattleManagerDestroyed += FinishBattle;
        }


        //private void DisposeEndTurnToken() => _endTurnToken?.Dispose();

        #region Private 
        private IPlayer GetPlayer(bool isLeft) => _playersManager.GetCharacter(isLeft);




        private void FinishBattle(IBattleManager battleManager)
        {
            battleManager.OnBattleManagerDestroyed -= FinishBattle;
        }


        #endregion

        #region Monobehaviour Callbacks 

        public override void Awake()
        {
            base.Awake();
            BattleManager.Register(this, OrderType.Default);
        }



        #endregion


    }



    public class GameCommands : IDisposable
    {
        //Handling the data
        private CommandHandler<ICommand> _dataCommands;
        //Handling the animation of the character
        private VisualCommandHandler _animationCommands;
        //Handling the keywords animation
        private KeywordCommandHandler _keywordsCommand;

        private KeywordManager _keywordManager;
        private IPlayersManager _playersManager;

        public CommandHandler<ICommand> DataCommands => _dataCommands;
        public KeywordCommandHandler KeywordCommandHandler => _keywordsCommand;
        public VisualCommandHandler AnimationCommands => _animationCommands;


        public GameCommands(IPlayersManager playersManager, KeywordManager keywordManager)
        {
            _playersManager = playersManager;
            _keywordManager = keywordManager;
            bool isLeft = true;
            playersManager.GetCharacter(isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;
            playersManager.GetCharacter(!isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;

            _dataCommands = new CommandHandler<ICommand>();
            _keywordsCommand = new KeywordCommandHandler();
            _animationCommands = new VisualCommandHandler();
        }

        public void InsertCardsCommands(bool isLeft, CardData card)
        {
            card.InitCommands(isLeft, _playersManager, _keywordManager);
            var cardCommands = card.CardCommands;

            _dataCommands.AddCommand(cardCommands.CardsKeywords.StaminaCommand);
            _keywordsCommand.AddCommand(cardCommands.CardsKeywords.KeywordCommand);
            _animationCommands.AddCommand(cardCommands.AnimationCommand);

            if (_animationCommands.IsEmpty && card.CardSO.AnimationBundle.AttackAnimation.Length == 0)
                _keywordsCommand.ExecuteKeywords();
        }

        public void ResetAll()
        {
            _dataCommands.ResetCommands();
            _animationCommands.ResetCommands();
            _keywordsCommand.ResetCommands();
        }

        public void ExecuteKeywords()
        {
            _keywordsCommand.ExecuteKeywords();
        }

        public void Dispose()
        {
            ResetAll();
            bool isLeft = true;
            _playersManager.GetCharacter(isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword -= ExecuteKeywords;
            _playersManager.GetCharacter(!isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword -= ExecuteKeywords;
        }
    }
}