using CardMaga.LoadingScene;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    //[SerializeField] Slider _loadingBar;
    //public static event  Action OnFinishLoadingScene;
    //[Range(0, 1)]
    //[SerializeField] float _delayOffset = .5f;
    //float _delayTime = 0;
    //bool isFirstUpdate = true;
    //bool isFlag;
    //
    //private void Start()
    //{
    //    _loadingBar.onValueChanged.AddListener(OnLoadingBarLoaded);
    //    isFlag = true;
    //}
    //void Update()
    //{
    //    //if (isFirstUpdate)
    //    //{
    //    //    _loadingBar.value = 0;
    //    //    isFirstUpdate = false;
    //    //   // SceneHandler.LoaderCallback();
    //    //    _delayTime = 0;
    //    //}
    //    //_delayTime += _delayOffset * Time.deltaTime;
    //    //float loadingProgress = LoadingSceneManager.LoadingProgress();
    //    //float progressReveresed = 1 - loadingProgress;
    //    //float value = Mathf.Lerp(0, loadingProgress, _delayTime);
    //    //Debug.Log($"LoadingProgress {loadingProgress}\nReversed {progressReveresed}\nValue {value}");
    //    //_loadingBar.value = value;
    //
    //}
    //private void SetValue(float val) => _loadingBar.value = value;
    //public void OnLoadingBarLoaded(float val)
    //{
    //    if (_loadingBar.value == _loadingBar.maxValue && isFlag)
    //    {
    //        isFlag = false;
    //        _loadingBar.onValueChanged.RemoveListener(OnLoadingBarLoaded);
    //        CancellationTokenSource tokenSource = new CancellationTokenSource();
    //        try
    //        {
    //            SceneHandler.UnloadPreviousScene(tokenSource.Token, OnFinishLoadingScene);
    //        }
    //        catch (OperationCanceledException)
    //        {
    //            Debug.LogError("Loading Progress Bar: Token Was Canceled!");
    //        }
    //        finally
    //        {
    //
    //            tokenSource.Dispose();
    //
    //        }
    //
    //    }
    //}
}
