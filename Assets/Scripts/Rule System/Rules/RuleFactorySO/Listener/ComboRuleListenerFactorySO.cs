using Battle.Combo;
using CardMaga.Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboRuleListenerFactorySO", menuName = "ScriptableObjects/Rule System/Combo Rule Listener FactorySO")]
public class ComboRuleListenerFactorySO : BaseEndGameRuleFactorySO
{
    [SerializeField] private ComboSO _comboToCheck;

    protected override BaseEndGameRule CreateRuleListener(IBattleManager battleManager)
    {
        return new ComboListener(_comboToCheck, DelayToEndGame);
    }
}
