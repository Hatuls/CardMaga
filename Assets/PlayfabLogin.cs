using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using UnityEngine;


public class PlayfabLogin : MonoBehaviour
{
    IDisposable _token;
    public static event Action<LoginResult> OnLoginSuccessfull;
    [SerializeField]
    private GoogleManager _googleManager;
    public void Init(ITokenReciever tokenMachine)
    {
        _token = tokenMachine.GetToken();

        Debug.Log("Google Manager Start Sign In");
        _googleManager.OnSignInButtonClicked();
    }

    public void TaskCompleted()
    {
        Debug.Log("Finished Log in - Continue To main menu");
        _token.Dispose();
    }

}
