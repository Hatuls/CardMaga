public class MainInputStateMachine : BaseStateMachine
{
    private void Awake()
    {
        SceneHandler.OnSceneStart += InitStateMachine;
    }

    public void Update()
    {
        if (_currentState == null)
            return;

        TryChangeState(_currentState.OnHoldState());
    }

    private void OnDestroy()
    {
        SceneHandler.OnSceneStart -= InitStateMachine;
    }
}