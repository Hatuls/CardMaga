using Battle;
using Battle.Combo;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboRuleListenerFactorySO", menuName = "ScriptableObjects/Rule System/Combo Rule Listener FactorySO")]
public class ComboRuleListenerFactorySO : BaseEndGameRuleFactorySO
{
    [SerializeField] private BaseBoolRuleLogicFactorySO[] _logicFactorySo;
    [SerializeField] private ComboSO _comboToCheck;
    public override BaseRuleLogicFactorySO<bool>[] BaseRuleLogicFactorySo
    {
        get => _logicFactorySo;
    }
    protected override BaseRule<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new ComboListener(_comboToCheck,DelayToEndGame);
    }
}
