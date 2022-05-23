using CardMaga.LoadingScene;
using ReiTools.TokenMachine;
using UnityEngine;
using UnityEngine.Events;

public class SceneHandlerTerminal : MonoBehaviour
{
    [SerializeField,EventsGroup]
    UnityTokenMachineEvent OnBeforeSceneShown, OnBeforeSceneUnloaded;
    [SerializeField,EventsGroup]
    UnityEvent OnSceneStart;
    private void SceneLoaded(ITokenReciever tokenReciever)
        => OnBeforeSceneShown?.Invoke(tokenReciever);
    private void SceneStarts( )
        => OnSceneStart?.Invoke();
    private void SceneUnLoaded(ITokenReciever tokenReciever)
        => OnBeforeSceneUnloaded?.Invoke(tokenReciever);
    private void Awake()
    {
        SceneHandler.OnBeforeSceneUnloaded += SceneUnLoaded;
        SceneHandler.OnBeforeSceneShown += SceneLoaded;
        SceneHandler.OnSceneStart += SceneStarts;
    }
    private void OnDestroy()
    {
        SceneHandler.OnBeforeSceneUnloaded -= SceneUnLoaded;
        SceneHandler.OnBeforeSceneShown -= SceneLoaded;
        SceneHandler.OnSceneStart -= SceneStarts;
    }
}












public interface ISceneManager
{

    ITokenReciever ScenesTokenMachine { get; }
    void OnSceneLoaded(LoadingSceneManager manager);
    void SceneEntered();
    void SceneExit();
}

