using Battle;
using Battle.Combo;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "SurviveEnemyAttackListenerFactorySO", menuName = "ScriptableObjects/Rule System/Survive Enemy Attack Listener FactorySO")]
public class SurviveEnemyAttackListenerFactorySO : BaseEndGameRuleFactorySO
{
    [SerializeField] private BaseBoolRuleLogicFactorySO[] _logicFactorySo;
    [SerializeField] private ComboSO _comboToCheck;

    public override BaseRuleLogicFactorySO<bool>[] BaseRuleLogicFactorySo
    {
        get => _logicFactorySo;
    }
    protected override BaseRule<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new SurviveEnemyAttackListener(_comboToCheck,DelayToEndGame);
    }
}
