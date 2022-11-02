using Battle.Turns;
using CardMaga.Battle;

// Need this?
public class ForceMoveToLockState : BaseCondition
{

    private IBattleManager _battleManager;
    private TurnHandler _turnHandler;
    private void Start()
    {
        _turnHandler = _battleManager.TurnHandler;
    }
    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn).OnTurnExit += ChangeState;
        _turnHandler.GetTurn(GameTurnType.ExitBattle).OnTurnEnter += ChangeState;
        BattleManager.OnGameEnded += ChangeState;
    }

    private void ChangeState()
    {
        _turnHandler.GetTurn(GameTurnType.LeftPlayerTurn).OnTurnExit -= ChangeState;
        _turnHandler.GetTurn(GameTurnType.ExitBattle).OnTurnEnter -= ChangeState;
        BattleManager.OnGameEnded -= ChangeState;
        _moveCondition = true;
    }
}