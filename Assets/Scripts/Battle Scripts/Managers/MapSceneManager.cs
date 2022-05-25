using CardMaga.LoadingScene;
using UnityEngine;

public class MapSceneManager : MonoBehaviour
{
    LoadingSceneManager _loadingManager;
    private void Awake()
    {
        LoadingSceneManager.OnIjectingLoadingSceneManager += Inject;
    }

    private void Inject(LoadingSceneManager obj)
        => _loadingManager = obj;

    private void OnDestroy()
    {
        LoadingSceneManager.OnIjectingLoadingSceneManager -= Inject;

    }


}