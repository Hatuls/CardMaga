using Battle.Deck;
using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "Check Combo Slots Amount", menuName = "ScriptableObjects/AI/Logic/Check Sequences")]
    public class ComboFilledSlotsCountLogic: BaseDecisionLogic
    {
        [SerializeField]
        private bool _isPlayer;
        [SerializeField,Min(0)]
        private int _amountToLookFor;
        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            PlayerCraftingSlots comboSlots = DeckManager.GetCraftingSlots(_isPlayer);
            comboSlots.CountCards();
            currentNode.NodeState = (_amountToLookFor == comboSlots.AmountOfFilledSlots) ? NodeState.Success: NodeState.Failure;
            return currentNode.NodeState;
        }
    }
}