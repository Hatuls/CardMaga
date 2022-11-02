using UnityEngine;

namespace CardMaga.AI
{
    /// <summary>
    /// AND Logic
    /// other name is Sequence
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class AndNode<T> : BaseNode<T>
    {
        /// <summary>
        /// Work as an AND Logic
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="basedEvaluationObject"></param>
        /// <returns></returns>
        public override NodeState Evaluate(T basedEvaluationObject)
        {
            bool anyChildIsRunning = false;
            for (int i = 0; i < Children.Length; i++)
            {
                switch (Children[i].Evaluate(basedEvaluationObject))
                {
                    case NodeState.Success:
                        continue;
                    case NodeState.Failure:
                        NodeState = NodeState.Failure;
                        return NodeState;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        break;
                    default:
                        NodeState = NodeState.Success;
                        return NodeState.Success;
                }

            }
            NodeState = (anyChildIsRunning) ? NodeState.Running : NodeState.Success;
            return NodeState;
        }
    }
}