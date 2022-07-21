
using Characters.Stats;
using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "Health Precentage Logic", menuName = "ScriptableObjects/AI/Logic/Health Precentage")]
    public class HealthPrecentageLogic : BaseDecisionLogic
    {
        [SerializeField]
        private bool _isPlayer;

        [Sirenix.OdinInspector.InfoBox("Calculation is based on precentage")]
        [SerializeField]
        private OperatorType _operator;
        [SerializeField,Range(0f,100f)]
        private float _precentage;

        public override NodeState Evaluate(Node currentNode, AICard basedEvaluationObject)
        {
            CharacterStatsHandler statsHandler = CharacterStatsManager.GetCharacterStatsHandler(_isPlayer);

            var maxHealth = statsHandler.GetStats(Keywords.KeywordTypeEnum.MaxHealth).Amount;
            var current   = statsHandler.GetStats(Keywords.KeywordTypeEnum.Heal).Amount;

            currentNode.NodeState = (_operator.Evaluate(current/maxHealth,_precentage)) ? NodeState.Success : NodeState.Failure;
            return currentNode.NodeState;
        }
    }


    public enum OperatorType
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqualTo,
        BiggerThan,
        BiggerThanOrEqualTo,
    }
}
public static class OperatorHelper
{
  
    public static bool Evaluate(this CardMaga.AI.OperatorType operatorType, float x, float y)
    {
        switch (operatorType)
        {
            case CardMaga.AI.OperatorType.Equal:
                return  x == y;
            case CardMaga.AI.OperatorType.NotEqual:
                return x != y;
            case CardMaga.AI.OperatorType.LessThan:
                return x < y;
            case CardMaga.AI.OperatorType.LessThanOrEqualTo:
                return x <= y;
            case CardMaga.AI.OperatorType.BiggerThan:
                return x > y;
            case CardMaga.AI.OperatorType.BiggerThanOrEqualTo:
                return x >= y;
        }
        return false;
    }
}