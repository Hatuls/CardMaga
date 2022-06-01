using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CardMaga.LoadingScene
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public static Action<LoadingSceneManager> OnIjectingLoadingSceneManager = null;
        public static Action<float> OnSceneProgress = null;
        private static List<int> _currentlyActiveScenes = new List<int>();

        public static bool IsSceneActive(int BuildIndex)
            => _currentlyActiveScenes.Contains(BuildIndex);




        /// <summary>
        /// Load The Current Active Scenes 
        /// Note: Persistent scene will never be unloaded and never should
        /// </summary>
        /// <param name="OnComplete"></param>
        public void LoadScenes(Action OnComplete = null, params int[] sceneBuildIndex)
       => StartCoroutine(LoadCoroutine(OnComplete, sceneBuildIndex));

        /// <summary>
        /// Unload The given scenes index
        /// Note: Persistent scene will never be unloaded and never should
        /// </summary>
        /// <param name="OnComplete"></param>
        public void UnloadScenes(Action OnComplete = null, params int[] sceneBuildIndex)
            => StartCoroutine(UnLoadCoroutine(OnComplete, sceneBuildIndex));

        /// <summary>
        /// Unload The Current Active Scenes 
        /// Note: Persistent scene will never be unloaded and never should
        /// </summary>
        /// <param name="OnComplete"></param>
        public void UnloadScenes(Action OnComplete = null)
          => StartCoroutine(UnLoadCoroutine(OnComplete, _currentlyActiveScenes.ToArray()));

        /// <summary>
        /// Unload the current active scenes and then will start load the new scenes
        /// Note: Persistent scene will not be unloaded and should never be unloaded!
        /// </summary>
        /// <param name="OnComplete"></param>
        /// <param name="sceneBuildIndex"></param>
        public void UnloadAndThenLoad(Action OnComplete = null, params int[] sceneBuildIndex)
      => StartCoroutine(UnloadAndLoadCoroutine(OnComplete, sceneBuildIndex));
        private IEnumerator UnloadAndLoadCoroutine(Action OnComplete = null, params int[] sceneBuildIndex)
        {
            yield return UnLoadCoroutine(null, _currentlyActiveScenes.ToArray());
            GC.Collect();
            yield return null;
            yield return LoadCoroutine(OnComplete, sceneBuildIndex);
        }

        //private IEnumerator UnloadAndLoadCoroutine(Action OnComplete = null, params int[] sceneBuildIndex)
        //{
        //    if (sceneBuildIndex.Length > 0)
        //    {
        //        float sceneProgression = 0;
        //        int scenesCount = sceneBuildIndex.Length;
        //        AsyncOperation[] operations = new AsyncOperation[scenesCount];


        //        for (int i = 0; i < scenesCount; i++)
        //        {
        //            //Load scenes
        //            operations[i] = SceneManager.LoadSceneAsync(sceneBuildIndex[i], LoadSceneMode.Additive);
        //            operations[i].allowSceneActivation = false;
        //        }

        //        bool allSceneCompletedLoaded = true;
        //        yield return UnLoadCoroutine(null, _currentlyActiveScenes.ToArray());
        //        do
        //        {
        //            allSceneCompletedLoaded = true;
        //            // update progression
        //            for (int i = 0; i < scenesCount; i++)
        //                sceneProgression += operations[i].progress / scenesCount;

        //            yield return null;

        //            // check if scene finished loading
        //            for (int i = 0; i < scenesCount; i++)
        //            {
        //                var current = operations[i];

        //                if (current.progress >= 0.9f)
        //                    current.allowSceneActivation = true;

        //                allSceneCompletedLoaded &= current.isDone;
        //            }

        //        } while (!allSceneCompletedLoaded);

        //        yield return null;

        //        // add the new scenes
        //        for (int i = 0; i < sceneBuildIndex.Length; i++)
        //            if (!_currentlyActiveScenes.Contains(sceneBuildIndex[i]))
        //                _currentlyActiveScenes.Add(sceneBuildIndex[i]);
        //    }
        //    OnIjectingLoadingSceneManager?.Invoke(this);
        //    OnComplete?.Invoke();
        //}
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
                AsyncOperation[] operations = new AsyncOperation[scenesCount];


                for (int i = 0; i < scenesCount; i++)
                {
                    //Load scenes
                    operations[i] = SceneManager.LoadSceneAsync(sceneBuildIndex[i], LoadSceneMode.Additive);
                    operations[i].allowSceneActivation = false;
                }

                bool allSceneCompletedLoaded;
                do
                {
                    allSceneCompletedLoaded = true;
                    // update progression
                    for (int i = 0; i < scenesCount; i++)
                        sceneProgression += operations[i].progress / scenesCount;

                    yield return null;

                    // check if scene finished loading
                    for (int i = 0; i < scenesCount; i++)
                    {
                        var current = operations[i];
                        if (current.progress >= 0.9f)
                        {
                            current.allowSceneActivation = true;
                            if (!_currentlyActiveScenes.Contains(sceneBuildIndex[i]))
                                _currentlyActiveScenes.Add(sceneBuildIndex[i]);
                        }
                        allSceneCompletedLoaded &= current.isDone;
                    }

                } while (!allSceneCompletedLoaded);
                yield return null;
            }
            OnIjectingLoadingSceneManager?.Invoke(this);
            OnComplete?.Invoke();
        }

    }


}
