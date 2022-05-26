using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabManager : MonoBehaviour
{
    private string _playfabID;
    public string PlayfabID { get => _playfabID;private set => _playfabID = value; }
    private void Awake()
    {
        PlayfabLogin.OnLoginSuccessfull += SuccessFullLogin;
    }
    private void OnDestroy()
    {
        PlayfabLogin.OnLoginSuccessfull -= SuccessFullLogin;
        
    }
    private void SuccessFullLogin(LoginResult loginResult )
    {
        var tokenEntityID = loginResult.EntityToken.Entity.Id;
        var entitiyType = loginResult.EntityToken.Entity.Type;
        PlayfabID = loginResult.PlayFabId;
    }
}
