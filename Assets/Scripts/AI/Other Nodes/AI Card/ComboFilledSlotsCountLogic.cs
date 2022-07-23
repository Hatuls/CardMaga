using Battle.Deck;
using UnityEngine;
namespace CardMaga.AI
{
    public class ComboFilledSlotsCountLogic: BaseNode<AICard>
    {
        [SerializeField]
        public bool IsPlayer { get; set; }

        public int AmountOfFilledSlots { get; set; }
        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            PlayerCraftingSlots comboSlots = DeckManager.GetCraftingSlots(IsPlayer);
            comboSlots.CountCards();
            NodeState = (AmountOfFilledSlots == comboSlots.AmountOfFilledSlots) ? NodeState.Success: NodeState.Failure;
            return NodeState;
        }
    }
}