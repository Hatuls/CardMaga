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
        PlayerCraftingSlots _playerCraftingSlots;
        PlayerCraftingSlots _enemyCraftingSlots;
        static byte threadId;

        #endregion
        #region Events
        [SerializeField] SoundsEvent _playSound;
        [SerializeField] VoidEvent _successCrafting;
        #endregion

        #region Properties

        public ComboSO CardRecipeDetected
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

            if (_cardRecipeDetected != null)
            {
            
                var craftedCard = Managers.CardManager.Instance.CreateCard(_cardRecipeDetected.CraftedCard);
                _playSound?.Raise(SoundsNameEnum.SuccessfullForge);
                _successCrafting?.Raise();

                switch (_cardRecipeDetected.GoToDeckAfterCrafting)
                {
                    case DeckEnum.Hand:
                    case DeckEnum.PlayerDeck:
                    case DeckEnum.Disposal:
                        var gotolocation = _cardRecipeDetected.GoToDeckAfterCrafting;
                        DeckManager.Instance.AddCardToDeck(isPlayer, craftedCard, gotolocation);

                        if (isPlayer)
                        {
                            if (gotolocation == DeckEnum.Hand)
                                CardUIManager.Instance.CraftCardUI(craftedCard, gotolocation);
                            else
                                DeckManager.Instance.DrawHand(true, 1);
                        }

                      
                        break;

                    case DeckEnum.AutoActivate:
           
                        Battles.CardExecutionManager.Instance.RegisterCard(craftedCard, isPlayer);
                      //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
         //               DeckManager.GetCraftingSlots(isPlayer).AddCard(craftedCard,false);
                        break;
                    default:
                        Debug.LogWarning("crafting card Detected but the deck that he go after that is " + _cardRecipeDetected.GoToDeckAfterCrafting.ToString());
                        break;
                }
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
            bool isPlayer = Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn;

            var _craftingUIHandler = CraftingUIManager.Instance.GetCharacterUIHandler(isPlayer);

            if (Instance._cardRecipeDetected == null)
            {
                FoundCombo = false;
                if(isPlayer)
                DeckManager.Instance.DrawHand(true, 1);
                _craftingUIHandler.ResetSlotsDetection();

            }
            else
            {
                FoundCombo = true;
                Instance.StartCoroutine(Instance.OnDetectionOfCombo(isPlayer, _craftingUIHandler));
            }

        }
        System.Collections.IEnumerator OnDetectionOfCombo(bool isPlayer,CraftingUIHandler craftingUIHandler)
        {
            yield return null;
            craftingUIHandler.MarkSlotsDetected();
            yield return new WaitForSeconds(0.15f);
            VFXManager.Instance.PlayParticle(isPlayer, BodyPartEnum.BottomBody, ParticleEffectsEnum.Crafting);
            yield return new WaitForSeconds(0.15f);
            TryForge(isPlayer);
            DeckManager.GetCraftingSlots(isPlayer).ResetDeck();
            _cardRecipeDetected = null;
        }
        static void DetectRecipe()
        {
            bool isPlayer = Battles.Turns.TurnHandler.CurrentState == Battles.Turns.TurnState.PlayerTurn;
            //coping the relevant crafting slots from the deck manager
            Card[] craftingSlots = new Card[DeckManager.GetCraftingSlots(isPlayer).GetDeck.Length];

            System.Array.Copy(DeckManager.GetCraftingSlots(isPlayer).GetDeck, craftingSlots, DeckManager.GetCraftingSlots(isPlayer).GetDeck.Length);

            System.Array.Reverse(craftingSlots);

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
           
            var recipes = isPlayer ? Managers.PlayerManager.Instance.Recipes : Battles.EnemyManager.Instance.Recipes;
            ComboSO[] combo = new ComboSO[recipes.Length];

            for (int i = 0; i < recipes.Length; i++)
                combo[i] = (recipes[i].ComboRecipe);

            CardTypeData[] cardTypeDatas;
            for (int i = 0; i < combo.Length; i++)
            {
                cardTypeDatas = new CardTypeData[combo[i].ComboSequance.Length];

                for (int j = 0; j < combo[i].ComboSequance.Length; j++)
                {
                    cardTypeDatas[j] = combo[i].ComboSequance[j];
                    //nextRecipe.Add(combo[i].ComboSequance[j]);
                }
                if (craftingItems.SequenceEqual(cardTypeDatas, new CardTypeComparer()))
                {
                    Instance.CardRecipeDetected = combo[i];
                    return;
                }
              
            }
            Instance.CardRecipeDetected = null;
        }
    }
}
