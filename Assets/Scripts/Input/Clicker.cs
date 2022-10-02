using CardMaga.Input;

public class Clicker : TouchableItem<Clicker>
{
    protected override void Awake()
    {
        base.Awake();
        ForceChangeState(true);
    }
}
