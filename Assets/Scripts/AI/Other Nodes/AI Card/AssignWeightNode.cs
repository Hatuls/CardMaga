namespace CardMaga.AI
{
    public class AssignWeightNode : BaseNode<AICard>
    {
        public int Weight { get; set; }
        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            basedEvaluationObject.Weight = Weight;
            NodeState = NodeState.Success;
            return NodeState;
        }
    }

    public class UseCardsValueAsWeightNode : BaseNode<AICard>
    {
        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            basedEvaluationObject.Weight = basedEvaluationObject.Card.CardSO.GetCardValue(basedEvaluationObject.Card.CardLevel);
            NodeState = NodeState.Success;
            return NodeState;
        }
    }

    public class AddWeightToCardsWeightNode : BaseNode<AICard>
    {
        public int Weight { get; set; }
        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            basedEvaluationObject.Weight = basedEvaluationObject.Card.CardSO.GetCardValue(basedEvaluationObject.Card.CardLevel) + Weight;
            NodeState = NodeState.Success;
            return NodeState;
        }
    }
}