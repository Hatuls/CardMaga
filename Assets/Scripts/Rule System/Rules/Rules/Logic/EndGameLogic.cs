using System;
using Battle;
using CardMaga.Rules;
using UnityEngine;

public class EndGameLogic : BaseRuleLogic<bool>
{
    public override void InitRuleLogic(IBattleManager battleManager)
    {
    }

    public override bool CheckCondition()
    {
        throw new NotImplementedException();
    }

    public override void UpDateRule()
    {
        throw new NotImplementedException();
    }

    public override void DeActiveRule(bool obj)
    {
        throw new NotImplementedException();
    }

    public override void ActiveRule(bool obj)
    {
        Debug.LogError("LeftPlayer died" + "is left player: " + obj);
    }

    public override void Dispose()
    {
        throw new NotImplementedException();
    }
}