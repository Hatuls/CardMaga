namespace CardMaga.AI
{
    public class ComboFilledSlotsCountLogic : BaseNode<AICard>
    {
        private CraftingHandler _craftingHandler;
        private int _amountOfFilledSlots;

        public CraftingHandler CraftingHandler { get => _craftingHandler; set => _craftingHandler = value; }
        public int AmountOfFilledSlots { get => _amountOfFilledSlots; set => _amountOfFilledSlots = value; }
        public override NodeState Evaluate(AICard basedEvaluationObject)
        {

            NodeState = (AmountOfFilledSlots == CraftingHandler.CountFullSlots) ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }
}