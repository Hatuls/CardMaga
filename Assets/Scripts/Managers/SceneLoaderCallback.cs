using UnityEngine;

[CreateAssetMenu (fileName = "SceneLoaderSO", menuName = "ScriptableObjects/SceneLoader")]
public class SceneLoaderCallback : ScriptableObject
{

    public void LoadScene(int destination)
    {
        SceneHandler.LoadScene((SceneHandler.ScenesEnum)destination);
    }

}