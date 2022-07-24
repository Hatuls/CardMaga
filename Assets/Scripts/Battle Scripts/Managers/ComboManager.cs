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

namespace Battle
{
    public class ComboManager : MonoSingleton<ComboManager>
    {

        #region Fields

        public event Action<CardData[]> OnCraftingComboToHand;

        [SerializeField] Combo.Combo _cardRecipeDetected;
        [SerializeField] VFXSO _comboVFX;
        PlayerCraftingSlots _playerCraftingSlots;
        PlayerCraftingSlots _enemyCraftingSlots;
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


        #endregion

        public override void Init(ITokenReciever token)
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

                switch (ComboSO.GoToDeckAfterCrafting)
                {
                    case DeckEnum.Hand:
                        if (isPlayer)
                            OnCraftingComboToHand?.Invoke(new CardData[]{craftedCard});
                        
                        DeckManager.Instance.AddCardToDeck(isPlayer,craftedCard,DeckEnum.Hand);
                        break;
                    case DeckEnum.PlayerDeck:
                    case DeckEnum.Discard:
                        var gotolocation = ComboSO.GoToDeckAfterCrafting;
                        DeckManager.Instance.AddCardToDeck(isPlayer, craftedCard, gotolocation);
                        DeckManager.Instance.DrawHand(isPlayer, 1);
                        break;

                    case DeckEnum.AutoActivate:

                        Battle.CardExecutionManager.Instance.RegisterCard(craftedCard, isPlayer);
                        //  DeckManager.AddToCraftingSlot(isPlayer, craftedCard);
                        DeckManager.GetCraftingSlots(isPlayer).AddCard(craftedCard, false);
                        DeckManager.Instance.DrawHand(isPlayer, 1);
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

        public static void StartDetection() => ThreadHandler.StartThread(new ThreadList(threadId,DetectRecipe, EndDetection));
        private static void EndDetection()
        {
            // need to change the logic!
            var state = Battle.Turns.TurnHandler.CurrentState;
            bool isPlayer = state == Battle.Turns.TurnState.PlayerTurn || state == Battle.Turns.TurnState.StartPlayerTurn || state == Battle.Turns.TurnState.EndPlayerTurn;

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
                var tuffle = VFXManager.Instance.RecieveParticleSystemVFX(isPlayer, Instance._comboVFX);
                tuffle.Item1.StartVFX(Instance._comboVFX, tuffle.Item2.AvatarHandler.GetBodyPart(BodyPartEnum.BottomBody));
                DeckManager.GetCraftingSlots(isPlayer).ResetDeck();
                Instance._cardRecipeDetected = null;
            }

        }

        static void DetectRecipe()
        {
            bool isPlayer = Battle.Turns.TurnHandler.CurrentState == Battle.Turns.TurnState.PlayerTurn;
            //coping the relevant crafting slots from the deck manager
            CardData[] craftingSlots = new CardData[DeckManager.GetCraftingSlots(isPlayer).GetDeck.Length];

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
            var recipes = isPlayer ? Managers.PlayerManager.Instance.GetCombos() : Battle.EnemyManager.Instance.Recipes;

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
    }
}
