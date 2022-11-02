using Battle.Combo;
using CardMaga.Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "SurviveEnemyAttackListenerFactorySO", menuName = "ScriptableObjects/Rule System/Survive RightPlayer Attack Listener FactorySO")]
public class SurviveEnemyAttackListenerFactorySO : BaseEndGameRuleFactorySO
{
    [SerializeField] private ComboSO _comboToCheck;

    protected override BaseEndGameRule CreateRuleListener(IBattleManager battleManager)
    {
        return new SurviveEnemyAttackListener(_comboToCheck, DelayToEndGame);
    }
}
