using UnityEngine;

namespace CardMaga.AI
{
    /// <summary>
    /// AND Logic
    /// other name is Sequence
    /// </summary>
    /// <typeparam name="T"></typeparam>

    [CreateAssetMenu(fileName = "AND Logic", menuName = "ScriptableObjects/AI/Logic/AND", order = -99999)]
    public class AndLogic : BaseDecisionLogic
    {

        //[Sirenix.OdinInspector.InfoBox("Works as an AND logic statement\nWill return success only if all of the children will return success")]



        /// <summary>
        /// Work as an AND Logic
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="basedEvaluationObject"></param>
        /// <returns></returns>
        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            bool anyChildIsRunning = false;
            var children = currentNode.Children;
            for (int i = 0; i < children.Length; i++)
            {
                switch (children[i].Evaluate(basedEvaluationObject))
                {
                    case NodeState.Success:
                        continue;
                    case NodeState.Failure:
                        currentNode.NodeState = NodeState.Failure;
                        return currentNode.NodeState;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        break;
                    default:
                        currentNode.NodeState = NodeState.Success;
                        return NodeState.Success;
                }

            }
            currentNode.NodeState = (anyChildIsRunning) ? NodeState.Running : NodeState.Success;
            return currentNode.NodeState;
        }
    }
}