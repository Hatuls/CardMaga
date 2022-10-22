using CardMaga.Input;

public class ComboAndDeckButton : ButtonGenaric
{
    private InputBehaviour<ButtonGenaric> _comboState;
    private InputBehaviour<ButtonGenaric> _deckState;

    public InputBehaviour<ButtonGenaric> ComboState
    {
        get => _comboState;
    }

    public InputBehaviour<ButtonGenaric> DeckState
    {
        get => _deckState;
    }

    protected override void Awake()
    {
        base.Awake();
        _comboState = new InputBehaviour<ButtonGenaric>();
        _deckState = new InputBehaviour<ButtonGenaric>();
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
