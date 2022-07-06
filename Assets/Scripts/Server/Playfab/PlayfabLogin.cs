using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;

namespace CardMaga.Playfab
{
    public class PlayfabLogin
    {

        public static event Action<PlayFabError> OnFailedLogin;
        public static event Action<LoginResult> OnSuccessfullLogin;


        [SerializeField]
        private LoginResult _playerLoginResult;


        private const string LoginResultFileName = "PlayFabAuthentication";


        public LoginResult PlayerLoginResult
        {
            get => _playerLoginResult;
            private set => _playerLoginResult = value;
        }

        public void LoginWithGoogleAccount(string serverAuthCode)
        {
            LoginResult loginResult = TryLoadLoginResult();

            Debug.Log("Try Login With Google Account");

            PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                ServerAuthCode = serverAuthCode,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetPlayerStatistics = true,
                    GetUserData = true
                },
                CreateAccount = true,
                AuthenticationContext = loginResult?.AuthenticationContext ?? null
            },
             SuccessfullLogin,
             FailedLogin
             ); 
        }


        public void LoginWithPlayfabCustomID()
        {
            LoginResult loginResult = TryLoadLoginResult();

            Debug.Log("Try Login With Playfab Custom ID");

            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                AndroidDevice = SystemInfo.deviceModel,
                CreateAccount = true,
                AuthenticationContext = loginResult?.AuthenticationContext ?? null,
               
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
                {
                    GetPlayerStatistics = true,
                    GetPlayerProfile = true,
                    GetUserData = true
                }
            },
             SuccessfullLogin,
             FailedLogin
        ) ;
        }


        private void FailedLogin(PlayFabError error)
        {

            Debug.LogError("ERROR: " + error.ErrorMessage);

            OnFailedLogin?.Invoke(error);

        }

        private void SuccessfullLogin(LoginResult loginResult)
        {

            Debug.Log("Success! We Logged in!");
            PlayerLoginResult = loginResult;
            SaveLoginResult();
            Account.AccountManager.Instance.OnLogin(loginResult);
            OnSuccessfullLogin?.Invoke(PlayerLoginResult);

        }

        private LoginResult TryLoadLoginResult()
        => SaveManager.Load<LoginResult>(LoginResultFileName, SaveManager.FileStreamType.FileStream);
        private void SaveLoginResult()
        => SaveManager.SaveFile(_playerLoginResult, LoginResultFileName, SaveManager.FileStreamType.FileStream, true);


    }
}