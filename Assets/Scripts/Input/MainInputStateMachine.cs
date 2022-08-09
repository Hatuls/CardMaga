using Battle;
using Managers;
using ReiTools.TokenMachine;
using System.Collections;

public class MainInputStateMachine : BaseStateMachine
{

    public void Update()
    {
        if (_currentState == null)
            return;

        TryChangeState(_currentState.OnHoldState());
    }

    private void Awake()
    {
        BattleStarter.Register(new SequenceOperation(Init, 1), BattleStarter.BattleStarterOperationType.Late);
    }
    private void Init(ITokenReciever tokenMachine)
    {
        using (tokenMachine.GetToken())
            InitStateMachine();
    }
}