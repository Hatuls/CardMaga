using CardMaga.Playfab;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using TMPro;
using UnityEngine;


public class LoginToPlayfab : MonoBehaviour
{
    IDisposable _token;
    public static event Action<LoginResult> OnLoginSuccessfull;
    [SerializeField]
    private GoogleManager _googleManager;
    [SerializeField]
    private PlayfabManager _playfabManager;


    [SerializeField]
    TextMeshProUGUI _errorText;

    private void Awake()
    {
       // _errorText.gameObject.SetActive(false);
        PlayfabLogin.OnSuccessfullLogin += TaskCompleted;
        PlayfabLogin.OnFailedLogin += TaskFailed;
    }

    private void TaskFailed(PlayFabError obj)
    {
        _errorText.gameObject.SetActive(true);
        _errorText.text = obj.ErrorMessage +"\n<a>Generated Error:</a> " +obj.GenerateErrorReport();
    }

    private void OnDestroy()
    {

        PlayfabLogin.OnFailedLogin -= TaskFailed;
        PlayfabLogin.OnSuccessfullLogin -= TaskCompleted;
    }
    public void Init(ITokenReciever tokenMachine)
    {
        _token = tokenMachine.GetToken();
        Debug.Log("Google Manager Start Sign In");
        //_googleManager.TrySignInWithGoogle();
//#if !UNITY_EDITOR
//                Debug.Log("Google Manager Start Sign In");
//                _googleManager.TrySignInWithGoogle();
//#elif UNITY_EDITOR
//        Debug.Log("Playfab Manager Start Sign In");
        _playfabManager.PlayFabLogin.LoginWithPlayfabCustomID();
//#endif

    }

    public void TaskCompleted(LoginResult result)
    {
        Debug.Log("Finished Log in!");
        _token.Dispose();
    }

}
