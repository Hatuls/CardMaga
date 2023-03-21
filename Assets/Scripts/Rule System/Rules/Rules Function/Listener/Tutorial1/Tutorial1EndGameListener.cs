using CardMaga.Battle;
using CardMaga.Rules;

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

    public Tutorial1EndGameListener(float delayToEndGame) : base(delayToEndGame)
    {

    }

    private void WaitForTutorialToComplete()
    {
        Active(true);
    }
}
