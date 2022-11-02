using Battle.Combo;
using Battle.Deck;
using Battle.Turns;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
using Cards;
using ReiTools.TokenMachine;
using System;
using System.Linq;
using UnityEngine;

namespace CardMaga.Battle.Combo
{
    public class ComboManager : ISequenceOperation<IBattleManager>
    {

        #region Events
        public event Action OnComboDetectedFinished;
        public event Action OnComboCraftSucceeded;
        public event Action<ComboData> OnComboSucceeded;
        public event Action<CardData[]> OnCraftingComboToHand;
        #endregion

        #region Fields

        private bool _isTryingToDetect;
        private IPlayersManager _playersManager;
        private TurnHandler _gameTurnHandler;
        private CardExecutionManager _cardExecutionManager;
        private CardTypeComparer _cardTypeComparer = new CardTypeComparer();
        private Factory.GameFactory.CardFactory _cardFactory;
        private GameCommands _gameCommands;
        #endregion

        #region Properties
        public bool IsTryingToDetect => _isTryingToDetect;


        public bool FoundCombo { get; internal set; }

        public int Priority => 0;


        #endregion
        #region MonoBehaviour Callbacks


        public ComboManager(IBattleManager battleManager)
        {
            battleManager.Register(this, OrderType.Default);
            _cardFactory = Factory.GameFactory.Instance.CardFactoryHandler;
        }
        #endregion
        #region Public Functions
        public void StartDetection() => DetectRecipe();
        #endregion

        #region Private Functions
        private void DetectRecipe()
        {
            _isTryingToDetect = true;

            bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
            IPlayer player = _playersManager.GetCharacter(isPlayer);
            ComboData[] characterCombos = player.Combos.GetCollection.ToArray();
            ComboData comboFound = null;

            if (characterCombos.Length > 0)
            {
                CraftingHandler craftingHandler = player.CraftingHandler;
                CardTypeData[] craftingCardType = craftingHandler.CardsTypeData.ToArray();

                for (int i = 0; i < characterCombos.Length; i++)
                {
                    if (CheckRecipe(characterCombos[i], craftingCardType))
                    {
                        comboFound = characterCombos[i];
                        break;
                    }
                }
            }

            EndDetection(player, comboFound);
        }

        private bool CheckRecipe(ComboData comboData, CardTypeData[] craftingCardType)
        {
            CardTypeData[] comboSequence = comboData.ComboSequence;

            bool comboFound = true;
            for (int i = 0; i < comboSequence.Length; i++)
            {
                comboFound &= _cardTypeComparer.Equals(comboSequence[i], craftingCardType[i]);

                if (!comboFound)
                    break;
            }


            return comboFound;
        }

        private void EndDetection(IPlayer player, ComboData comboFound)
        {

            DeckHandler deck = player.DeckHandler;
            if (comboFound == null || comboFound.ComboSO == null)
            {
                FoundCombo = false;
                deck.DrawHand(1);

            }
            else
            {
                FoundCombo = true;
                OnComboCraftSucceeded?.Invoke();
                OnComboSucceeded?.Invoke(comboFound);

                TryForge(player, comboFound);

                ResetCraftingSlotCommand resetCraftingCommands = new ResetCraftingSlotCommand(player.CraftingHandler);
                _gameCommands.GameDataCommands.DataCommands.AddCommand(resetCraftingCommands);

            }
            _isTryingToDetect = false;
            OnComboDetectedFinished?.Invoke();
        }

        private void TryForge(IPlayer isPlayer, ComboData comboData)
        {
            var ComboSO = comboData.ComboSO;

            var craftedCard = _cardFactory.CreateCard(ComboSO.CraftedCard.ID, comboData.Level);


            DeckHandler deck = isPlayer.DeckHandler;
            CommandHandler<ICommand> dataCommandsHandler = _gameCommands.GameDataCommands.DataCommands;
            ICommand command = null;

            switch (ComboSO.GoToDeckAfterCrafting)
            {
                case DeckEnum.Hand:
                    command = new AddNewCardToDeck(DeckEnum.Hand, craftedCard, deck);

                    dataCommandsHandler.AddCommand(command);
                    if (isPlayer.IsLeft)
                        OnCraftingComboToHand?.Invoke(new CardData[] { craftedCard });

                    break;


                case DeckEnum.PlayerDeck:
                case DeckEnum.Discard:

                    command = new AddNewCardToDeck(ComboSO.GoToDeckAfterCrafting, craftedCard, deck);
                    dataCommandsHandler.AddCommand(command);
                    command = new DrawHandCommand(deck, 1);
                    dataCommandsHandler.AddCommand(command);
                    break;


                case DeckEnum.AutoActivate:

                    _cardExecutionManager.ForceExecuteCard(isPlayer.IsLeft, craftedCard);
                    deck.DrawHand(1);
                    break;
                default:
                    Debug.LogWarning("crafting card Detected but the deck that he go after that is " + comboData.ComboSO.GoToDeckAfterCrafting.ToString());
                    break;
            }

        }
        #endregion

