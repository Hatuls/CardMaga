using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CardMaga.LoadingScene
{
    public class LoadingSceneManager : MonoBehaviour
    {

        /// <summary>
        /// Notify The Current Listens when the scene is start to be unloaded
        /// </summary>
        public static Action<IRecieveOnlyTokenMachine> OnBeforeSceneUnLoading = null;
        /// <summary>
        /// Notify the listeners on that wi
        /// </summary>
        public static Action<IRecieveOnlyTokenMachine> OnSceneLoaded = null;
        public static Action<IRecieveOnlyTokenMachine> OnMiddleSceneTransition = null;
        public static Action<LoadingSceneManager> OnScenesEnter = null;
        public static Action<float> OnSceneProgress = null;
        private static TokenMachine _sceneTransitionTokenMachine;
        private static TokenMachine _beforeSceneUnloading;
        private static TokenMachine _afterSceneLoading;
        private static List<int> _currentlyActiveScenes = new List<int>();
    

        public void LoadScenes(Action OnComplete = null, params int[] sceneBuildIndex)
       => StartCoroutine(LoadCoroutine(OnComplete, sceneBuildIndex));
        public void UnloadScenes(Action OnComplete = null, params int[] sceneBuildIndex)
            => StartCoroutine(UnLoadCoroutine(OnComplete, sceneBuildIndex));

        public void UnLoadAndThenLoad(Action OnComplete = null, params int[] scenesToLoad)
        {

            //Making Sure every thing that need to know before this operation will do their preperation
            _beforeSceneUnloading = new TokenMachine(StartOperation);

            //starting the operation
            using (IDisposable token = _beforeSceneUnloading.GetToken())
            {
                OnBeforeSceneUnLoading?.Invoke(_beforeSceneUnloading); // Notify others
            } //<- automatically will end the operation 


            //On _beforeSceneUnloading finish execute this V 
            void StartOperation()
            {
                //Create new token machine to those who need to be prepared 
                _afterSceneLoading = new TokenMachine(() => OnSceneLoaded?.Invoke(_afterSceneLoading), OnComplete);


                IDisposable taskCompleted = null;
                _sceneTransitionTokenMachine = new TokenMachine(
                    //  Will release the token after the scenes that were suppose to be unloaded were unloaded successfully
                    () => UnloadScenes(null, _currentlyActiveScenes.ToArray()),

                    //Notify new scene's monobehaviour scripts that the scene was activated
                    () => LoadScenes(
                        () =>
                        {
                            using (IDisposable token = _afterSceneLoading.GetToken())
                                OnSceneLoaded?.Invoke(_afterSceneLoading); // notify
                        },
                        scenesToLoad)
                    );

                //Start the whole operation ^
                using (_sceneTransitionTokenMachine.GetToken())
                    OnMiddleSceneTransition?.Invoke(_sceneTransitionTokenMachine);
            }

        }

        private IEnumerator UnLoadCoroutine(Action OnComplete = null, params int[] sceneBuildIndex)
        {
            if (sceneBuildIndex.Length > 0)
            {

                float sceneProgression = 0;
                int scenesCount = sceneBuildIndex.Length;
                for (int i = 0; i < scenesCount; i++)
                {
                    var task = SceneManager.UnloadSceneAsync(sceneBuildIndex[i]);
                    // Load Scene
                    do
                    {
                        sceneProgression += task.progress / scenesCount;
                        yield return null;

                    } while (!task.isDone);

                    // Remove From CurrentActive Scenes
                    _currentlyActiveScenes.Remove(sceneBuildIndex[i]);
                }
            }
            // completed unloading all the scene
            OnComplete?.Invoke();
        }

        private IEnumerator LoadCoroutine(Action OnComplete = null, params int[] sceneBuildIndex)
        {
            if (sceneBuildIndex.Length > 0)
            {
                float sceneProgression = 0;
                int scenesCount = sceneBuildIndex.Length;
                for (int i = 0; i < scenesCount; i++)
                {
                    //Load scenes
                    var task = SceneManager.LoadSceneAsync(sceneBuildIndex[i], LoadSceneMode.Additive);
                    do
                    {
                        sceneProgression += task.progress / scenesCount;
                        yield return null;

                    } while (!task.isDone);

                    //Add to active scene List
                    if (!_currentlyActiveScenes.Contains(sceneBuildIndex[i]))
                        _currentlyActiveScenes.Add(sceneBuildIndex[i]);
                }
            }
            OnScenesEnter?.Invoke(this);
            OnSceneLoaded?.Invoke(_afterSceneLoading);
            OnComplete?.Invoke();
        }

    }


}
