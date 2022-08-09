using Battle;
using Battle.UI;
using CardMaga.LoadingScene;
using Managers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private LoadingSceneManager _sceneManager;


    private void Awake()
    {
        LoadingSceneManager.OnInjectingLoadingSceneManager += Inject;
    }
    private void OnDestroy()
    {
        LoadingSceneManager.OnInjectingLoadingSceneManager -=Inject;
    }


    private void Inject(LoadingSceneManager loadingSceneManager)
        => _sceneManager = loadingSceneManager;
}
