using Characters.Stats;
using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "Has Stamina Logic", menuName = "ScriptableObjects/AI/Logic/Check Stamina")]
    public class HaveEnoughStaminaLogic : BaseDecisionLogic
    {
        [Sirenix.OdinInspector.InfoBox("Check if card can be player based on the stamina cost")]

        [SerializeField]
        private bool _isForPlayer = false;


        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            currentNode.NodeState = StaminaHandler.Instance?.IsEnoughStamina(_isForPlayer, basedEvaluationObject.Card) ?? false ? NodeState.Success : NodeState.Failure;
            return currentNode.NodeState;
        }
    }
}