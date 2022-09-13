using Battle;
using Managers;
using ReiTools.TokenMachine;

public class MainInputStateMachine : BaseStateMachine, ISequenceOperation<IBattleManager>
{
    public int Priority => 1;


    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager battleManager)
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
        BattleManager.Register(this, OrderType.After);
    }
}