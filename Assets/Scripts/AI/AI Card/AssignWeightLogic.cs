using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "Assign Weight", menuName = "ScriptableObject" +
        "s/AI/Logic/Assign Weight")]
    public class AssignWeightLogic : BaseDecisionLogic
    {
        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            currentNode.AssignWeight(basedEvaluationObject);
            return NodeState.Success;
        }
    }
}