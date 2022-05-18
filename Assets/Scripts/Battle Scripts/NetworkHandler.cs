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
    [SerializeField] TextMeshProUGUI _status;
    [SerializeField] TextMeshProUGUI _myVersion;
    [SerializeField] TextMeshProUGUI _webVersion;
    [SerializeField] GameObject _btnLogin;
    [SerializeField] GameFactoryTerminal gameFactoryTerminal;
    private string path = "https://drive.google.com/uc?export=download&id=15g1v7OV3XyE9wR6GBYUfvujDqwwp5uss";

    GameVersion _gv;
    IDisposable _token;
    private void Awake()
    {

        LoadingSceneManager.OnSceneLoaded += Init;
        _webVersion.enabled = false;
        _status.enabled = false;

        _myVersion.text = string.Concat("Alpha Version: ", Application.version);

    }
    private void OnDestroy()
    {
        LoadingSceneManager.OnSceneLoaded -= Init;
    }
    public void Init(IRecieveOnlyTokenMachine token)
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
        InitAccount();
        //JsonUtilityHandler.LoadOverrideFromJson(text,_gv);
        CheckVersion(_gv);
        _token.Dispose();
    }
    [Sirenix.OdinInspector.Button]

    public async void InitAccount()
    {
        await gameFactoryTerminal.Init();
        await Task.Yield();
        await AccountManager.Instance.Init();
    }

    private void CheckVersion(GameVersion currentVersion)
    {
        _webVersion.enabled = true;
        _webVersion.text = "Version in the Web : " + currentVersion.Version;
        _status.enabled = true;
        if (PlayerPrefs.GetString(Version) == currentVersion.Version)
        {

            _status.text = "Status: Up to date!";
            //   _continueBtn.enabled = true;
        }
        else
        {
            _status.text = "Status: Not Up to date!";
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
