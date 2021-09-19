using Battles.Deck;
using UnityEngine;
using Cards;
using System.Collections.Generic;
using Collections.RelicsSO;
using ThreadsHandler;
using System.Linq;
using Unity.Events;
using Battles.UI;

namespace Combo
{
    public class ComboManager : MonoSingleton<ComboManager>
    {
      
        #region Fields
     
        [SerializeField] ComboSO _cardRecipeDetected;

        static byte threadId;

        #endregion
        #region Events
        [SerializeField] SoundsEvent _playSound;
        [SerializeField] VoidEvent _successCrafting;
        #endregion

        #region Properties
    
        public ComboSO CardRecipeDetected { get => _cardRecipeDetected; set
            {
                _cardRecipeDetected = value;
            }
        }


        #endregion

        public override void Init()
        {
            threadId = ThreadHandler.GetNewID;
        }
        void CreateCard()
        {

            Debug.Log(_cardRecipeDetected);

            Debug.Log(_cardRecipeDetected?.ComboName);

            if(_cardRecipeDetected != null)
            {
                //create card
                _successCrafting?.Raise();

                Card crafted = Managers.CardManager.Instance.CreateCard(_cardRecipeDetected.CraftedCard);
             //   BattleUiManager.Instance.SetCardPosition(crafted);

                DeckManager.Instance.AddCardToDeck(true,crafted, _cardRecipeDetected.GoToDeckAfterCrafting);
                VFXManager.Instance.PlayParticle(true, BodyPartEnum.BottomBody, ParticleEffectsEnum.Crafting);
                _playSound?.Raise( SoundsNameEnum.SuccessfullForge);
            }
            else
            {
                if (DeckManager.GetCraftingSlots(true).GetAmountOfFilledSlots <= 0)
                {
             
                   _playSound?.Raise( SoundsNameEnum.Reject);
                    //reject request for forging
                    return;
                }
                else
                {
                    _playSound?.Raise(SoundsNameEnum.BurningSound);
                }
            }


            DeckManager.GetCraftingSlots(true).ResetDeck();
            _cardRecipeDetected = null;
        }
        public void TryForge()
        {

            if (Battles.Turns.TurnHandler.CurrentState != Battles.Turns.TurnState.PlayerTurn)
                return;

            CreateCard();

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
              var _craftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(true);

            if (Instance._cardRecipeDetected == null)
             _craftingUIHandler.ResetSlotsDetection();
                
            else
              _craftingUIHandler.MarkSlotsDetected();
            
        }
        static void DetectRecipe()
        {
            
            Card[] craftingSlots =new Card[DeckManager.GetCraftingSlots(true).GetDeck.Length];

            System.Array.Copy( DeckManager.GetCraftingSlots(true).GetDeck, craftingSlots, DeckManager.GetCraftingSlots(true).GetDeck.Length);

            System.Array.Reverse(craftingSlots);

            int amountCache=0;
            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if (craftingSlots[i] != null)
                    amountCache++;
            }

            List<CardTypeData> craftingItems = new List<CardTypeData>(amountCache);

            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if(craftingSlots[i] != null)
                {
                    craftingItems.Add(craftingSlots[i].CardSO.CardType);
                }
            }
            if(craftingItems.Count > 1)
            {
                CheckRecipe( craftingItems);
            }
         
        }
    static void CheckRecipe(List<CardTypeData> craftingItems)
        {
            List<CardTypeData> nextRecipe = new List<CardTypeData>();

            var recipes = Managers.PlayerManager.Instance.Recipes;
            ComboSO[] combo = new ComboSO[recipes.Length];
            for (int i = 0; i < recipes.Length; i++)
                combo[i] = (recipes[i].ComboRecipe);
            


            
            for (int i = 0; i < combo.Length; i++)
            {
                for (int j = 0; j < combo[i].ComboSequance.Length; j++)
                {
                    nextRecipe.Add(combo[i].ComboSequance[j]);
                }
                if (craftingItems.SequenceEqual(nextRecipe , new CardTypeComparer()))
                {
                    Instance.CardRecipeDetected = combo[i];
                    return;
                }
                else
                {
                    nextRecipe.Clear();
                }
            }
            Instance.CardRecipeDetected = null;
        }
    }
}


