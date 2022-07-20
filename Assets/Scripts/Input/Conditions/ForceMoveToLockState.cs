using Battle;
using Battle.Turns;
using CardMaga.UI;

public class ForceMoveToLockState : BaseCondition
{
    public override bool CheckCondition()
    {
        return _moveCondition;
    }

    public override void InitCondition()
    {
        EndPlayerTurn.OnPlayerEndTurn += ChangeState;
        BattleManager.OnGameEnded += ChangeState;
    }

    private void ChangeState()
    {
        EndPlayerTurn.OnPlayerEndTurn -= ChangeState;
        BattleManager.OnGameEnded -= ChangeState;
        _moveCondition = true;
    }
}