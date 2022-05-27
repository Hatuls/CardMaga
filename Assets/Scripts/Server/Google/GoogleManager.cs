using CardMaga.Playfab;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
    [SerializeField]
    private PlayfabManager _pfManager;

    //[SerializeField]
    //private TextMeshProUGUI _txt;



    public void TrySignInWithGoogle()
    {
 //       _txt.text = "Activate Google";


        try
        {
            //Creating Config
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .AddOauthScope("profile")
            .RequestServerAuthCode(false)
            .Build();
            PlayGamesPlatform.InitializeInstance(config);

            // recommended for debugging:
            PlayGamesPlatform.DebugLogEnabled = true;

            // Activate the Google Play Games platform
            PlayGamesPlatform.Activate();


            Social.localUser.Authenticate((bool success) =>
            {

                if (success)
                {
                //    _txt.text = "Google Signed In";
                    // get server authentication string
                    string serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    // login with playfab through google
                    _pfManager.PlayFabLogin.LoginWithGoogleAccount(serverAuthCode);
                }
                else
                {
               //     _txt.text = "Google Failed to Authorize your login";
                }

            });
        }
        catch (System.Exception m)
        {

     //       _txt.text = m.Message;
            throw m;
        }
     //   _txt.text = "Authenticated";
    }



}
