public interface IStateMachine
{
    BaseState CurrentState { get; }

    void TryChangeState(StateIdentificationSO stateID);

    void ForceChangeState(StateIdentificationSO stateID);
}