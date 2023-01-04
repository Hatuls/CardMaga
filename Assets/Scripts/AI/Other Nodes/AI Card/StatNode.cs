
using Battle;
using CardMaga.Battle;
using Characters.Stats;
namespace CardMaga.AI
{

    public class StatNode : BaseNode<AICard>
    {
        /// <summary>
        ///  Is it the player's Stat
        /// </summary>
        public bool IsPlayer { get; set; }
        /// <summary>
        /// The keyword you want to check
        /// </summary>
        public Keywords.KeywordType KeywordType { get; set; }
        /// <summary>
        /// The Math operator
        /// </summary>
        public OperatorType Operator { get; set; }
        /// <summary>
        /// The amount to compare the keyword to
        /// </summary>
        public float Amount { get; set; }

        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            CharacterStatsHandler statsHandler = BattleManager.Instance.PlayersManager.GetCharacter(IsPlayer).StatsHandler; 
            int stat = statsHandler.GetStat(KeywordType).Amount;
            NodeState = (Operator.Evaluate(stat, Amount)) ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }

    public class CheckCardTypeNode : BaseNode<AICard>
    {
        /// <summary>
        /// The BattleCard Type To Compare To
        /// </summary>
        public Card.CardTypeEnum CardType { get; set; }

        public override NodeState Evaluate(AICard evaluateObject)
        {
            NodeState = (CardType == evaluateObject.BattleCard.CardSO.CardTypeData.CardType)? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }

    public class CompareBetweenKeywordsNode : BaseNode<AICard>
    {
        public bool IsPlayer { get; set; }
        public Keywords.KeywordType KeywordA { get; set; }
        public Keywords.KeywordType KeywordB { get; set; }
        public OperatorType Operator { get; set; }
        public override NodeState Evaluate(AICard evaluateObject)
        {
            CharacterStatsHandler statsHandler = BattleManager.Instance.PlayersManager.GetCharacter(IsPlayer).StatsHandler;

            int amountA = statsHandler.GetStat(KeywordA).Amount;
            int amountB = statsHandler.GetStat(KeywordB).Amount;

            NodeState = (Operator.Evaluate(amountA,amountB)) ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }
}
