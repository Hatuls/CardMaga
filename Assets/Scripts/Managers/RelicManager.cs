using Battles.Deck;
using UnityEngine;
using Cards;
using System.Threading;
using Battles.UI;
using System.Collections.Generic;
using Collections.RelicsSO;
using ThreadsHandler;
namespace Relics
{
    public class RelicManager : MonoSingleton<RelicManager>
    {
        #region Fields
        RelicCollectionSO playerKnownRecipe;
        #endregion



        public override void Init()
        {
            playerKnownRecipe = Resources.Load<RelicCollectionSO>("ComboRecipe/PlayerRecipe");
        }


        public bool TryForge()
        {
            // get the crafting deck
            //       DeckManager.GetCraftingSlots;

            /*
             * run on the possiblities from low to high
             * 
             * return true if found option
             * 
            * return false if nothign found 
             */
            return true;
        }

    }
}


