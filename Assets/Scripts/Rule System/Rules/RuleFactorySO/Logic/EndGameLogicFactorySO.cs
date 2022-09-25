using Battle;
using CardMaga.Rules;
using UnityEngine;

[CreateAssetMenu(fileName = "EndGameRuleLogic", menuName = "ScriptableObjects/Rule System/New End Game Logic Rule")]
public class EndGameLogicFactorySO : BaseBoolRuleLogicFactorySO
{
    public override BaseRuleLogic<bool> CreateRuleLogic(IBattleManager battleManager)
    {
        return new EndGameLogic();
    }
}