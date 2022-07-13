using CardMaga.Playfab;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using UnityEngine;

public class GoogleManager : MonoBehaviour
{
    [SerializeField]
    private PlayfabManager _pfManager;

    [SerializeField]
    private TextMeshProUGUI _txt;



    public void TrySignInWithGoogle()
    {
        _txt.text = "Activate Google";
        Debug.LogWarning(1);

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
            Debug.LogWarning(5);

            Social.localUser.Authenticate((bool success) =>
            {
                Debug.LogWarning(6);
                if (success)
                {
                    Debug.LogWarning(7);
                    _txt.text = "Google Signed In";
                    // get server authentication string
                    string serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                    // login with playfab through google
                    _pfManager.PlayFabLogin.LoginWithGoogleAccount(serverAuthCode);
                    Debug.LogWarning(8);
                }
                else
                {
                    Debug.LogWarning(9);
                    _txt.text = "Google Failed to Authorize your login";
                }

            });
        }
        catch (System.Exception m)
        {
           _txt.text = "Error : " + m.Message + "\n";
            throw m;
        }
       _txt.text = "Authenticated";
        Debug.LogWarning(10);
    }



}
