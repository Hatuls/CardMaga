
using CardMaga.Battle.UI;
using CardMaga.SequenceOperation;
using ReiTools.TokenMachine;

public class MainInputStateMachine : BaseStateMachine, ISequenceOperation<IBattleUIManager>
{
    public int Priority => 1;


    public void Dispose()
    {
        throw new System.NotImplementedException();
    }

    public void ExecuteTask(ITokenReceiver tokenMachine, IBattleUIManager battleManager)
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

  
}