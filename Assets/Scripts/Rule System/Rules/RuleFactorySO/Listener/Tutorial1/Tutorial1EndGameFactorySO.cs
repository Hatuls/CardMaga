using Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial1EndGameFactorySO", menuName = "ScriptableObjects/Rule System/Tutorial1EndGameFactorySO")]
public class Tutorial1EndGameFactorySO : BaseEndGameRuleFactorySO
{
    protected override BaseEndGameRule CreateRuleListener(IBattleManager battleManager)
    {
        return new Tutorial1EndGameListener(_delayToEndGame);
    }
}
