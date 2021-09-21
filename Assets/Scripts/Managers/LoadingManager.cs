using Unity.Events;

public class LoadingManager : MonoSingleton<LoadingManager>
{
    [UnityEngine.SerializeField] VoidEvent _loadBattleEvent;
    SceneHandler _sceneHandler;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        Init();
    }
    public override void Init()
    {
        _sceneHandler = new SceneHandler(this);
        NetworkHandler.CheckVersionEvent.Invoke();
    }
    public void LoadScene(SceneHandler.ScenesEnum scenesEnum)
    {

        SceneHandler.LoadScene(scenesEnum);
    }
}