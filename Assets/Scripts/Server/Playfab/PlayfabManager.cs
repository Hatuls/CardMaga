using CardMaga.LoadingScene;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Playfab
{

    public class PlayfabManager : MonoBehaviour
    {
        public static Action<PlayfabManager> OnSceneEnter;
        public PlayfabLogin PlayFabLogin { get; private set; }





        #region Recieve From Server
        public void GetUserData(Action<GetUserDataResult> onUserDataRecieved, Action<PlayFabError> OnRequestFailed)
        =>  PlayFabClientAPI.GetUserData(new GetUserDataRequest(), onUserDataRecieved, OnRequestFailed);
        public void GetTitleData(Action<GetTitleDataResult> onTitleDataRecieved, Action<PlayFabError> OnRequestFailed)
            => PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), onTitleDataRecieved, OnRequestFailed);
        #endregion

        #region Send Data To Server
        public void SendUserData(Dictionary<string, string> data, Action<UpdateUserDataResult> onDataSendSuccessfully, Action<PlayFabError> OnRequestFailed)
        {
            var request = new UpdateUserDataRequest
            {
                Data = data
            };

            PlayFabClientAPI.UpdateUserData(request, onDataSendSuccessfully, OnRequestFailed);
        }
  
        #endregion

        private void Start()
        {
            PlayFabLogin = new PlayfabLogin();
            OnSceneEnter?.Invoke(this);
        }
    }


}