using UnityEngine;

public class SceneLoaderCallback : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneHandler.SceneLoaderCallback = this;
    }
    public void LoadScene()
    {
        SceneHandler.LoadScene(SceneHandler.ScenesEnum.Battle);
    }

}