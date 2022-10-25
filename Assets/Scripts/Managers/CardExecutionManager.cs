using Battle.Deck;
using Battle.Turns;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
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
        private KeywordManager _keywordManager;

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


            if (isLeft)
            {
                OnPlayerCardExecute?.Invoke();
                //CardUIManager.Instance.LockHandCards(false);
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
            GameCommands.GameDataCommands.InsertCardDataCommand(card,true, true);



            return true;
        }


        public void ForceExecuteCard(bool isPlayer, CardData card)
        {
            card.InitCommands(isPlayer, _playersManager, _keywordManager);
       
            GameCommands.GameVisualCommands.InsertCardsCommands(isPlayer, card);
            GameCommands.GameDataCommands.InsertCardDataCommand(card,false, false);
        }
        // Remake it so it has based the visual stats
        internal bool CanDefendIncomingAttack(bool Reciever)
        {
          //  IPlayer currentPlayer = GetPlayer(Reciever);
         //   var shieldStat = currentPlayer.VisualCharacter.VisualStats.VisualStatsDictionary[KeywordTypeEnum.Shield];

           // Debug.LogError("No visual stats yet");
            var historyCommands = _gameCommands.GameVisualCommands.VisualKeywordCommandHandler.CommandStack;


            foreach (var item in historyCommands)
            {
                switch (item)
                {
                    case VisualKeywordCommand cmd:
                        if (cmd.KeywordType == KeywordTypeEnum.Shield)
                            return cmd.Amount > 0;
                        break;
                    case VisualKeywordsPackCommands pack:
                        foreach (var visualkeword in pack.VisualKeywordCommands)
                        {
                            if (visualkeword.KeywordType == KeywordTypeEnum.Shield)
                                return visualkeword.Amount > 0;
                        }

                        break;
                    default:
                        break;
                }
            }


            return false;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _turnHandler = data.TurnHandler;
            _playersManager = data.PlayersManager;
            GameCommands = data.GameCommands;
            _keywordManager = data.KeywordManager;
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
        private readonly GameDataCommands _gameDataCommands;
        private readonly GameVisualCommands _gameVisualCommands;

        public GameCommands(IBattleManager battleManager)
        {
            _gameDataCommands = new GameDataCommands(battleManager.PlayersManager, battleManager.KeywordManager);
            _gameVisualCommands = new GameVisualCommands(battleManager);
        }

        public GameDataCommands GameDataCommands => _gameDataCommands;
        public GameVisualCommands GameVisualCommands => _gameVisualCommands;

        public void Dispose()
        {
            GameDataCommands.Dispose();
            GameVisualCommands.Dispose();
        }
    }

    public class GameVisualCommands : IDisposable
    {

        public readonly IPoolObject<AnimationVisualCommand> ModelAnimationCommandsPool;
        public readonly IPoolObject<VisualKeywordCommand> VisualKeywordCommandsPool;

        //Handling the animation of the character
        private VisualCommandHandler _animationCommands;
        private VisualKeywordCommandHandler _visualKeywordCommandHandler;

        private IPlayersManager _playersManager;




        public VisualCommandHandler AnimationCommands => _animationCommands;

        public VisualKeywordCommandHandler VisualKeywordCommandHandler { get => _visualKeywordCommandHandler; }


        public GameVisualCommands(IBattleManager battleManager)
        {
            _playersManager = battleManager.PlayersManager;

            bool isLeft = true;
            _playersManager.GetCharacter(isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;
            _playersManager.GetCharacter(!isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;




            ModelAnimationCommandsPool = new ObjectPool<AnimationVisualCommand>(5);
            VisualKeywordCommandsPool = new ObjectPool<VisualKeywordCommand>(5);


            _animationCommands = new VisualCommandHandler();
            _visualKeywordCommandHandler = new VisualKeywordCommandHandler();
        }
        public void InsertCardsCommands(bool isLeft, CardData card)
        {
            AnimationVisualCommand visualCommand = ModelAnimationCommandsPool.Pull();
            visualCommand.Init(_playersManager.GetCharacter(isLeft).VisualCharacter.AnimatorController, card.CardSO, CommandType.AfterPrevious);
            _animationCommands.AddCommand(visualCommand);

        }





        public void ExecuteKeywords()
        {
            _visualKeywordCommandHandler.ExecuteKeywords();
        }

        public void Dispose()
        {
            VisualKeywordCommandHandler.ResetCommands();
            AnimationCommands.ResetCommands();

            bool isLeft = true;
            _playersManager.GetCharacter(isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword -= ExecuteKeywords;
            _playersManager.GetCharacter(!isLeft).VisualCharacter.AnimatorController.OnAnimationExecuteKeyword -= ExecuteKeywords;
        }
    }

    public class GameDataCommands : IDisposable
    {
        //Handling the data
        private CommandHandler<ICommand> _dataCommands;

        private KeywordManager _keywordManager;
        private IPlayersManager _playersManager;

        public CommandHandler<ICommand> DataCommands => _dataCommands;



        public GameDataCommands(IPlayersManager playersManager, KeywordManager keywordManager)
        {
            _playersManager = playersManager;
            _keywordManager = keywordManager;
            _dataCommands = new CommandHandler<ICommand>();
        }

   
        public void InsertCardDataCommand(CardData card,bool toReduceStamina, bool toDetectCombo)
        {
            CardCommandsHolder cardCommands = card.CardCommands;
            if (toReduceStamina)
                _dataCommands.AddCommand(cardCommands.StaminaCostCommand);
            cardCommands.CardTypeCommand.ToNotify = toDetectCombo;
            _dataCommands.AddCommand(cardCommands);
        }
        public void ResetAll()
        {
            _dataCommands.ResetCommands();
        }


        public void Dispose()
        {
            ResetAll();

        }
    }


}