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
    public class CardEvent : UnityEvent<BattleCardData> { }

    public class CardExecutionManager : ISequenceOperation<IBattleManager>
    {
        #region Events
        public event Action OnFailedToExecute;
        public event Action OnSuccessfullExecution;
        public event Action OnPlayerCardExecute;

        public event Action<CardData> OnEnemyCardExecute;
        public event Action<bool, CardData> OnCardDataExecute;

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

        public bool CanPlayCard(bool isLeft, BattleCardData battleCard) => (battleCard == null) ? false : GetPlayer(isLeft).StaminaHandler.CanPlayCard(battleCard);
        public bool TryExecuteCard(BattleCardUI battleCardUI)
        {
            BattleCardData battleCard = battleCardUI.BattleCardData;
            bool isExecuted = TryExecuteCard(true, battleCard);

            return isExecuted;
        }
        public bool TryExecuteCard(bool isLeft, BattleCardData battleCard)
        {
            if (battleCard == null)
                throw new System.Exception("BattleCard cannot be executed battleCard is null\n LeftPlayer " + isLeft + " Tried to play a null BattleCard");

            if (CanPlayCard(isLeft, battleCard) == false)
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
                OnEnemyCardExecute?.Invoke(battleCard);
            }



            var deckHandler = currentPlayer.DeckHandler;
            var transferCommand = new TransferSingleCardCommand(deckHandler, DeckEnum.Selected, battleCard.IsExhausted ? DeckEnum.Exhaust : DeckEnum.Discard, battleCard);

            var dataCommands = GameCommands.GameDataCommands.DataCommands;

            //Transfer BattleCard Command;
            dataCommands.AddCommand(transferCommand);

            battleCard.InitCommands(isLeft, _playersManager, _keywordManager);
            //Visuals

            OnCardDataExecute?.Invoke(isLeft, card);


            //Logic Commands
            GameCommands.GameDataCommands.InsertCardDataCommand(battleCard, true, true);



            return true;
        }



        public void ForceExecuteCard(bool isLeft, CardData card)
        {
            card.InitCommands(isLeft, _playersManager, _keywordManager);

            OnCardDataExecute?.Invoke(isLeft, card);
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