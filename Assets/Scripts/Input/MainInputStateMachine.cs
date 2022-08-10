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
        SceneStarter.Register(new OperationTask(Init, 1), SceneStarter.SequenceType.Late);
    }
    private void Init(ITokenReciever tokenMachine)
    {
        using (tokenMachine.GetToken())
            InitStateMachine();
    }
}