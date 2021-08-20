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
        [SerializeField] ComboCollectionSO _playerKnownRecipe;
        [SerializeField] ComboSO _cardRecipeDetected;
        [SerializeField] CraftingUIHandler _craftingUIHandler;
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
        public ComboCollectionSO PlayerRelics => _playerKnownRecipe;

        #endregion

        public override void Init()
        {
            _playerKnownRecipe = Resources.Load<ComboCollectionSO>("ComboRecipe/PlayerRecipe");
            threadId = ThreadHandler.GetNewID;
        }
        void CreateCard()
        {

            Debug.Log(_cardRecipeDetected);

            Debug.Log(_cardRecipeDetected?.GetRelicName);

            if(_cardRecipeDetected != null)
            {
                //create card
                _successCrafting?.Raise();
                Card crafted = Managers.CardManager.CreateCard(true, _cardRecipeDetected.GetCraftedCard.GetCardName);
                DeckManager.Instance.AddCardToDeck(crafted, DeckEnum.Hand);
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

            if (Instance._cardRecipeDetected == null)
            {
                Instance._craftingUIHandler.ResetSlotsDetection();
                
            }
            else
            {
                Instance._craftingUIHandler.MarkSlotsDetected();
            }
        }
        static void DetectRecipe()
        {
            Card[] craftingSlots =new Card[DeckManager.GetCraftingSlots.GetDeck.Length];

            System.Array.Copy( DeckManager.GetCraftingSlots.GetDeck, craftingSlots, DeckManager.GetCraftingSlots.GetDeck.Length);

            System.Array.Reverse(craftingSlots);

            List<CardType> craftingItems = new List<CardType>();
            for (int i = 0; i < craftingSlots.Length; i++)
            {
                if(craftingSlots[i] != null)
                {
                    craftingItems.Add(craftingSlots[i].GetSetCard.GetCardType);
                }
            }
            if(craftingItems.Count > 1)
            {
                CheckRecipe(ref craftingItems);
            }
        }
    static void CheckRecipe(ref List<CardType> craftingItems)
        {
            List<CardType> nextRecipe = new List<CardType>();
            for (int i = 0; i < Instance._playerKnownRecipe.GetComboSO.Length; i++)
            {
                for (int j = 0; j < Instance._playerKnownRecipe.GetComboSO[i].GetCombo.Length; j++)
                {
                    nextRecipe.Add(Instance._playerKnownRecipe.GetComboSO[i].GetCombo[j]);
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


