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
using Managers;
using Battle.Turns;

namespace Battle
{
    public class ComboManager : MonoSingleton<ComboManager>, ISequenceOperation<BattleManager>
    {

        #region Fields

        public event Action<CardData[]> OnCraftingComboToHand;

        [SerializeField] Combo.Combo _cardRecipeDetected;
        [SerializeField] VFXSO _comboVFX;

        private PlayersManager _playersManager;
        private GameTurnHandler _gameTurnHandler;
        static byte threadId;

        #endregion
        #region Events
        [SerializeField] VoidEvent _successCrafting;
        #endregion

        #region Properties

        public Battle.Combo.Combo CardRecipeDetected
        {
            get => _cardRecipeDetected; set
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
            PlayerCraftingSlots.OnDetectComboRequire += StartDetection;
            BattleManager.Register(this, OrderType.Default);
        }
        private void OnDestroy()
        {
            PlayerCraftingSlots.OnDetectComboRequire -= StartDetection;
        }
        public void Start()
        {
            threadId = ThreadHandler.GetNewID;
        }

        public void TryForge(bool isPlayer)
        {
            var ComboSO = _cardRecipeDetected.ComboSO;
            if (_cardRecipeDetected != null && ComboSO != null)
            {
                var factory = Factory.GameFactory.Instance.CardFactoryHandler;
                var craftedCard = factory.CreateCard(ComboSO.CraftedCard.ID, _cardRecipeDetected.Level);

                _successCrafting?.Raise();
                DeckHandler deck = _playersManager.GetCharacter(isPlayer).DeckHandler;
                switch (ComboSO.GoToDeckAfterCrafting)
                {
                    case DeckEnum.Hand:
                        if (isPlayer)
                            OnCraftingComboToHand?.Invoke(new CardData[]{craftedCard});

                        deck.AddCardToDeck(craftedCard,DeckEnum.Hand);
                        break;
                    case DeckEnum.PlayerDeck:
                    case DeckEnum.Discard:
                        var gotolocation = ComboSO.GoToDeckAfterCrafting;
                        deck.AddCardToDeck( craftedCard, gotolocation);
                        deck.DrawHand( 1);
                        break;

                    case DeckEnum.AutoActivate:

                        Battle.CardExecutionManager.Instance.RegisterCard(craftedCard, isPlayer);
                        //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
                        (deck[DeckEnum.CraftingSlots] as PlayerCraftingSlots).AddCard(craftedCard, false);
                       deck.DrawHand( 1);
                        break;
                    default:
                        Debug.LogWarning("crafting card Detected but the deck that he go after that is " + _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting.ToString());
                        break;
                }
                // Need To be Re-Done
                //var battledata = Account.AccountManager.Instance.BattleData;
                //var sounds = (isPlayer) ? battledata.Player.CharacterData.CharacterSO.ComboSounds : battledata.Opponent.CharacterData.CharacterSO.ComboSounds;
                //sounds?.PlaySound();
            }

            // CreateCard();

            // get the crafting deck


            /*
             * run on the possiblities from low to high
             * 
             * return true if found option
             * 
            * return false if nothign found 
             */
        }

        public  void StartDetection() => ThreadHandler.StartThread(new ThreadList(threadId,DetectRecipe, EndDetection));
        private  void EndDetection()
        {
            // need to change the logic!

            bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
            var deck = _playersManager.GetCharacter(isPlayer).DeckHandler;
            var data = deck[DeckEnum.CraftingSlots] as PlayerCraftingSlots;

        //    var _craftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);

            if (Instance._cardRecipeDetected == null || Instance._cardRecipeDetected.ComboSO == null)
            {
                FoundCombo = false;
               deck.DrawHand( 1);
            }
            else
            {
                FoundCombo = true;
               // _craftingUIHandler.MarkSlotsDetected();
                Instance.TryForge(isPlayer);

                Instance._cardRecipeDetected = null;
            data.ResetDeck();
            }
        }

         void DetectRecipe()
        {
            bool isPlayer = _gameTurnHandler.IsLeftCharacterTurn;
            //coping the relevant crafting slots from the deck manager
            var deck = _playersManager.GetCharacter(isPlayer).DeckHandler;
            var data = deck[DeckEnum.CraftingSlots] as PlayerCraftingSlots;

            CardData[] craftingSlots = new CardData[data.GetDeck.Length];

            System.Array.Copy(data.GetDeck, craftingSlots, data.GetDeck.Length);

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

                    craftingItems.Add(craftingSlots[i].CardSO.CardType);

            }
            if (amountCache > 1)
            {
                CheckRecipe(craftingItems.ToArray(), isPlayer);
            }

        }
        static void CheckRecipe(CardTypeData[] craftingItems, bool isPlayer)
        {
            // need to make algorithem better!!! 
            var recipes = isPlayer ? Managers.PlayerManager.Instance.Combos : Battle.EnemyManager.Instance.Combos;

            CardTypeComparer comparer = new CardTypeComparer();
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
                if (craftingItems.SequenceEqual(cardTypeDatas, comparer))
                {
                    Instance.CardRecipeDetected = recipes[i];
                    return;
                }

            }
            Instance.CardRecipeDetected = null;
        }

        public void ExecuteTask(ITokenReciever tokenMachine, BattleManager data)
        {
            _gameTurnHandler = data.TurnHandler;
            _playersManager = data.PlayersManager;
        }
    }
}
