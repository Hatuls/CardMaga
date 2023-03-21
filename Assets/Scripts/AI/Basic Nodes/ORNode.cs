namespace CardMaga.AI
{

    /// <summary>
    /// OR Logic
    /// other name is Selector
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class ORNode<T> : BaseNode<T>
    {
        /// <summary>
        /// Work as an OR Logic
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="basedEvaluationObject"></param>
        /// <returns></returns>
       // [Header("Works as an OR logic statement\nWill return success only if one of the children will return success")]
        public override NodeState Evaluate(T basedEvaluationObject)
        {

            for (int i = 0; i < Children.Length; i++)
            {
                NodeState result = Children[i].Evaluate(basedEvaluationObject);
                switch (result)
                {
                    case NodeState.Success:
                    case NodeState.Running:
                        NodeState = result;
                        return NodeState;

                    case NodeState.Failure:
                    default:
                        continue;
                }

            }
            NodeState = NodeState.Failure;
            return NodeState;
        }
    }
}