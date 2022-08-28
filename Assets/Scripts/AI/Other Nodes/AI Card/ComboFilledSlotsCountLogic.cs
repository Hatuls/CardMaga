using Battle.Deck;
using UnityEngine;
namespace CardMaga.AI
{
    public class ComboFilledSlotsCountLogic: BaseNode<AICard>
    {
   
 
        public DeckHandler DeckHandler { get; set; }
        public int AmountOfFilledSlots { get; set; }
        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            PlayerCraftingSlots comboSlots = DeckHandler[DeckEnum.CraftingSlots] as PlayerCraftingSlots;
            comboSlots.CountCards();
            NodeState = (AmountOfFilledSlots == comboSlots.AmountOfFilledSlots) ? NodeState.Success: NodeState.Failure;
            return NodeState;
        }
    }
}