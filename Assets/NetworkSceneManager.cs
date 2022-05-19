using CardMaga.LoadingScene;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class NetworkSceneManager : MonoBehaviour, ISceneManager
{
    public static event Action<ITokenReciever> OnNetworkSceneLoaded;
    public UnityTokenMachineEvent OnNetworkSceneLoadedEvent;

    private IDisposable _blackPanelToken;
    private  TokenMachine _networkSceneTokenMachine;
    private LoadingSceneManager _loadingSceneManager;

    public  ITokenReciever ScenesTokenMachine => _networkSceneTokenMachine;


    public void OnEnable()
    {
        LoadingSceneManager.OnScenesEnter += OnSceneLoaded;
    }
    public void OnDisable()
    {
        LoadingSceneManager.OnScenesEnter -= OnSceneLoaded;
    }
    public void OnSceneLoaded(LoadingSceneManager manager)
    {
        _networkSceneTokenMachine = new TokenMachine(SceneEntered, null);
        _loadingSceneManager = manager;

        using (_networkSceneTokenMachine.GetToken())
        {
            OnNetworkSceneLoaded?.Invoke(ScenesTokenMachine);
            OnNetworkSceneLoadedEvent?.Invoke(ScenesTokenMachine);
        }
    }

    public void SceneEntered()
    {
        _blackPanelToken = BlackScreenPanel.GetToken();
    }

    public void SceneExit()
    {
        _blackPanelToken.Dispose();
        _loadingSceneManager.UnloadScenes(
          () => _loadingSceneManager.LoadScenes(null, 3)
            );
    }
}

public interface ISceneManager
{

    ITokenReciever ScenesTokenMachine { get; }
    void OnSceneLoaded(LoadingSceneManager manager);
    void SceneEntered();
    void SceneExit();
}

