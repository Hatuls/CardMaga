using CardMaga.Input;

public class Clicker : TouchableItem<Clicker>
{
    private void Awake()
    {
        ForceChangeState(true);
    }
}
