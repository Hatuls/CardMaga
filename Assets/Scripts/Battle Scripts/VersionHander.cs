using CardMaga.Playfab;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using TMPro;
using UnityEngine;
using CardMaga.Tools.Json;
public class VersionHander : MonoBehaviour
{
    public static event Action<PlayFabError> OnTitleDataRecievedFailed;
    public static event Action<PlayFabError> OnFailedToUpdateUsersVersion;
    public static event Action OnTitleCorruptedData;
    const string Version = "GeneralInfo";


    private CardMaga.General.GameVersion _serverVersion = null;

    [SerializeField]
    GameObject _versionUI;
    [SerializeField] 
    private TextMeshProUGUI _myVersion;
    [SerializeField]
    private HyperlinksSO _hyperlinksSO;

    private PlayfabManager _playfabManager;
    private string path = "https://drive.google.com/uc?export=download&id=15g1v7OV3XyE9wR6GBYUfvujDqwwp5uss";

    [SerializeField]
    private bool _ignoreServerVersion;


    GameVersion _gv;
    IDisposable _token;
    private void Awake()
    {
        _myVersion.text = string.Concat("Alpha Version: ", Application.version);
        PlayfabManager.OnSceneEnter += Inject;
        _versionUI.SetActive(false);


#if PRODUCTION_BUILD
        _ignoreServerVersion = false;
#endif
    }
    private void OnDestroy()
    {
        PlayfabManager.OnSceneEnter -= Inject;
    }
    private void Inject(PlayfabManager playfabManager)
    => _playfabManager = playfabManager;


    public void Init(ITokenReciever token)
    {

        FireBaseHandler.Init(token);
        UnityAnalyticHandler.SendEvent(Application.version);

        _token = token.GetToken();

        if (!_ignoreServerVersion)
            RequestVersions();
        else
            VersionMatch();


        WebRequests.Get(
            path,
           Debug.Log,
           OnGameVersionRecieved
            );
    }


    public void RequestVersions()
    {
        _playfabManager.GetTitleData(GetServerVersion, OnTitleDataRecievedFailed);
    }

    private void GetServerVersion(GetTitleDataResult obj)
    {
        if (obj != null && obj.Data != null && obj.Data.TryGetValue(Version, out string serverVersion))
        {
            _serverVersion = JsonUtilityHandler.LoadFromJson<CardMaga.General.GameVersion>(serverVersion);
            CheckVersion();
        }
        else
            OnTitleCorruptedData?.Invoke();
    }

    private void CheckVersion()
    {
        if (_serverVersion == null)
            return;
        else if (_serverVersion.Version == Application.version)
        {
            // Updating the version in the player
            _playfabManager.SendUserData(
                new System.Collections.Generic.Dictionary<string, string>
                {
                    { Version, Application.version }
                },
                (result) => Debug.Log("Updated user's Version"),
                 OnFailedToUpdateUsersVersion);
            VersionMatch();
        }
        else
        {
            _versionUI.SetActive(true);
        }
    }
    private void VersionMatch()
    {
        _token?.Dispose();
    }

    /// <summary>
    /// delete this
    /// </summary>
    /// <param name="data"></param>
    private void OnGameVersionRecieved(string data)
    {
        _gv = JsonUtility.FromJson<GameVersion>(data);
        DefaultVersion._gameVersion = _gv;

        CheckVersion(_gv);

    }


    /// <summary>
    /// delete this
    /// </summary>
    /// <param name="currentVersion"></param>
    private void CheckVersion(GameVersion currentVersion)
    {

        if (PlayerPrefs.GetString(Version) == currentVersion.Version)
        {


        }
        else
        {

            PlayerPrefs.SetString(Version, currentVersion.Version);
        }
    }

    public void QuitApp()
    => Application.Quit();

    public void GoToUpdateLink()
        => _hyperlinksSO.UseLink(HyperlinksSO.GooglePlayLink);
}

namespace CardMaga.General
{
    [Serializable]
    public class GameVersion
    {
        public string Version;
    }
}

[System.Serializable]
/// delete this and move this to playfab server
public class GameVersion
{
    public string Version;
    public int EnergyToPlay = 5;
    public int MaxEnergy = 30;
    public int Chips = 0;
    public int Diamonds = 0;
    public int EXP = 0;
    public int Level = 1;
}
public static class DefaultVersion
{
    public static GameVersion _gameVersion = new GameVersion();
}
namespace CardMaga.Tools.Json
{

    public static class JsonUtilityHandler
    {
        public static string ConvertObjectToJson(object data)
        => JsonUtility.ToJson(data);

        public static void LoadOverrideFromJson(string json, object data)
        => JsonUtility.FromJsonOverwrite(json, data);

        public static T LoadFromJson<T>(string json)
        => JsonUtility.FromJson<T>(json);
        public static string ToJson(this object data)
      => JsonUtility.ToJson(data);
    }

}