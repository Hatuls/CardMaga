using Battle;
using Managers;
using ReiTools.TokenMachine;
using System.Collections;

public class MainInputStateMachine : BaseStateMachine , ISequenceOperation
{
    public int Priority => 1;

    public OrderType Order =>  OrderType.After;

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void ExecuteTask(ITokenReciever tokenMachine)
    {
        using (tokenMachine.GetToken())
            InitStateMachine();
    }

    public void Update()
    {
        if (_currentState == null)
            return;

        TryChangeState(_currentState.OnHoldState());
    }

    private void Awake()
    {
        BattleManager.Register(this);
    }
}