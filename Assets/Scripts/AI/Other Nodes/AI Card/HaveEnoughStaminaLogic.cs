using Characters.Stats;
namespace CardMaga.AI
{
    public class HaveEnoughStaminaLogic : BaseNode<AICard>
    {
        public bool IsPlayer { get; set; }

        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            NodeState = StaminaHandler.Instance?.IsEnoughStamina(IsPlayer, basedEvaluationObject.Card) ?? false ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }
}