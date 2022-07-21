using Battle.Deck;
using CardMaga.Card;
using Cards;
using Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "Can Complete Combo", menuName = "ScriptableObjects/AI/Logic/Combo Craft")]
    public class CanCraftComboLogic : BaseDecisionLogic
    {
        [SerializeField]
        private bool _isPlayer;
        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            var deck = DeckManager.GetCraftingSlots(_isPlayer);

            CardData[] craftingSlots = new CardData[deck.GetDeck.Length + 1];

            System.Array.Copy(deck.GetDeck, craftingSlots, deck.GetDeck.Length);
            craftingSlots[craftingSlots.Length - 1] = basedEvaluationObject.Card;

            // checking how many of them are not null
            List<CardTypeData> craftingItems;

            if (GetCraftingSlotsFilled() > 1)
                CheckRecipe();

            return currentNode.NodeState;


            int GetCraftingSlotsFilled()
            {
                int amountCache = 0;
                for (int i = 0; i < craftingSlots.Length; i++)
                {
                    if (craftingSlots[i] != null)
                        amountCache++;
                }

                craftingItems = new List<CardTypeData>(amountCache);
                for (int i = 0; i < craftingSlots.Length; i++)
                {
                    if (craftingSlots[i] != null)
                        craftingItems.Add(craftingSlots[i].CardSO.CardType);

                }
                return amountCache;
            }
            void CheckRecipe()
            {
                // need to make algorithem better!!! 
                var recipes = _isPlayer ? PlayerManager.Instance.GetCombos() : Battle.EnemyManager.Instance.Recipes;

                var comparer = new CardTypeComparer();
                CardTypeData[] cardTypeDatas;
                for (int i = 0; i < recipes.Length; i++)
                {
                    var comboSO = recipes[i].ComboSO;
                    cardTypeDatas = new CardTypeData[comboSO.ComboSequence.Length];

                    for (int j = 0; j < comboSO.ComboSequence.Length; j++)
                        cardTypeDatas[j] = comboSO.ComboSequence[j];


                    if (craftingItems.SequenceEqual(cardTypeDatas, comparer))
                    {
                        currentNode.NodeState = NodeState.Success;
                        return;
                    }

                }
                currentNode.NodeState = NodeState.Failure;


            }
        }
    }
}