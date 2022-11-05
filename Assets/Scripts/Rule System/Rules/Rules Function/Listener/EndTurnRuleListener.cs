using Battle;
using Battle.Turns;
using CardMaga.Battle;
using CardMaga.Rules;
using UnityEngine;

public class EndTurnRuleListener : BaseEndGameRule
{
    private TurnHandler _turnHandler;
    
    public override void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<bool>[] ruleLogics)
    {
        base.InitRuleListener(battleManager, ruleLogics);
        _turnHandler = battleManager.TurnHandler;
        _turnHandler.OnTurnCountChange += CheckTurnCount;
    }

    private void CheckTurnCount(int count)
    {
        Debug.Log(count);
        
        if (count > 1)
        {
            Active(true);
        }
    }

    public override void Dispose()
    {
        _turnHandler.OnTurnCountChange -= CheckTurnCount;
    }

    public EndTurnRuleListener(float delayToEndGame) : base(delayToEndGame)
    {
    }
}
