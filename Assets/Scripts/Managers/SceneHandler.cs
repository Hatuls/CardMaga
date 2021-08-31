using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public static class SceneHandler
{
    public enum ScenesEnum { MainMenu = 0, Loading = 1, Battle = 2, }
    static System.Action onLoaderCallback;
    static AsyncOperation _loadingAsyncOperation;
    public static System.Action<ScenesEnum> onFinishLoadingScene;


    /// <summary>
    /// Scene gameobject that callbacks on finish loading scene
    /// </summary>
    public static SceneLoaderCallback SceneLoaderCallback
    { get; set; }


    private static ScenesEnum _currentScene = ScenesEnum.MainMenu;
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

    
    public static void LoadScene(ScenesEnum sceneEnum)
    {
        onLoaderCallback = () =>
        {
            SceneLoaderCallback.StartCoroutine(LoadSceneAsync(sceneEnum));
        };


        CurrentScene = ScenesEnum.Loading;
        SceneManager.LoadScene((int)ScenesEnum.Loading);

    }

    static IEnumerator LoadSceneAsync(ScenesEnum sceneEnum)
    {
        yield return null;

        _loadingAsyncOperation = SceneManager.LoadSceneAsync((int)sceneEnum);

        while (!_loadingAsyncOperation.isDone) 
        yield return null;
        
        
        CurrentScene = sceneEnum;
        onFinishLoadingScene?.Invoke(sceneEnum);
    }
    /// <summary>
    /// 
    /// </summary>
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
