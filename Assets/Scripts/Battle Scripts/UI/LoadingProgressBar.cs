using UnityEngine.UI;
using UnityEngine;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] Slider _loadingBar;


   
    bool isFirstUpdate = true;


    void Update()
    {
        if (isFirstUpdate)
        {
            _loadingBar.value = 0;
            isFirstUpdate = false;
            SceneHandler.LoaderCallback();
            
        }

        _loadingBar.value = 1-SceneHandler.LoadingProgress();
        if (SceneHandler.LoadingComplete)
            SceneHandler.UnloadScene(SceneHandler.ScenesEnum.LoadingScene);
        
    }
}
