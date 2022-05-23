using CardMaga.LoadingScene;
using ReiTools.TokenMachine;
using System;
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
    public static event Action<ISceneHandler> OnSceneHandlerActivated;
    public static event Action<ITokenReciever> OnBeforeSceneShown;
    public static event Action<ITokenReciever> OnBeforeSceneUnloaded;

    private IDisposable _blackPanelToken;

    private LoadingSceneManager _loadingSceneManager;


    public LoadingSceneManager LoadingSceneManager => _loadingSceneManager;


    #region Monobehaviour Callbacks
    public void Awake()
    {
        LoadingSceneManager.OnIjectingLoadingSceneManager += SceneLoaded;
    }
    public void OnDisable()
    {
        LoadingSceneManager.OnIjectingLoadingSceneManager -= SceneLoaded;
    }
    #endregion


    /// <summary>
    /// Called when the scene is loaded
    /// </summary>
    /// <param name="manager"></param>
    public void SceneLoaded(LoadingSceneManager manager)
    {
        // creating a token machine
        TokenMachine sceneTokenMachine = new TokenMachine(RemoveBlackPanel);
        // saving the loading manager
        _loadingSceneManager = manager;

        using (sceneTokenMachine.GetToken())// starting the token machine
        {
            OnSceneHandlerActivated?.Invoke(this);
            OnBeforeSceneShown?.Invoke(sceneTokenMachine); // notifiyng all who need to do preperation 
        }
        // register to when panel finished fading out
        BlackScreenPanel.OnFinishFadeOut += BlackScreenFinished;


        void BlackScreenFinished()
        {
            BlackScreenPanel.OnFinishFadeOut -= BlackScreenFinished;

            OnSceneStart?.Invoke(); // notifiyng that the game should start now
        }
    }




    public void MoveToScene(params int[] scenesIndex)
    {

        BlackScreenPanel.OnFinishFadeIn += SceneTransition;

        // notify about unloading
        CurrentSceneIsUnloaded();

        void SceneTransition()
        {
            BlackScreenPanel.OnFinishFadeIn -= SceneTransition;
            // unload current scenes and start loading the new ones
            _loadingSceneManager.UnloadAndThenLoad(null, scenesIndex);


        }
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

    // Maybe add function to load scenes to this current scene



    #region Private Functions

    private void RemoveBlackPanel()
    {
        _blackPanelToken = BlackScreenPanel.GetToken();
    }

    private void CurrentSceneIsUnloaded()
    {
        TokenMachine t = new TokenMachine(_blackPanelToken.Dispose);

        using (t.GetToken())
        {
            OnBeforeSceneUnloaded?.Invoke(t);
        }
    }
    #endregion

}
