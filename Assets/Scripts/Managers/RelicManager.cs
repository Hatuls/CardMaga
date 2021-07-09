using Battles.Deck;
using UnityEngine;
using Cards;
using System.Collections.Generic;
using Collections.RelicsSO;
using ThreadsHandler;
using System.Linq;
using Unity.Events;
namespace Relics
{
    public class RelicManager : MonoSingleton<RelicManager>
    {
        #region Fields
        RelicCollectionSO _playerKnownRecipe;
        RelicSO _cardRecipeDetected;
        #endregion
        #region Events
        [SerializeField] SoundsEvent playSound;
        #endregion


        public override void Init()
        {
            _playerKnownRecipe = Resources.Load<RelicCollectionSO>("ComboRecipe/PlayerRecipe");
        }
        void CreateCard()
        {
            if(_cardRecipeDetected != null)
            {
                //Managers.CardManager.CreateCard(true,_cardRecipeDetected)
                //create card
            }
            else
            {
                if (DeckManager.GetCraftingSlots.GetAmountOfFilledSlots <= 1)
                {
                    //playSound?.Raise();
                    //reject request for forging
                    return;
                }
            }
            DeckManager.GetCraftingSlots.ResetDeck();
            _cardRecipeDetected = null;
        }
        public void TryForge()
        {
            // get the crafting deck
            byte id = ThreadHandler.GetNewID;
            ThreadHandler.StartThread(new ThreadList(id, () => DetectRecipe(),() => CreateCard()));
            /*
             * run on the possiblities from low to high
             * 
             * return true if found option
             * 
            * return false if nothign found 
             */
        }
        void DetectRecipe()
        {
            var craftingSlots = DeckManager.GetCraftingSlots.GetDeck;
            List<BodyPartEnum> craftingItems = new List<BodyPartEnum>();
            for (int i = 0; i < craftingItems.Count; i++)
            {
                if(craftingSlots[i] != null)
                {
                    craftingItems.Add(craftingSlots[i].GetSetCard.GetBodyPartEnum);
                }
            }
            if(craftingItems.Count > 1)
            {
                CheckRecipe(ref craftingItems);
            }
        }
        void CheckRecipe(ref List<BodyPartEnum> craftingItems)
        {
            List<BodyPartEnum> nextRecipe = new List<BodyPartEnum>();
            for (int i = 0; i < _playerKnownRecipe.GetRelicSO.Length; i++)
            {
                for (int j = 0; j < _playerKnownRecipe.GetRelicSO[i].GetCombo.Length; j++)
                {
                    nextRecipe.Add(_playerKnownRecipe.GetRelicSO[i].GetCombo[j]);
                }
                if (craftingItems.SequenceEqual(nextRecipe))
                {
                    _cardRecipeDetected = _playerKnownRecipe.GetRelicSO[i];
                    return;
                }
                else
                {
                    nextRecipe.Clear();
                }
            }
        }
    }
}


