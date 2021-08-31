using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NetworkHandler : MonoBehaviour
{
    public static System.Action CheckVersionEvent;
    const string Version = "Version";
    [SerializeField] TextMeshProUGUI _status;
    [SerializeField] TextMeshProUGUI _myVersion;
    [SerializeField] TextMeshProUGUI _webVersion;
    [SerializeField] Button _continueBtn;
    string path = "https://drive.google.com/uc?export=download&id=15g1v7OV3XyE9wR6GBYUfvujDqwwp5uss";

    GameVersion _gv;

    private void Awake()
    {
        _myVersion.enabled = false;
        _webVersion.enabled = false;
        _status.enabled = false;
        _continueBtn.enabled = false;
        
    }
    private void OnEnable()
    {
        CheckVersionEvent += Init;
    }
    private void OnDisable()
    {
        CheckVersionEvent -= Init;
    }
    public void Init()
    {
        _myVersion.enabled = true;
        _myVersion.text = "My Version is : " + PlayerPrefs.GetString(Version);


        WebRequests.Get(
            path,
            null,
            (string text) =>
            {
                //JsonUtilityHandler.LoadOverrideFromJson(text,_gv);
                _gv = JsonUtility.FromJson<GameVersion>(text);
                CheckVersion(_gv);
            }
            );


    }
    private void CheckVersion(GameVersion currentVersion)
    {
        _webVersion.enabled = true;
        _webVersion.text ="Version in the Web : " + currentVersion.Version;
        _status.enabled = true;
        if (PlayerPrefs.GetString(Version) == currentVersion.Version)
        {
    
           _status.text ="Status: Up to date!";
            _continueBtn.enabled=true;
        }
        else
        {
            _status.text = "Status: Not Up to date!";
            PlayerPrefs.SetString(Version, currentVersion.Version);
        }

    }



    [System.Serializable]
    class GameVersion
    {
        public string Version;
    }
}


public static class JsonUtilityHandler
{
    public static string ConvertObjectToJson (object data) 
    => JsonUtility.ToJson(data);
    
    public static void LoadOverrideFromJson(string json, object data)
    => JsonUtility.FromJsonOverwrite(json, data);
    
    public static T LoadFromJson<T>(string json) 
    =>JsonUtility.FromJson<T>(json);

} 
public static class WebRequests
{
    private class WebRequestMonoBehaviour : MonoBehaviour { }
    private static WebRequestMonoBehaviour _webRequestMonoBehaviour;

    private static void Init()
    {
        if (_webRequestMonoBehaviour == null)
        {
            GameObject gameObject = new GameObject("WebRequestMonoBehaviour");
            _webRequestMonoBehaviour = gameObject.AddComponent<WebRequestMonoBehaviour>();
        }
    }
    public static void Get(string url, System.Action<string> onError, System.Action<string> onSuccess)
    {
        Init();
        _webRequestMonoBehaviour.StartCoroutine(GetDataCoroutine(url, onError, onSuccess));
    }
    public static void Get(string url, System.Action<string> onError, System.Action<Texture2D> onSuccess)
    {
        Init();
        _webRequestMonoBehaviour.StartCoroutine(GetTextureCoroutine(url, onError, onSuccess));
    }
    private static IEnumerator GetTextureCoroutine(string url, System.Action<string> onError, System.Action<Texture2D> onSuccess)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (request.isDone == false)
            {
                Debug.Log(request.downloadProgress);
                yield return null;
            }
            //   yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
                onError?.Invoke(request.error);
            else
            {
                DownloadHandlerTexture downloadHandlerTexture = request.downloadHandler as DownloadHandlerTexture;
                onSuccess?.Invoke(downloadHandlerTexture.texture);
            }
        }
    }
    private static IEnumerator GetDataCoroutine(string url, System.Action<string> onError, System.Action<string> onSuccess)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SendWebRequest();
            while (request.isDone == false)
            {
                float value = request.downloadProgress;
                Debug.Log("Progress: " + value);

                yield return null;
            }

            Debug.Log("Unity Web Request Completed");
            if (request.isNetworkError || request.isHttpError)
                onError?.Invoke(request.error);

            else
                onSuccess?.Invoke(request.downloadHandler.text);
        }

    }
}