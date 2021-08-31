public class LoadingManager : MonoSingleton<LoadingManager>
{
    

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        NetworkHandler.CheckVersionEvent.Invoke();
    }



}