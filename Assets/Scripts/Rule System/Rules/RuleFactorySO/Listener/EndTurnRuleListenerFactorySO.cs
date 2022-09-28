using Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "EndTurnRuleListenerFactorySO", menuName = "ScriptableObjects/Rule System/End Turn Rule Listener FactorySO")]
public class EndTurnRuleListenerFactorySO : BaseEndGameRuleFactorySO
{
    [SerializeField] private BaseBoolRuleLogicFactorySO[] _logicFactorySo;
    public override BaseRuleLogicFactorySO<bool>[] BaseRuleLogicFactorySo
    {
        get => _logicFactorySo;
    }
    protected override BaseRule<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new EndTurnRuleListener(DelayToEndGame);
    }
}
