using System;
using Battle.Deck;
using Battle.UI;
using Cards;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using System.Linq;
using ThreadsHandler;
using Unity.Events;
using UnityEngine;
using Battle.Combo;
using CardMaga.Card;
using CardMaga.SequenceOperation;
using Battle.Turns;
using System.Threading;

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
                switch (ComboSO.GoToDeckAfterCrafting)
                {
                    case DeckEnum.Hand:
                        deck.AddCardToDeck(craftedCard,DeckEnum.Hand);
                        if (isPlayer)
                            OnCraftingComboToHand?.Invoke(new CardData[]{craftedCard});
                        break;
                    case DeckEnum.PlayerDeck:
                    case DeckEnum.Discard:
                        var gotolocation = ComboSO.GoToDeckAfterCrafting;
                        deck.AddCardToDeck( craftedCard, gotolocation);
                        Debug.Log("DrawFrom 1");
                        deck.DrawHand( 1);
                        break;

                    case DeckEnum.AutoActivate:

                        _cardExecutionManager.ForceExecuteCard(isPlayer,craftedCard);
                
                        //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
                       // (deck[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(craftedCard, false);
                        Debug.Log("DrawFrom 2");
	                    deck.DrawHand( 1);
                        break;
                    default:
                        Debug.LogWarning("crafting card Detected but the deck that he go after that is " + _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting.ToString());
                        break;
                }
            }
        }

     //   public  void StartDetection() => ThreadHandler.StartThread(new ThreadList(threadId,DetectRecipe, EndDetection));
        public  void StartDetection() => DetectRecipe();
        private  void EndDetection()
        {
            // need to change the logic!

            bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
            DeckHandler deck = _playersManager.GetCharacter(isPlayer).DeckHandler;


            if (_cardRecipeDetected == null || _cardRecipeDetected.ComboSO == null)
            {
                FoundCombo = false;
                Debug.Log("DrawFrom 3");
                deck.DrawHand( 1);
            }
            else
            {
                FoundCombo = true;
                OnComboSucceeded?.Invoke(_cardRecipeDetected);
               // _craftingUIHandler.MarkSlotsDetected();
                _playersManager.GetCharacter(isPlayer).CraftingHandler.ResetCraftingSlots();
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

            CardTypeData[] craftingSlots = new CardTypeData[data.LengthSize];

            System.Array.Copy(data.CardsTypeData.ToArray(), craftingSlots, data.LengthSize);

            //  System.Array.Reverse(craftingSlots);

            // checking how many of them are not null
            int amountCache = 0;
            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if (craftingSlots[i] != null)
                    amountCache++;
            }

            List<CardTypeData> craftingItems = new List<CardTypeData>(amountCache);

            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if (craftingSlots[i] != null)
                    craftingItems.Add(craftingSlots[i]);

            }
            if (amountCache > 1)
            {
                CheckRecipe(craftingItems, isPlayer);
            }
            EndDetection();
        }
         void CheckRecipe(IReadOnlyList<CardTypeData> craftingItems, bool isPlayer)
        {
            // need to make algorithem better!!! 
            var recipes = _playersManager.GetCharacter(isPlayer).Combos;

        
            CardTypeData[] cardTypeDatas;
            for (int i = 0; i < recipes.Length; i++)
            {
                var comboSO = recipes[i].ComboSO;
                cardTypeDatas = new CardTypeData[comboSO.ComboSequence.Length];

                for (int j = 0; j < comboSO.ComboSequence.Length; j++)
                {
                    cardTypeDatas[j] = comboSO.ComboSequence[j];
                    //nextRecipe.Add(combo[i].ComboSequance[j]);
                }
                if (craftingItems.SequenceEqual(cardTypeDatas, _cardTypeComparer))
                {
                    CardRecipeDetected = recipes[i];
                  //  Thread.Sleep(100);
                    return;
                }

            }
            CardRecipeDetected = null;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _gameTurnHandler = data.TurnHandler;
            _cardExecutionManager = data.CardExecutionManager;
            _playersManager = data.PlayersManager;
            var left = _playersManager.GetCharacter(true);
            var right = _playersManager.GetCharacter(false);
            OnComboDetectedFinished += left.StaminaHandler.CheckStaminaEmpty;
            OnComboDetectedFinished +=right.StaminaHandler.CheckStaminaEmpty;

            left.CraftingHandler.OnComboDetectionRequired += StartDetection;
            //right.CraftingHandler.OnComboDetectionRequired += StartDetection;
          


            data.OnBattleManagerDestroyed += BattleManagerDestroyed;
        }
   
        private void BattleManagerDestroyed(IBattleManager bm)
        {

            bm.OnBattleManagerDestroyed -= BattleManagerDestroyed;

            var left =  _playersManager.GetCharacter(true);
            var right = _playersManager.GetCharacter(false);
            OnComboDetectedFinished -= left.StaminaHandler.CheckStaminaEmpty;
            OnComboDetectedFinished -= right.StaminaHandler.CheckStaminaEmpty;

            left.CraftingHandler.OnComboDetectionRequired  -= StartDetection;
            //right.CraftingHandler.OnComboDetectionRequired -= StartDetection;

        }
    }
}
