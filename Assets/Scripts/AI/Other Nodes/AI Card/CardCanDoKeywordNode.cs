﻿using Battle;
using CardMaga.Battle;
using Characters.Stats;

namespace CardMaga.AI
{
    public class CardCanDoKeywordNode : BaseNode<AICard>
    {
        public Keywords.KeywordType Keyword { get; set; }
        public override NodeState Evaluate(AICard evaluateObject)
        {
            NodeState = evaluateObject.Card.TryGetKeyword(Keyword, out int amount) ? NodeState.Success : NodeState.Failure;
            return NodeState;
        }
    }

    public class IsGoingToFinishStamina : BaseNode<AICard>
    {
public bool IsPlayer { get; set; }
        public override NodeState Evaluate(AICard evaluateObject)
        {
          
            NodeState = (BattleManager.Instance.PlayersManager.GetCharacter(IsPlayer).StaminaHandler.Stamina == evaluateObject.Card.StaminaCost) ? NodeState.Success: NodeState.Failure;
            return NodeState;
        }
    }
}