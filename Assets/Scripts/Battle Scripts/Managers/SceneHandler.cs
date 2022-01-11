using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler
{
    private static SceneHandler _instance;
    public static SceneHandler Instance => _instance;

    public static bool LoadingComplete;
    public enum ScenesEnum { NetworkScene = 0, LoadingScene = 1, MainMenuScene = 2, MapScene = 3, GameBattleScene = 4, LoreScene = 5 }
    static System.Action onLoaderCallback;
    static AsyncOperation _loadingAsyncOperation;
    public static System.Action<ScenesEnum> onFinishLoadingScene;
    public static System.Action<ScenesEnum> onStartLoadingScene;
    public static System.Action OnSceneChange;
    public SceneHandler(LoadingManager sceneLoaderCallback)
    {
        SceneLoaderCallback = sceneLoaderCallback;
        _instance = this;
    }

    /// <summary>
    /// Scene gameobject that callbacks on finish loading scene
    /// </summary>
    public static LoadingManager SceneLoaderCallback
    { get; set; }

    private static ScenesEnum _previousScene;
    private static ScenesEnum _currentScene = ScenesEnum.NetworkScene;
    public static ScenesEnum CurrentScene
    {
        get => _currentScene;
        private set
        {
            if (_currentScene != value)
            {
                _currentScene = value;
            }
        }
    }

    public static void LoadSceneWithNoLoadingScreen(ScenesEnum scene)
    {
        CurrentScene = scene;
        onStartLoadingScene?.Invoke(CurrentScene);
        SceneManager.LoadScene((int)scene, LoadSceneMode.Single);
    }
    public static void LoadScene(ScenesEnum sceneEnum)
    {


        LoadingComplete = false;
        _previousScene = CurrentScene;
        CurrentScene = ScenesEnum.LoadingScene;
        onStartLoadingScene?.Invoke(sceneEnum);
        SceneManager.LoadScene((int)ScenesEnum.LoadingScene, LoadSceneMode.Additive);


        onLoaderCallback = () =>
        {
            SceneLoaderCallback.StartCoroutine(LoadSceneAsync(sceneEnum));
        };
    }

    static IEnumerator LoadSceneAsync(ScenesEnum sceneEnum)
    {
        yield return null;
        _loadingAsyncOperation = SceneManager.LoadSceneAsync((int)sceneEnum, LoadSceneMode.Additive);
        Debug.Log("Loading Async the scene " + sceneEnum.ToString());
        while (!_loadingAsyncOperation.isDone)
            yield return null;

        Debug.Log("Finished Loading Asyncly the scene " + sceneEnum.ToString());
        //    Debug.Log(SceneManager.sceneCount);
        //    SceneManager.SetActiveScene(SceneManager.GetSceneAt((int)sceneEnum));
        Debug.Log("Set Active Scene " + sceneEnum.ToString());
        yield return null;
        LoadingComplete = true;
        CurrentScene = sceneEnum;
        onFinishLoadingScene?.Invoke(sceneEnum);
        OnSceneChange?.Invoke();
    }
    public static async void UnloadPreviousScene(CancellationToken token, System.Action OnUnloadComplete = null)
    {
        await UnloadScene(_previousScene, token, async () => await UnloadScene(ScenesEnum.LoadingScene, token, OnUnloadComplete));
    }
    public async static Task UnloadScene(ScenesEnum scene, CancellationToken token, System.Action OnUnloadComplete = null)
    {
        await UnloadScene((int)scene, token, OnUnloadComplete);
    }
    public static async Task UnloadScene(int sceneIndex, CancellationToken token, System.Action OnUnloadComplete = null)
    {
        var operation = SceneManager.UnloadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            await Task.Yield();
            if (token.IsCancellationRequested)
                break;
        }

        OnUnloadComplete?.Invoke();
    }
    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback.Invoke();
            onLoaderCallback = null;
        }
    }

    /// <summary>
    /// Return the progress of loading the scene
    /// 1 is done 
    /// 0 is started
    /// </summary>
    /// <returns>The Progress Of Loading The Next Scene</returns>
    public static float LoadingProgress()
   => _loadingAsyncOperation != null ? _loadingAsyncOperation.progress : 1f;

}
