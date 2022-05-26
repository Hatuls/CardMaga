using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GoogleManager : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnSuccessfullLogin;
    [SerializeField]
    private TextMeshProUGUI _txt;
    public void OnSignInButtonClicked()
    {
        _txt.text = "Activate Google";
        //Debug.Log("Authenticate!");
        //Social.localUser;
             
        try
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();
            PlayGamesPlatform.InitializeInstance(config);

            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;

            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();


            Social.localUser.Authenticate((bool success) => {

                if (success)
                {

                    _txt.text = "Google Signed In";
                    var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    Debug.Log("Server Auth Code: " + serverAuthCode);

                    PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
                    {
                        TitleId = PlayFabSettings.TitleId,
                        ServerAuthCode = serverAuthCode,
                        CreateAccount = true
                    }, SuccessfullLogin, FailedLogin);
                }
                else
                {
                    _txt.text = "Google Failed to Authorize your login";
                }

            });
        }
        catch (System.Exception m)
        {
           
        _txt.text = m.Message;
            throw;
        }
        //   PlayGamesPlatform.Activate();
        _txt.text = "Authenticated";
        //    PlayGamesPlatform.Instance.Authenticate(Social.localUser, AuthenticateLogin);
    }
    //private void AuthenticateLogin(bool success, string info)
    //{
    //    if (success)
    //    {

    //        Debug.Log("Google Signed In");
    //        PlayGamesPlatform.Instance.RequestServerSideAccess(false,
    //            (serverAuthCode) =>
    //            {
    //                Debug.Log("Server Auth Code: " + serverAuthCode);
    //                PlayFabClientAPI.LoginWithGoogleAccount(
    //                    new LoginWithGoogleAccountRequest()
    //                    {
    //                        TitleId = PlayFabSettings.TitleId,
    //                        ServerAuthCode = serverAuthCode,
    //                        CreateAccount = true
    //                    },
    //                    SuccessfullLogin,
    //                    FailedLogin
    //                    );
    //            });
    //        _txt.text = "Google Sign in!";
    //    }
    //    else
    //    {




    //        Debug.LogError("Google Failed to Authorize your login - " + info);
    //        _txt.text = "Google Failed to Authorize your login - " + info;

    //    }
    //}

  

    private void FailedLogin(PlayFabError error)
    {
        Debug.LogError("ERROR: " + error.ErrorMessage);
        _txt.text = "ERROR: " + error.ErrorMessage;
    }

    private void SuccessfullLogin(LoginResult loginResult)
    {
        Debug.Log("Success! We Logged in!");
        _txt.text = "Success! We Logged In!";
        OnSuccessfullLogin?.Invoke();
    }

}
