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
    public static event Action<IRecieveOnlyTokenMachine> OnBattleSceneLoaded;
    public override void Awake()
    {
        base.Awake();
        LoadingSceneManager.OnSceneLoaded += Init;
       
    }

    private void OnDestroy()
    {
        LoadingSceneManager.OnSceneLoaded -= Init;
    }


    private void Update()
    {
        //  Application.targetFrameRate = _maxFPS;
        ThreadsHandler.ThreadHandler.TickThread();
    }
    public override void Init(IRecieveOnlyTokenMachine token)
    {
        OnBattleSceneLoaded?.Invoke(token);


        //const byte amount = 12;
        //_singletons = new ISingleton[amount]
        //{
        //    VFXManager.Instance,
        //    CardExecutionManager.Instance,
        //    BattleUiManager.Instance,
        //    CardManager.Instance,
        //    CameraController.Instance,
        //    PlayerManager.Instance,
        //    EnemyManager.Instance,
        //    Combo.ComboManager.Instance,
        //    Keywords.KeywordManager.Instance,
        //    Battles.Deck.DeckManager.Instance,
        //    CardUIManager.Instance,
        //    BattleManager.Instance,
        //};

       
    }
  
}
