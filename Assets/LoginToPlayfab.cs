using CardMaga.Playfab;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using UnityEngine;


public class LoginToPlayfab : MonoBehaviour
{
    IDisposable _token;
    public static event Action<LoginResult> OnLoginSuccessfull;
    [SerializeField]
    private GoogleManager _googleManager;
    [SerializeField]
    private PlayfabManager _playfabManager;

    private void Awake()
    {
        PlayfabLogin.OnSuccessfullLogin += TaskCompleted;
    }
    private void OnDestroy()
    {
        
        PlayfabLogin.OnSuccessfullLogin -= TaskCompleted;
    }
    public void Init(ITokenReciever tokenMachine)
    {
        _token = tokenMachine.GetToken();

#if !UNITY_EDITOR
        Debug.Log("Google Manager Start Sign In");
        _googleManager.TrySignInWithGoogle();
#elif UNITY_EDITOR
        Debug.Log("Playfab Manager Start Sign In");
        _playfabManager.PlayFabLogin.LoginWithPlayfabCustomID();
#endif

    }

    public void TaskCompleted(LoginResult result)
    {
        Debug.Log("Finished Log in - Continue To main menu");
        _token.Dispose();
    }

}
