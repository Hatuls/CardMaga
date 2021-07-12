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
        [SerializeField] SoundsEvent _playSound;
        [SerializeField] VoidEvent _successCrafting;
        #endregion


        public override void Init()
        {
            _playerKnownRecipe = Resources.Load<RelicCollectionSO>("ComboRecipe/PlayerRecipe");
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
                if (DeckManager.GetCraftingSlots.GetAmountOfFilledSlots <= 1)
                {
             
                   _playSound?.Raise( SoundsNameEnum.Reject);
                    //reject request for forging
                    return;
                }
                else
                {
                    _playSound?.Raise(SoundsNameEnum.Reject);
                }
            }
            DeckManager.GetCraftingSlots.ResetDeck();
            _cardRecipeDetected = null;
        }
        public void TryForge()
        {

            if (Battles.Turns.TurnHandler.CurrentState != Battles.Turns.TurnState.PlayerTurn)
                return;


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
            Card[] craftingSlots = new  Card[DeckManager.GetCraftingSlots.GetDeck.Length];
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
        void CheckRecipe(ref List<CardType> craftingItems)
        {
            List<CardType> nextRecipe = new List<CardType>();
            for (int i = 0; i < _playerKnownRecipe.GetRelicSO.Length; i++)
            {
                for (int j = 0; j < _playerKnownRecipe.GetRelicSO[i].GetCombo.Length; j++)
                {
                    nextRecipe.Add(_playerKnownRecipe.GetRelicSO[i].GetCombo[j]);
                }
                if (craftingItems.SequenceEqual(nextRecipe , new CardTypeComparaer()))
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


