
using Characters.Stats;
using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "New Stat Logic", menuName = "ScriptableObjects/AI/Logic/Stat")]
    public class StatLogic : BaseDecisionLogic
    {
        [SerializeField]
        private bool _isPlayer;

        [Sirenix.OdinInspector.InfoBox("Calculation is based on precentage")]

        [SerializeField]
        Keywords.KeywordTypeEnum _keyword;

        [SerializeField]
        private OperatorType _operator;
        [SerializeField, Range(0f, 100f)]
        private float _amount;

        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            CharacterStatsHandler statsHandler = CharacterStatsManager.GetCharacterStatsHandler(_isPlayer);
            var stat = statsHandler.GetStats(_keyword).Amount;

            currentNode.NodeState = (_operator.Evaluate(stat, _amount)) ? NodeState.Success : NodeState.Failure;
            return currentNode.NodeState;
        }
    }
}
