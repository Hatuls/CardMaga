using Battles.Deck;
using UnityEngine;
using Cards;
using System.Collections.Generic;
using Collections;
using ThreadsHandler;
using System.Linq;
using Unity.Events;
using Battles.UI;
using Managers;

namespace Combo
{
    public class ComboManager : MonoSingleton<ComboManager>
    {

        #region Fields
        [SerializeField] Combo _cardRecipeDetected;
        [SerializeField] VFXSO _comboVFX;
        PlayerCraftingSlots _playerCraftingSlots;
        PlayerCraftingSlots _enemyCraftingSlots;
        static byte threadId;

        #endregion
        #region Events
        [SerializeField] VoidEvent _successCrafting;
        #endregion

        #region Properties

        public Combo CardRecipeDetected
        {
            get => _cardRecipeDetected; set
            {
                _cardRecipeDetected = value;
            }
        }

        public  static bool FoundCombo { get; internal set; }


        #endregion

        public override void Init()
        {
            threadId = ThreadHandler.GetNewID;
        }
        
        public void TryForge(bool isPlayer)
        {

            if (_cardRecipeDetected != null && _cardRecipeDetected.ComboSO != null)
            {
                var factory = Factory.GameFactory.Instance.CardFactoryHandler;
                var craftedCard = factory.CreateCard(_cardRecipeDetected.ComboSO.CraftedCard,_cardRecipeDetected.Level);

                _successCrafting?.Raise();

                switch (_cardRecipeDetected.ComboSO.GoToDeckAfterCrafting)
                {
                    case DeckEnum.Hand:
                        DeckManager.Instance.AddCardToDeck(isPlayer, craftedCard, _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting);

                        CardUIManager.Instance.UpdateHand();
                        break;
                    case DeckEnum.PlayerDeck:
                    case DeckEnum.Disposal:
                        var gotolocation = _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting;
                        DeckManager.Instance.AddCardToDeck(isPlayer, craftedCard, gotolocation);
                        DeckManager.Instance.DrawHand(isPlayer, 1);
                        break;

                    case DeckEnum.AutoActivate:
           
                        Battles.CardExecutionManager.Instance.RegisterCard(craftedCard, isPlayer);
                      //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
                        DeckManager.GetCraftingSlots(isPlayer).AddCard(craftedCard,false);
                        DeckManager.Instance.DrawHand(isPlayer, 1);
                        break;
                    default:
                        Debug.LogWarning("crafting card Detected but the deck that he go after that is " + _cardRecipeDetected.ComboSO.GoToDeckAfterCrafting.ToString());
                        break;
                }

                var battledata = Account.AccountManager.Instance.BattleData;
                var sounds = (isPlayer) ? battledata.Player.CharacterData.CharacterSO.ComboSounds : battledata.Opponent.CharacterData.CharacterSO.ComboSounds;
                sounds?.PlaySound();
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

        public static void StartDetection() => ThreadHandler.StartThread(new ThreadList(threadId, () => DetectRecipe(), () => EndDetection()));
        private static void EndDetection()
        {
            // need to change the logic!
            var state = Battles.Turns.TurnHandler.CurrentState;
            bool isPlayer = state == Battles.Turns.TurnState.PlayerTurn || state == Battles.Turns.TurnState.StartPlayerTurn || state == Battles.Turns.TurnState.EndPlayerTurn;

            var _craftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);

            if (Instance._cardRecipeDetected == null || Instance._cardRecipeDetected.ComboSO == null)
            {
                FoundCombo = false;
                _craftingUIHandler.ResetSlotsDetection();
                DeckManager.Instance.DrawHand(isPlayer, 1);
            }
            else
            {
                FoundCombo = true;
                _craftingUIHandler.MarkSlotsDetected();
                Instance.TryForge(isPlayer);
                VFXManager.Instance.PlayParticle(isPlayer, BodyPartEnum.BottomBody,Instance._comboVFX);
                DeckManager.GetCraftingSlots(isPlayer).ResetDeck();
                Instance. _cardRecipeDetected = null;
            }

        }
  
        static void DetectRecipe()
        {
            bool isPlayer = Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn;
            //coping the relevant crafting slots from the deck manager
            Card[] craftingSlots = new Card[DeckManager.GetCraftingSlots(isPlayer).GetDeck.Length];

            System.Array.Copy(DeckManager.GetCraftingSlots(isPlayer).GetDeck, craftingSlots, DeckManager.GetCraftingSlots(isPlayer).GetDeck.Length);

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
            var recipes = isPlayer ? Managers.PlayerManager.Instance.Recipes : Battles.EnemyManager.Instance.Recipes;
           

            CardTypeData[] cardTypeDatas;
            for (int i = 0; i < recipes.Length; i++)
            {
                var comboSO = recipes[i].ComboSO;
                cardTypeDatas = new CardTypeData[comboSO.ComboSequance.Length];

                for (int j = 0; j < comboSO.ComboSequance.Length; j++)
                {
                    cardTypeDatas[j] = comboSO.ComboSequance[j];
                    //nextRecipe.Add(combo[i].ComboSequance[j]);
                }
                if (craftingItems.SequenceEqual(cardTypeDatas, new CardTypeComparer()))
                {
                    Instance.CardRecipeDetected = recipes[i];
                    return;
                }
              
            }
            Instance.CardRecipeDetected = null;
        }
    }
}
