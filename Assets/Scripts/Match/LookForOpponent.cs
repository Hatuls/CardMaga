using Account.GeneralData;
using Battle.Data;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Battle.MatchMaking
{

    public class LookForOpponent : MonoBehaviour
    {
        [Serializable]
        public class Rootobject
        {
            public string CharacterData;
            public string ArenaData;
        }



        [SerializeField]
        private IDisposable _token;
        [SerializeField]
        GameObject _searchForOpponent;



        private void Start()
        {
            _searchForOpponent.SetActive(false);
        }

        public void Init(ITokenReciever tokenReceiver)
        {
            _token = tokenReceiver.GetToken();
 
            LookForOpponentOnServer();

        }

        private void LookForOpponentOnServer()
        {
            _searchForOpponent.SetActive(true);
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "Rank",
                MaxResultsCount = 5,
                PlayFabId = Account.AccountManager.Instance.LoginResult.PlayFabId,

            };

            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnPotentialOpponentFound, OnMatchFailed);
        }



        private void OnPotentialOpponentFound(GetLeaderboardAroundPlayerResult obj)
        {
            if (GetRandomOpponent(obj, out string opponentPlayfabID))
            {
                var request = new ExecuteCloudScriptRequest()
                {
                    FunctionName = "LookForOpponent",
                    FunctionParameter = new
                    {
                        Opponent = opponentPlayfabID
                    }
                };

                PlayFabClientAPI.ExecuteCloudScript(request, OnOpponentsDataReceived, OnMatchFailed);
            }
            else
            {
                // No Options found call for bot

                _token.Dispose();
            }
        }

        private bool GetRandomOpponent(GetLeaderboardAroundPlayerResult obj,out string opponentPlayfabID)
        {
            List<string> optionalOpponents = new List<string>();

            //un comment when you have answer for no players
           // string myPlayfabID = Account.AccountManager.Instance.LoginResult.PlayFabId;

            for (int i = 0; i < obj.Leaderboard.Count; i++)
            {
                string player = obj.Leaderboard[i].PlayFabId;

                if (player == "")
                    continue;

                //un comment when you have answer for no players
                //    if (myPlayfabID != player)
                optionalOpponents.Add(player);

            }
            opponentPlayfabID = optionalOpponents.Count == 0 ? "" : optionalOpponents[UnityEngine.Random.Range(0, optionalOpponents.Count)];
            return optionalOpponents.Count != 0;
        }

        private void OnOpponentsDataReceived(ExecuteCloudScriptResult obj)
        {

            Rootobject charactersData = JsonUtility.FromJson<Rootobject>(obj.FunctionResult.ToString());

            // contain info about the characters and decks
            var opponentCharacter = JsonUtility.FromJson<CharactersData>(charactersData.CharacterData);

            // Contain Info About the arena
            var opponentArena = JsonUtility.FromJson<ArenaData>(charactersData.ArenaData);


            RegisterOpponent(opponentCharacter.GetMainCharacter);
            _token.Dispose();
        }
        private void RegisterOpponent(Character character)
            => BattleData.Instance.AssginCharacter(false, character);
        private void OnMatchFailed(PlayFabError obj)
        {
            throw new Exception(obj.GenerateErrorReport());
        }

    }

}