using Battle.Combo;
using CardMaga.Battle;
using CardMaga.Battle.Combo;
using CardMaga.Rules;

public class ComboListener : BaseEndGameRule
{
    private ComboManager _comboManager;
    private ComboSO _comboToCheck;

    public ComboListener(ComboSO combo,float delayToEndGame) : base(delayToEndGame)
    {
        _comboToCheck = combo;
    }
    
    public override void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<bool>[] ruleLogics)
    {
        base.InitRuleListener(battleManager, ruleLogics);
        _comboManager = battleManager.ComboManager;
        _comboManager.OnComboSucceeded += CheckCombo;
    }

    private void CheckCombo(BattleComboData battleComboData)
    {
        if (battleComboData.ComboSO == _comboToCheck)
        {
            Active(true);
        }
    }

    public override void Dispose()
    {
        _comboManager.OnComboSucceeded -= CheckCombo;
    }
}
