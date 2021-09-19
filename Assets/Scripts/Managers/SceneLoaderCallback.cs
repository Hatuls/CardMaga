using UnityEngine;

public class SceneLoaderCallback : MonoBehaviour
{

    public void LoadScene()
    {
        SceneHandler.LoadScene(SceneHandler.ScenesEnum.Battle);

    }

}