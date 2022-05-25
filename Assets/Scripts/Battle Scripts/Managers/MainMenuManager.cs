using Battles;
using Battles.UI;
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
        LoadingSceneManager.OnIjectingLoadingSceneManager += Inject;
    }
    private void OnDestroy()
    {
        LoadingSceneManager.OnIjectingLoadingSceneManager -=Inject;
    }


    private void Inject(LoadingSceneManager loadingSceneManager)
        => _sceneManager = loadingSceneManager;
}
