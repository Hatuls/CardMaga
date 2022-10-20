using CardMaga.Input;

public class ComboAndDeckFilterButton : Button
{
    private InputBehaviour _comboState;
    private InputBehaviour _deckState;

    public InputBehaviour ComboState
    {
        get => _comboState;
    }
    public InputBehaviour DeckState
    {
        get => _deckState;
    }

    protected override void Awake()
    {
        base.Awake();
        _comboState = new InputBehaviour();
        _deckState = new InputBehaviour();
        SetToComboState();
    }
    
    public void SetToComboState()
    {
        TrySetInputBehaviour(_comboState);
    }

    public void SetToDeckState()
    {
        TrySetInputBehaviour(_deckState);
    }
}
