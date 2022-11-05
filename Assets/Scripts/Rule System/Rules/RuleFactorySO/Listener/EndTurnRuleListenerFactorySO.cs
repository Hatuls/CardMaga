using CardMaga.Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "EndTurnRuleListenerFactorySO", menuName = "ScriptableObjects/Rule System/End Turn Rule Listener FactorySO")]
public class EndTurnRuleListenerFactorySO : BaseEndGameRuleFactorySO
{
    protected override BaseEndGameRule CreateRuleListener(IBattleManager battleManager)
    {
        return new EndTurnRuleListener(DelayToEndGame);
    }
}
