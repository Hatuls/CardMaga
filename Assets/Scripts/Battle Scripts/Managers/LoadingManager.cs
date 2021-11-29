using Unity.Events;

public class LoadingManager : MonoSingleton<LoadingManager>
{
    [UnityEngine.SerializeField] VoidEvent _loadBattleEvent;
    SceneHandler _sceneHandler;
    private void Start()
    {
       
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {

            // DontDestroyOnLoad(this.gameObject);
            if (SceneHandler.CurrentScene == SceneHandler.ScenesEnum.NetworkScene)
        Init();
        }
    }
    public override void Init()
    {
        _sceneHandler = new SceneHandler(this);
        NetworkHandler.CheckVersionEvent?.Invoke();
    }
    public void LoadScene(SceneHandler.ScenesEnum scenesEnum)
    {

        SceneHandler.LoadScene(scenesEnum);
    }
}