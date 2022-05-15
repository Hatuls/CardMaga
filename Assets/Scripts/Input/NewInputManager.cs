
public class NewInputManager : BaseStateMachine
{
    public void Update()
    {
        if (_currentState == null)
        {
            return;
        }
        
        TryChangeState(_currentState.OnHoldState());
    }
}
