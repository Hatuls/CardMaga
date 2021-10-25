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
            isFirstUpdate = false;
            SceneHandler.LoaderCallback();
        }

        _loadingBar.value = SceneHandler.LoadingProgress();
    }
}
