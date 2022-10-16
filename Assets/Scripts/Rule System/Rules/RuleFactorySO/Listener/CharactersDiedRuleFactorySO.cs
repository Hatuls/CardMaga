using Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersDiedRuleFactory", menuName = "ScriptableObjects/Rule System/Characters Died Rule Factory")]
public class CharactersDiedRuleFactorySO : BaseEndGameRuleFactorySO
{
    protected override BaseEndGameRule CreateRuleListener(IBattleManager battleManager)
    {
        return new CharactersDiedListener(_delayToEndGame);
    }
}