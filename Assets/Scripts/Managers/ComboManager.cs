using Battle.Combo;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
using Cards;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using ThreadsHandler;
using Unity.Events;
using UnityEngine;

namespace Battle
{
    public class ComboManager : MonoSingleton<ComboManager>, ISequenceOperation<IBattleManager>
    {

        #region Events
        [SerializeField] VoidEvent _successCrafting;
        public event Action<Combo.ComboData> OnComboSucceeded;
        public event Action OnComboDetectedFinished;
        public event Action<CardData[]> OnCraftingComboToHand;

        #endregion

        #region Fields

        [SerializeField] Combo.ComboData _cardRecipeDetected;
        [SerializeField] VFXSO _comboVFX;
        private bool _isTryingToDetect;
        private IPlayersManager _playersManager;
        private GameTurnHandler _gameTurnHandler;
        private CardExecutionManager _cardExecutionManager;
        private GameCommands _gameCommands;
        static byte threadId;
        private CardTypeComparer _cardTypeComparer = new CardTypeComparer();
        private Factory.GameFactory.CardFactory _cardFactory;
        #endregion

        #region Properties
        public bool IsTryingToDetect => _isTryingToDetect;
        public Battle.Combo.ComboData CardRecipeDetected
        {
            get => _cardRecipeDetected;
            set
            {
                _cardRecipeDetected = value;
            }
        }

        public static bool FoundCombo { get; internal set; }

        public int Priority => 0;


        #endregion
        public override void Awake()
        {
            base.Awake();

            BattleManager.Register(this, OrderType.Default);
        }

        public void Start()
        {
            threadId = ThreadHandler.GetNewID;
            _cardFactory = Factory.GameFactory.Instance.CardFactoryHandler;
        }

        public void TryForge(bool isPlayer)
        {
            var ComboSO = _cardRecipeDetected.ComboSO;
            if (_cardRecipeDetected != null && ComboSO != null)
            {

                var craftedCard = _cardFactory.CreateCard(ComboSO.CraftedCard.ID, _cardRecipeDetected.Level);

                _successCrafting?.Raise();
                DeckHandler deck = _playersManager.GetCharacter(isPlayer).DeckHandler;
                ICommand command;
                var dataCommandsHandler = _gameCommands.GameDataCommands.DataCommands;
                switch (ComboSO.GoToDeckAfterCrafting)
                {
                    case DeckEnum.Hand:
                        command = new AddNewCardToDeck(DeckEnum.Hand, craftedCard, deck);
                        dataCommandsHandler.AddCommand(command);
                        // deck.AddCardToDeck(craftedCard,DeckEnum.Hand);
                        if (isPlayer)
                            OnCraftingComboToHand?.Invoke(new CardData[] { craftedCard });
                        break;
                    case DeckEnum.PlayerDeck:
                    case DeckEnum.Discard:
                        var gotolocation = ComboSO.GoToDeckAfterCrafting;
                        command = new AddNewCardToDeck(gotolocation, craftedCard, deck);
                        dataCommandsHandler.AddCommand(command);
                        command = new DrawHandCommand(deck, 1);
                        dataCommandsHandler.AddCommand(command);

                        //   deck.AddCardToDeck( craftedCard, gotolocation);
                        //                        deck.DrawHand( 1);
                        break;

                    case DeckEnum.AutoActivate:

                        _cardExecutionManager.ForceExecuteCard(isPlayer, craftedCard);

                        //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
                        // (deck[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(craftedCard, false);
                        Debug.Log("DrawFrom 2");
                        deck.DrawHand(1);
                        break;
                    default:
                        Debug.LogWarning("crafting card Detected but the deck that he go after that is " + _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting.ToString());
                        break;
                }
            }
        }

        //   public  void StartDetection() => ThreadHandler.StartThread(new ThreadList(threadId,DetectRecipe, EndDetection));
        public void StartDetection() => DetectRecipe();
        private void EndDetection()
        {
            // need to change the logic!

            bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
            DeckHandler deck = _playersManager.GetCharacter(isPlayer).DeckHandler;


            if (_cardRecipeDetected == null || _cardRecipeDetected.ComboSO == null)
            {
                FoundCombo = false;
                deck.DrawHand(1);
            }
            else
            {
                FoundCombo = true;
                OnComboSucceeded?.Invoke(_cardRecipeDetected);
                ResetCraftingSlotCommand resetCraftingCommands = new ResetCraftingSlotCommand(_playersManager.GetCharacter(isPlayer).CraftingHandler);
                _gameCommands.GameDataCommands.DataCommands.AddCommand(resetCraftingCommands);
                TryForge(isPlayer);

                _cardRecipeDetected = null;
            }
            _isTryingToDetect = false;
            OnComboDetectedFinished?.Invoke();
        }

        private void DetectRecipe()
        {
            _isTryingToDetect = true;
            bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
            //coping the relevant crafting slots from the deck manager

            var data = _playersManager.GetCharacter(isPlayer).CraftingHandler;

            CheckRecipe(data.CardsTypeData, isPlayer);

            EndDetection();
        }
        void CheckRecipe(IEnumerable<CardTypeData> craftingItems, bool isPlayer)
        {
            // need to make algorithem better!!! 
            var recipes = _playersManager.GetCharacter(isPlayer).Combos;


            //  CardTypeData[] cardTypeDatas;
            for (int i = 0; i < recipes.Length; i++)
            {
                var comboSO = recipes[i].ComboSO;
                //  cardTypeDatas = new CardTypeData[comboSO.ComboSequence.Length];

                var toList = craftingItems.ToList();
                if (SequenceEquals(craftingItems,comboSO.ComboSequence))
                {
                    CardRecipeDetected = recipes[i];
                    //  Thread.Sleep(100);
                    return;
                }

            }

            CardRecipeDetected = null;

            bool SequenceEquals(IEnumerable<CardTypeData> current, IEnumerable<CardTypeData> other)
            {
                int length = current.Count();
                bool match = true;
                CardTypeData A;
                CardTypeData B;
                for (int i = 0; i < length; i++)
                {
                     A = current.ElementAt(i);
                     B = other.ElementAt(i);
                    match &= _cardTypeComparer.Equals(A,B);
                    if (!match)
                        break;
                }
                return match;
            }
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _gameTurnHandler = data.TurnHandler;
            _cardExecutionManager = data.CardExecutionManager;
            _playersManager = data.PlayersManager;
            var left = _playersManager.GetCharacter(true);
            var right = _playersManager.GetCharacter(false);
            OnComboDetectedFinished += left.StaminaHandler.CheckStaminaEmpty;
            OnComboDetectedFinished += right.StaminaHandler.CheckStaminaEmpty;

            left.CraftingHandler.OnComboDetectionRequired += StartDetection;
            //right.CraftingHandler.OnComboDetectionRequired += StartDetection;

            _gameCommands = data.GameCommands;

            data.OnBattleManagerDestroyed += BattleManagerDestroyed;
        }

        private void BattleManagerDestroyed(IBattleManager bm)
        {

            bm.OnBattleManagerDestroyed -= BattleManagerDestroyed;

            var left = _playersManager.GetCharacter(true);
            var right = _playersManager.GetCharacter(false);
            OnComboDetectedFinished -= left.StaminaHandler.CheckStaminaEmpty;
            OnComboDetectedFinished -= right.StaminaHandler.CheckStaminaEmpty;

            left.CraftingHandler.OnComboDetectionRequired -= StartDetection;
            //right.CraftingHandler.OnComboDetectionRequired -= StartDetection;

        }
    }
}
