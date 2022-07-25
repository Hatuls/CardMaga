using CardMaga.LoadingScene;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;

public enum ScenesEnum { NetworkScene = 0, LoadingScene = 1, MainMenuScene = 2, MapScene = 3, GameBattleScene = 4, LoreScene = 5 }

public interface ISceneHandler
{
    LoadingSceneManager LoadingSceneManager { get; }
    /// <summary>
    /// Will Unload the current active scenes and then load the new one
    ///   /// Note: Always better to use the SceneIdentificationSO
    /// </summary>
    /// <param name="scenesIndex"></param>
    void MoveToScene(int sceneIndex);
    /// <summary>
    /// Will Unload the current active scenes and then load the new one
    /// </summary>
    /// <param name="sceneIdentificationSO"></param>
    void MoveToScene(SceneIdentificationSO sceneIdentificationSO);
    /// <summary>
    /// Will Unload the current active scenes and then load the new ones
    /// Note: Always better to use the SceneIdentificationSO
    /// </summary>
    /// <param name="scenesIndex"></param>
    void MoveToScene(params int[] scenesIndex);

    /// <summary>
    /// Will Unload the current active scenes and then load the new ones
    /// </summary>
    /// <param name="scenes"></param>
    void MoveToScene(params SceneIdentificationSO[] scenes);
}

public class SceneHandler : MonoBehaviour, ISceneHandler
{

    public static event Action OnSceneStart;
    public static event Action OnSceneLateStart;
    public static event Action<ISceneHandler> OnSceneHandlerActivated;
    public static event Action<ITokenReciever> OnBeforeSceneShown;
    public static event Action<ITokenReciever> OnLateBeforeSceneShown;
    public static event Action<ITokenReciever> OnBeforeSceneUnloaded;
    


    private IDisposable _blackPanelToken;
    private LoadingSceneManager _loadingSceneManager;

    [SerializeField]
    private bool _overrideLoadingSceneManager;

   
    public LoadingSceneManager LoadingSceneManager => _loadingSceneManager;


    #region Monobehaviour Callbacks
    public void Awake()
    {
        LoadingSceneManager.OnInjectingLoadingSceneManager += SceneLoaded;
    }
    public void OnDisable()
    {
        LoadingSceneManager.OnInjectingLoadingSceneManager -= SceneLoaded;
    }

    public IEnumerator Start()
    {
#if PRODUCTION_BUILD
_overrideLoadingSceneManager = false;
#endif
        yield return null;
        if (_overrideLoadingSceneManager)
            StartScene();
    }
#endregion


    /// <summary>
    /// Called when the scene is loaded
    /// </summary>
    /// <param name="manager"></param>
    public void SceneLoaded(LoadingSceneManager manager)
    {
        // saving the loading manager
        _loadingSceneManager = manager;
        StartScene();
    }
    private void StartScene()
    {
        TokenMachine sceneTokenMachine = new TokenMachine(BeforeLateSceneStart);
        using (sceneTokenMachine.GetToken())// starting the token machine
        {
            OnSceneHandlerActivated?.Invoke(this);
            OnBeforeSceneShown?.Invoke(sceneTokenMachine); // notifiyng all who need to do preperation 
        }
    }

    private void BeforeLateSceneStart()
    {
        TokenMachine sceneTokenMachine = new TokenMachine(OnSceneStart);
        using (sceneTokenMachine.GetToken())
            OnLateBeforeSceneShown?.Invoke(sceneTokenMachine);
    }

    public void MoveToScene(params int[] scenesIndex)
    {

        //   BlackScreenPanel.OnFinishFadeIn += SceneTransition;

        //// notify about unloading
        //CurrentSceneIsUnloaded();

        TokenMachine t = new TokenMachine(()=> _loadingSceneManager.UnloadAndThenLoad(null, scenesIndex));

           using (t.GetToken())
           {
               OnBeforeSceneUnloaded?.Invoke(t);
           }

        // unload current scenes and start loading the new ones
     

    }

    public void MoveToScene(int sceneIndex)
   => MoveToScene(new int[1] { sceneIndex });

    public void MoveToScene(params SceneIdentificationSO[] scenes)
    {
        int[] scenesIndex = new int[scenes.Length];

        for (int i = 0; i < scenes.Length; i++)
            scenesIndex[i] = scenes[i].SceneBuildIndex;

        MoveToScene(scenesIndex);
    }

    public void MoveToScene(SceneIdentificationSO sceneIdentificationSO)
        => MoveToScene(sceneIdentificationSO.SceneBuildIndex);

 



}
