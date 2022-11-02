namespace CardMaga.AI
{
    public class InvertNode<T> : BaseNode<T>
    {
        /// <summary>
        /// Work as an INVERT Logic
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="basedEvaluationObject"></param>
        /// <returns></returns>
        public override NodeState Evaluate(T basedEvaluationObject)
        {
            NodeState result = NodeState.Failure;
            for (int i = 0; i < Children.Length; i++)
            {
                result = Children[i].Evaluate(basedEvaluationObject);
                switch (result)
                {
                    case NodeState.Success:
                        result = NodeState.Failure;
                        break;

                    case NodeState.Failure:
                        result = NodeState.Success;
                        break;
                    case NodeState.Running:
                    default:
                        break;
                }

            }

            return result;
        }
    }
}