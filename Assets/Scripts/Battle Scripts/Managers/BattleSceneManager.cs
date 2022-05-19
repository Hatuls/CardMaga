using Battles;
using Battles.UI;
using CardMaga.LoadingScene;
using Managers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;

public class BattleSceneManager : MonoSingleton<BattleSceneManager>
{
    ITokenInitialized[] _singletons;
    //[SerializeField]
    //int _maxFPS = 30;
    private TokenMachine _firstLoadTokenMachine;
    private IDisposable _blackPanelToken;
    public static event Action<ITokenReciever> OnBattleSceneLoaded;
    public static event Action OnStartGame;
    public override void Awake()
    {

        base.Awake();
        LoadingSceneManager.OnScenesEnter += BattleSceneLoaded;
        BlackScreenPanel.OnFinishFadeIn += StartGame;
    }

    private void OnDestroy()
    {
        BlackScreenPanel.OnFinishFadeIn -= StartGame;
        LoadingSceneManager.OnScenesEnter -= BattleSceneLoaded;

    }


    private void BattleSceneLoaded(LoadingSceneManager sceneManager)
    {

        _firstLoadTokenMachine = new TokenMachine(
            () =>{
                _blackPanelToken = BlackScreenPanel.GetToken();
            });

        using(_firstLoadTokenMachine.GetToken())
        OnBattleSceneLoaded.Invoke(_firstLoadTokenMachine);
    }
    private void Update()
    {
        //  Application.targetFrameRate = _maxFPS;
        ThreadsHandler.ThreadHandler.TickThread();
    }
 
  
    public void StartGame()
    {
        OnStartGame?.Invoke();
    }
    public void EndGame()
    {
        _blackPanelToken?.Dispose();
    }

    public override void Init(ITokenReciever token)
    {
      
    }
}
