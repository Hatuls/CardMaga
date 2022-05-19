using Account;
using CardMaga.LoadingScene;
using Factory;
using ReiTools.TokenMachine;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NetworkHandler : MonoBehaviour
{
    const string Version = "Version";
    [SerializeField] TextMeshProUGUI _myVersion;

    [SerializeField] GameObject _btnLogin;
    
    private string path = "https://drive.google.com/uc?export=download&id=15g1v7OV3XyE9wR6GBYUfvujDqwwp5uss";

    GameVersion _gv;
    IDisposable _token;
    private void Awake()
    {
        _myVersion.text = string.Concat("Alpha Version: ", Application.version);

    }

    public void Init(ITokenReciever token)
    {
        _token = token.GetToken();
        FireBaseHandler.Init();
        UnityAnalyticHandler.SendEvent(Application.version);
        WebRequests.Get(
            path,
            (x)=> _token?.Dispose(),
           OnGameVersionRecieved
            );
    }
    private void OnGameVersionRecieved(string data)
    {
        _gv = JsonUtility.FromJson<GameVersion>(data);
        DefaultVersion._gameVersion = _gv;

        CheckVersion(_gv);
        _token?.Dispose();
    }
  
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
}
[System.Serializable]
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

public static class JsonUtilityHandler
{
    public static string ConvertObjectToJson(object data)
    => JsonUtility.ToJson(data);

    public static void LoadOverrideFromJson(string json, object data)
    => JsonUtility.FromJsonOverwrite(json, data);

    public static T LoadFromJson<T>(string json)
    => JsonUtility.FromJson<T>(json);

}
