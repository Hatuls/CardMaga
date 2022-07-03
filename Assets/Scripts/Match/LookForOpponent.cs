using ReiTools.TokenMachine;
using System;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using Battle.Data;

public class LookForOpponent : MonoBehaviour
{

   private IDisposable _token;

    public void Init(ITokenReciever tokenReceiver)
    {
        _token = tokenReceiver.GetToken();
        LookForOpponentOnServer();
    }

    private void LookForOpponentOnServer()
    {
        var request = new GetLeaderboardAroundPlayerRequest()
        {
             StatisticName = "Rank",
            MaxResultsCount = 5,
            PlayFabId = Account.AccountManager.Instance.LoginResult.PlayFabId,

        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnPotentialOpponentFound, OnMatchFailed);
    }

    private void OnMatchFailed(PlayFabError obj)
    {
        throw new Exception(obj.GenerateErrorReport());
    }

    private void OnPotentialOpponentFound(GetLeaderboardAroundPlayerResult obj)
    {
        string opponentID = obj.Leaderboard[0].PlayFabId;

        //if (opponentID == Account.AccountManager.Instance.LoginResult.PlayFabId) // if this is me
        //{
        //    LookForOpponentOnServer();
        //    return;
        //}
        var request = new PlayFab.ServerModels.GetUserDataRequest()
        {
             PlayFabId  = opponentID,
        };
        PlayFabServerAPI.GetUserData(request, OnOpponentDataRecieved, OnMatchFailed);

    }

    private void OnOpponentDataRecieved(PlayFab.ServerModels.GetUserDataResult obj)
    {
           if (obj == null)
            OnMatchFailed(null);


        var opponent = new Account.AccountData(obj.Data);
        var characters = opponent.CharactersData;
        Debug.LogError(opponent.DisplayName);

        var opponentCharacter = characters.Characters[characters.MainCharacter];

        BattleData.Instance.AssginCharacter(false, opponentCharacter);
    }
}
