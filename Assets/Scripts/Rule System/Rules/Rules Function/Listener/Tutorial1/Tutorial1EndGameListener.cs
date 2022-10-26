﻿using Battle;
using CardMaga.Rules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1EndGameListener : BaseEndGameRule
{
    public override void Dispose()
    {
        BattleTutorial.OnTutorialFinished -= WaitForTutorialToComplete;
    }

    public override void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<bool>[] ruleLogics)
    {
        base.InitRuleListener(battleManager, ruleLogics);
        BattleTutorial.OnTutorialFinished += WaitForTutorialToComplete;
    }

    public Tutorial1EndGameListener(float delayToEndGame) : base (delayToEndGame)
    {
         
    }

    private void WaitForTutorialToComplete()
    {
        Active(true);
    }
}