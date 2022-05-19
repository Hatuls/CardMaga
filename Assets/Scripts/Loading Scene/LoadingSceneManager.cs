using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace CardMaga.LoadingScene
{
    public class LoadingSceneManager : MonoBehaviour
    {
        public static Action<LoadingSceneManager> OnScenesEnter = null;
        public static Action<float> OnSceneProgress = null;
        private static List<int> _currentlyActiveScenes = new List<int>();

        public void LoadScenes(Action OnComplete = null, params int[] sceneBuildIndex)
       => StartCoroutine(LoadCoroutine(OnComplete, sceneBuildIndex));
        public void UnloadScenes(Action OnComplete = null, params int[] sceneBuildIndex)
            => StartCoroutine(UnLoadCoroutine(OnComplete, sceneBuildIndex));
        public void UnloadScenes(Action OnComplete = null)
          => StartCoroutine(UnLoadCoroutine(OnComplete, _currentlyActiveScenes.ToArray()));
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
            OnComplete?.Invoke();
        }

    }


}
