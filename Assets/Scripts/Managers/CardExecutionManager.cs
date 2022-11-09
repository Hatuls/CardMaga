using Battle.Deck;
using Battle.Turns;
using CardMaga.Battle.Players;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Keywords;
using CardMaga.SequenceOperation;
using CardMaga.UI.Card;
using ReiTools.TokenMachine;
using System;
using UnityEngine.Events;

namespace CardMaga.Battle.Execution
{
    [Serializable]
    public class CardEvent : UnityEvent<CardData> { }

    public class CardExecutionManager : ISequenceOperation<IBattleManager>
    {
        #region Events
        public event Action OnFailedToExecute;
        public event Action OnSuccessfullExecution;
        public event Action OnPlayerCardExecute;
        public event Action<CardData> OnEnemyCardExecute;
        #endregion
        // REMOVE SINGLETON PATTERN LATER
        private static CardExecutionManager _instance;

        private IPlayersManager _playersManager;
        private GameCommands _gameCommands;
        private KeywordManager _keywordManager;

        #region Properties
        public int Priority => 0;

        public GameCommands GameCommands { get => _gameCommands; private set => _gameCommands = value; }
        public static CardExecutionManager Instance => _instance;

        #endregion
        public CardExecutionManager(IBattleManager battleManager)
        {
            _instance = this;
            battleManager.Register(this, OrderType.Default);
        }

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

            OnSuccessfullExecution?.Invoke();


            if (isLeft)
            {
                OnPlayerCardExecute?.Invoke();
            }
            else
            {
                OnEnemyCardExecute?.Invoke(card);
            }



            var deckHandler = currentPlayer.DeckHandler;
            var transferCommand = new TransferSingleCardCommand(deckHandler, DeckEnum.Selected, card.IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard, card);

            var dataCommands = GameCommands.GameDataCommands.DataCommands;

            //Transfer Card Command;
            dataCommands.AddCommand(transferCommand);

            card.InitCommands(isLeft, _playersManager, _keywordManager);
            //Visuals
            GameCommands.GameVisualCommands.InsertCardsCommands(isLeft, card);
            //Logic Commands
            GameCommands.GameDataCommands.InsertCardDataCommand(card, true, true);



            return true;
        }


        public void ForceExecuteCard(bool isPlayer, CardData card)
        {
            card.InitCommands(isPlayer, _playersManager, _keywordManager);

            GameCommands.GameVisualCommands.InsertCardsCommands(isPlayer, card);
            GameCommands.GameDataCommands.InsertCardDataCommand(card, false, false);
        }
        // Remake it so it has based the visual stats
       

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
    
            _playersManager = data.PlayersManager;
            GameCommands = data.GameCommands;
            _keywordManager = data.KeywordManager;
            data.OnBattleManagerDestroyed += FinishBattle;
        }


        #region Private 
        private IPlayer GetPlayer(bool isLeft) => _playersManager.GetCharacter(isLeft);




        private void FinishBattle(IBattleManager battleManager)
        {
            battleManager.OnBattleManagerDestroyed -= FinishBattle;
        }


        #endregion

        #region Monobehaviour Callbacks 

        #endregion


    }


}