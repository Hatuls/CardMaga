using UnityEngine;
namespace CardMaga.AI
{

    /// <summary>
    /// OR Logic
    /// other name is Selector
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [CreateAssetMenu(fileName = "OR Logic",menuName = "ScriptableObjects/AI/Logic/OR", order = -99999)] 
    
    public class ORLogic : BaseDecisionLogic
    {


        /// <summary>
        /// Work as an OR Logic
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="basedEvaluationObject"></param>
        /// <returns></returns>
       // [Header("Works as an OR logic statement\nWill return success only if one of the children will return success")]
        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            Node[] children = currentNode.Children;
            for (int i = 0; i < children.Length; i++)
            {
                NodeState result = children[i].Evaluate(basedEvaluationObject);
                switch (result)
                {
                    case NodeState.Success:
                    case NodeState.Running:
                        currentNode.NodeState = result;
                        return currentNode.NodeState;

                    case NodeState.Failure:
                    default:
                        continue;
                }

            }
            currentNode.NodeState = NodeState.Failure;
            return currentNode.NodeState;
        }


    }
    
}