using Battles.Deck;
using UnityEngine;
using Cards;
using System.Collections.Generic;

using ThreadsHandler;
using System.Linq;
using Unity.Events;
using Battles.UI;
using Unity.Collections;
using Collections;

namespace Combo
{
    public class ComboManager : MonoSingleton<ComboManager>
    {
      
        #region Fields
        [SerializeField] ComboRecipeCollectionSO _playerKnownRecipe;
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
        public ComboRecipeCollectionSO PlayerRelics => _playerKnownRecipe;

        #endregion

        public override void Init()
        {

            _playerKnownRecipe = Resources.Load<ComboRecipeCollectionSO>("CollectionSO/PlayerRecipe");
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

                Card crafted = Managers.CardManager.CreateCard(true, _cardRecipeDetected.CraftedCard.CardName);
             //   BattleUiManager.Instance.SetCardPosition(crafted);

                DeckManager.Instance.AddCardToDeck(crafted, _cardRecipeDetected.GoToDeckAfterCrafting);
                VFXManager.Instance.PlayParticle(true, BodyPartEnum.BottomBody, ParticleEffectsEnum.Crafting);
                _playSound?.Raise( SoundsNameEnum.SuccessfullForge);
            }
            else
            {
                if (DeckManager.GetCraftingSlots.GetAmountOfFilledSlots <= 0)
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


            DeckManager.GetCraftingSlots.ResetDeck();
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
            
            Card[] craftingSlots =new Card[DeckManager.GetCraftingSlots.GetDeck.Length];

            System.Array.Copy( DeckManager.GetCraftingSlots.GetDeck, craftingSlots, DeckManager.GetCraftingSlots.GetDeck.Length);

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
            List<CardTypeData> nextRecipe = new List<CardTypeData>(Instance._playerKnownRecipe.GetComboSO[0].ComboSequance.Length);
            for (int i = 0; i < Instance._playerKnownRecipe.GetComboSO.Length; i++)
            {
                for (int j = 0; j < Instance._playerKnownRecipe.GetComboSO[i].ComboSequance.Length; j++)
                {
                    nextRecipe.Add(Instance._playerKnownRecipe.GetComboSO[i].ComboSequance[j]);
                }
                if (craftingItems.SequenceEqual(nextRecipe , new CardTypeComparer()))
                {
                    Instance.CardRecipeDetected = Instance._playerKnownRecipe.GetComboSO[i];
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


