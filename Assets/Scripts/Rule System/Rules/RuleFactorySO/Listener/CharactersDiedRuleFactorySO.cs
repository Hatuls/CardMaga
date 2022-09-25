using Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersDiedRuleFactory", menuName = "ScriptableObjects/Rule System/Characters Died Rule Factory")]
public class CharactersDiedRuleFactorySO : BaseBoolRuleFactorySO
{
    [SerializeField] private BaseBoolRuleLogicFactorySO[] _logicFactorySo;
    public override BaseRuleLogicFactorySO<bool>[] BaseRuleLogicFactorySo
    {
        get => _logicFactorySo;
    }

    protected override BaseRule<bool> CreateRuleListener(IBattleManager battleManager)
    {
        return new CharactersDiedListener();
    }
}