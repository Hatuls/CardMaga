using System;
using System.Threading.Tasks;
using Unity.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ReiTools.TokenMachine;
using static SceneHandler;

public class LoadingManager : MonoSingleton<LoadingManager>
{
    [SerializeField] VoidEvent _loadBattleEvent;
    SceneHandler _sceneHandler;
    [SerializeField]
    SceneTransitionSO SceneTransitionSO;
    [SerializeField]
    UnityEvent<float> _progressEvent;
    private void Start()
    {

        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
        {
          //  Init();
        }
    }
    public override void Init(ITokenReciever token)
    {


    }
    public void LoadScene(SceneHandler.ScenesEnum scenesEnum)
    {

      
    }


    public async void LoadScene2(SceneHandler.ScenesEnum s)
    {
     //await   RemoveSceneAsync(
     //    SceneHandler.CurrentScene,
     //    ()=> _ = AddSceneAsync(s, () => Debug.Log("FinishedLoading")));

    }
    private async Task RemoveSceneAsync(ScenesEnum scene, Action OnFinished = null)
    {
        //var operation = SceneManager.UnloadSceneAsync((int)scene);
        //do
        //{
        //    await Task.Yield();
        //} while (operation.isDone == false);
        //OnFinished?.Invoke();
     
    }

    private async Task AddSceneAsync(ScenesEnum scene, Action OnFinished = null)
    {
        //var operation = SceneManager.LoadSceneAsync((int)scene);
        //do
        //{
        //    await Task.Yield();
        //} while (operation.isDone == false);
        //OnFinished?.Invoke();

    }
}