        #region Previous Combo Manager
        //public void TryForge(bool isPlayer)
        //{
        //    var ComboSO = _cardRecipeDetected.ComboSO;
        //    if (_cardRecipeDetected != null && ComboSO != null)
        //    {

        //        var craftedCard = _cardFactory.CreateCard(ComboSO.CraftedCard.ID, _cardRecipeDetected.Level);

        //        _successCrafting?.Raise();
        //        DeckHandler deck = _playersManager.GetCharacter(isPlayer).DeckHandler;
        //        switch (ComboSO.GoToDeckAfterCrafting)
        //        {
        //            case DeckEnum.Hand:
        //                deck.AddCardToDeck(craftedCard, DeckEnum.Hand);
        //                if (isPlayer)
        //                    OnCraftingComboToHand?.Invoke(new CardData[] { craftedCard });
        //                break;
        //            case DeckEnum.PlayerDeck:
        //            case DeckEnum.Discard:
        //                var gotolocation = ComboSO.GoToDeckAfterCrafting;
        //                deck.AddCardToDeck(craftedCard, gotolocation);
        //                Debug.Log("DrawFrom 1");
        //                deck.DrawHand(1);
        //                break;

        //            case DeckEnum.AutoActivate:

        //                _cardExecutionManager.ForceExecuteCard(isPlayer, craftedCard);

        //                //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
        //                // (deck[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(craftedCard, false);
        //                Debug.Log("DrawFrom 2");
        //                deck.DrawHand(1);
        //                break;
        //            default:
        //                Debug.LogWarning("crafting card Detected but the deck that he go after that is " + _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting.ToString());
        //                break;
        //        }
        //    }
        //}

        //void CheckRecipe()
        //{
        //    // need to make algorithem better!!! 
        //    var recipes = _playersManager.GetCharacter(isPlayer).Combos.GetCollection.ToArray();


        //    CardTypeData[] cardTypeDatas;
        //    for (int i = 0; i < recipes.Length; i++)
        //    {
        //        var comboSO = recipes[i].ComboSO;
        //        cardTypeDatas = new CardTypeData[comboSO.ComboSequence.Length];

        //        for (int j = 0; j < comboSO.ComboSequence.Length; j++)
        //        {
        //            cardTypeDatas[j] = comboSO.ComboSequence[j];
        //            //nextRecipe.Add(combo[i].ComboSequance[j]);
        //        }
        //        if (craftingItems.SequenceEqual(cardTypeDatas, _cardTypeComparer))
        //        {
        //            CardRecipeDetected = recipes[i];
        //            //  Thread.Sleep(100);
        //            return;
        //        }

        //    }
        //    CardRecipeDetected = null;
        //}

        //private void EndDetection()
        //{
        //    // need to change the logic!

        //    bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
        //    DeckHandler deck = _playersManager.GetCharacter(isPlayer).DeckHandler;


        //    if (_cardRecipeDetected == null || _cardRecipeDetected.ComboSO == null)
        //    {
        //        FoundCombo = false;
        //        Debug.Log("DrawFrom 3");
        //        deck.DrawHand(1);
        //    }
        //    else
        //    {
        //        FoundCombo = true;
        //        OnComboSucceeded?.Invoke(_cardRecipeDetected);
        //        // _craftingUIHandler.MarkSlotsDetected();
        //        _playersManager.GetCharacter(isPlayer).CraftingHandler.ResetCraftingSlots();
        //        TryForge(isPlayer);

        //        _cardRecipeDetected = null;
        //    }
        //    _isTryingToDetect = false;
        //    OnComboDetectedFinished?.Invoke();
        //}


        #endregion


        #region ISequenceOperation<IBattleManager> Implementation

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _gameTurnHandler = data.TurnHandler;
            _cardExecutionManager = data.CardExecutionManager;
            _gameCommands = data.GameCommands;
            _playersManager = data.PlayersManager;

            var left  = _playersManager.GetCharacter(true);
            var right = _playersManager.GetCharacter(false);

            OnComboDetectedFinished += left.StaminaHandler.CheckStaminaEmpty;
            OnComboDetectedFinished += right.StaminaHandler.CheckStaminaEmpty;

            left.CraftingHandler.OnComboDetectionRequired += StartDetection;
            right.CraftingHandler.OnComboDetectionRequired += StartDetection;


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
            right.CraftingHandler.OnComboDetectionRequired -= StartDetection;

        }
        #endregion
    }
}
