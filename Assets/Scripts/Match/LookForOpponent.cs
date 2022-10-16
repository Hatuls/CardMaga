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
        public static event Action OnStartLooking;
        public static event Action OnNoOpponentFound;
        public static event Action<string,CharactersData> OnOpponentFound;


        [Serializable]
        public class Rootobject
        {
            public string CharacterData;
            public string ArenaData;
        }


        [SerializeField]
        private IDisposable _token;


        private string _opponentDisplayName;
        
        public void Init(ITokenReciever tokenReceiver)
        {
            _token = tokenReceiver.GetToken();
            LookForOpponentOnServer();
            OnStartLooking?.Invoke();
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
                OnNoOpponentFound?.Invoke();
                _token.Dispose();
            }
        }

        private bool GetRandomOpponent(GetLeaderboardAroundPlayerResult obj, out string opponentPlayfabID)
        {
            List<PlayerLeaderboardEntry> optionalOpponents = new List<PlayerLeaderboardEntry>();

            //un comment when you have answer for no players
            // string myPlayfabID = Account.AccountManager.Instance.LoginResult.PlayFabId;

            for (int i = 0; i < obj.Leaderboard.Count; i++)
            {

                string player = obj.Leaderboard[i].PlayFabId;

                if (player == "")
                    continue;
            
                //un comment when you have answer for no players
                //    if (myPlayfabID != player)
                optionalOpponents.Add(obj.Leaderboard[i]);

            }

            int rndIndexOpponent = UnityEngine.Random.Range(0, optionalOpponents.Count);

            opponentPlayfabID = optionalOpponents.Count == 0 ? "" : optionalOpponents[rndIndexOpponent].PlayFabId;

            _opponentDisplayName = optionalOpponents[rndIndexOpponent]?.DisplayName ?? "";

            return optionalOpponents.Count != 0;
        }

        private void OnOpponentsDataReceived(ExecuteCloudScriptResult obj)
        {

            Rootobject charactersData = JsonUtility.FromJson<Rootobject>(obj.FunctionResult.ToString());

            // contain info about the characters and decks
            var opponentCharacter = JsonUtility.FromJson<CharactersData>(charactersData.CharacterData);

            // Contain Info About the arena
            var opponentArena = JsonUtility.FromJson<ArenaData>(charactersData.ArenaData);


            OnOpponentFound?.Invoke(_opponentDisplayName,opponentCharacter);
            _token.Dispose();
        }
      


        private void OnMatchFailed(PlayFabError obj)
        {
            throw new Exception(obj.GenerateErrorReport());
        }


    }

}