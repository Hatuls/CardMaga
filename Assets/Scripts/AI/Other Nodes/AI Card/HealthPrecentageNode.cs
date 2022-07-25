﻿
using Characters.Stats;
namespace CardMaga.AI
{

    public enum OperatorType
    {
        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqualTo,
        BiggerThan,
        BiggerThanOrEqualTo,
    }

    public class HealthPrecentageNode : BaseNode<AICard>
    {
        /// <summary>
        /// Is it the player's health or the enemys
        /// </summary>
        public bool IsPlayer { get; set; }
        /// <summary>
        /// What type of math operation to do
        /// </summary>
        public OperatorType Operator { get; set; }
        /// <summary>
        /// The Precentage of the health
        /// </summary>
        public float Precentage { get; set; }

        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            const int ONE_HUNDREND_PRECENT = 100;
            CharacterStatsHandler statsHandler = CharacterStatsManager.GetCharacterStatsHandler(IsPlayer);

            var maxHealth = (float)statsHandler.GetStats(Keywords.KeywordTypeEnum.MaxHealth).Amount;
            var current = (float)statsHandler.GetStats(Keywords.KeywordTypeEnum.Heal).Amount;

            NodeState = (Operator.Evaluate((current / maxHealth)* ONE_HUNDREND_PRECENT, Precentage)) ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }

   
}
public static class OperatorHelper
{

    public static bool Evaluate(this CardMaga.AI.OperatorType operatorType, float x, float y)
    {
        switch (operatorType)
        {
            case CardMaga.AI.OperatorType.Equal:
                return x == y;
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