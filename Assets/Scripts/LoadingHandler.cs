using CardMaga.LoadingScene;
using UnityEngine;

public class LoadingHandler : MonoBehaviour
{
    private LoadingSceneManager _loadingSceneManager;
    [SerializeField]
    private SceneIdentificationSO[] _sceneToLoad;


  
    private void Start()
    {
        LoadingSceneManager.OnInjectingLoadingSceneManager += Inject;
    }
    private void OnDestroy()
    {
        LoadingSceneManager.OnInjectingLoadingSceneManager -= Inject;

    }
    private void Inject(LoadingSceneManager obj)
   => _loadingSceneManager = obj;


    public void LoadScenesAdditive()
 =>
      _loadingSceneManager.LoadScenes(null, GetScenesIndex());

    public void UnloadScenes()
    =>
        _loadingSceneManager.UnloadScenes(null, GetScenesIndex());

    public void UnloadAndThenLoad()
    =>
        _loadingSceneManager.UnloadAndThenLoad(null, GetScenesIndex());

    private int[] GetScenesIndex()
    {
        int[] scenesIndex = new int[_sceneToLoad.Length];
        for (int i = 0; i < scenesIndex.Length; i++)
            scenesIndex[i] = _sceneToLoad[i].SceneBuildIndex;
        return scenesIndex;
    }
}
