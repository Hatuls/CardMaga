using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

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
            gameObject.tag = "Web";
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