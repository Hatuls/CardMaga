using UnityEngine;

namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "Invert Logic", menuName = "ScriptableObjects/AI/Logic/Invert", order = -99999)]
    public class InvertLogic : BaseDecisionLogic
    {
        /// <summary>
        /// Work as an INVERT Logic
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="basedEvaluationObject"></param>
        /// <returns></returns>
        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            NodeState result = NodeState.Failure;
            var children = currentNode.Children;
            for (int i = 0; i < children.Length; i++)
            {
                 result = children[i].Evaluate(basedEvaluationObject);
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