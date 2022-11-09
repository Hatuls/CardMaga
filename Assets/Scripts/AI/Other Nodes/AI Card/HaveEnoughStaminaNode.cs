using CardMaga.Battle;
namespace CardMaga.AI
{
    public class HaveEnoughStaminaNode : BaseNode<AICard>
    {
        public bool IsPlayer { get; set; }

        public override NodeState Evaluate(AICard basedEvaluationObject)
        {
            var character = BattleManager.Instance.PlayersManager.GetCharacter(IsPlayer);
            NodeState = character.StaminaHandler.CanPlayCard(basedEvaluationObject.BattleCard) ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }
}