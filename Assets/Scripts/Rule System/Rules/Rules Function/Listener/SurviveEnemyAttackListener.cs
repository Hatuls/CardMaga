using Battle.Combo;
using Battle.Turns;
using CardMaga.Battle;
using CardMaga.Battle.Combo;
using CardMaga.Rules;

public class SurviveEnemyAttackListener : BaseEndGameRule
{
    private ComboManager _comboManager;
    private TurnHandler _turnHandler;
    private ComboSO _comboToCheck;

    private bool _didTheRightCombo = false;

    public SurviveEnemyAttackListener(ComboSO combo,float delayToEndGame) : base(delayToEndGame)
    {
        _comboToCheck = combo;
    }
    
    public override void InitRuleListener(IBattleManager battleManager, BaseRuleLogic<bool>[] ruleLogics)
    {
        base.InitRuleListener(battleManager, ruleLogics);
        _comboManager = battleManager.ComboManager;
        _turnHandler = battleManager.TurnHandler;

        bool isLeftCharacter = false;
        
        _comboManager.OnComboSucceeded += CheckCombo;
        _turnHandler.GetCharacterTurn(isLeftCharacter).OnTurnExit += CheckIfEnemyEndedTurn;

    }

    private void CheckCombo(BattleComboData battleComboData)
    {
        if (battleComboData.ComboSO == _comboToCheck)
        {
            _didTheRightCombo = true;
        }
    }

    private void CheckIfEnemyEndedTurn()
    {
        if (_didTheRightCombo)
        {
            Active(true);
        }
    }

    public override void Dispose()
    {
        _comboManager.OnComboSucceeded -= CheckCombo;
    }
}
