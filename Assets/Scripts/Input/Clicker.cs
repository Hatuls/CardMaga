using CardMaga.Input;

public class Clicker : TouchableItem<Clicker>
{
    private void Start()
    {
        UnLock();
    }

    public override InputIdentificationSO InputIdentification { get; }
}
