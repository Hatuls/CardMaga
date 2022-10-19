using Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboRuleListenerFactorySO", menuName = "ScriptableObjects/Rule System/Tutorial1EndGameFactory")]
public class Tutorial1EndGameFactory : BaseEndGameRuleFactorySO
{
    protected override BaseEndGameRule CreateRuleListener(IBattleManager battleManager)
    {
        return new Tutorial1EndGameListener(_delayToEndGame);
    }
}
