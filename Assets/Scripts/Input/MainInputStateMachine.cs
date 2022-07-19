public class MainInputStateMachine : BaseStateMachine
{
    private void Awake()
    {
        SceneHandler.OnSceneLateStart += InitStateMachine;
    }

    public void Update()
    {
        if (_currentState == null)
            return;

        TryChangeState(_currentState.OnHoldState());
    }

    private void OnDestroy()
    {
        SceneHandler.OnSceneLateStart -= InitStateMachine;
    }
}