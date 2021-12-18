using UnityEditor;
using UnityEngine;

public class SceneHelper
{
    static string NetworkSceneName = "NetworkScene";
    static string LoadingSceneName = "LoadingScene";
    static string BattleSceneName = "Game Battle Scene";
    static string MainMenuSceneName = "Main Menu Scene";
    static string MapSceneName = "Map Scene";


    [MenuItem("Scenes/Network Scene")]
    private static void NetworkScene()
    {
        LoadScene(NetworkSceneName);
    }
    [MenuItem("Scenes/Battle Scene")]
    private static void BattleScene()
    {
        LoadScene(BattleSceneName);
    }
    [MenuItem("Scenes/Map Scene")]
    private static void MapScene()
    { LoadScene(MapSceneName); }
    [MenuItem("Scenes/Main Menu Scene")]
    private static void MainMenuScene()
    {
        LoadScene(MainMenuSceneName);
    }

    [MenuItem("Scenes/Loading Scene Scene")]
    private static void LoadingScene()
    {
        LoadScene(LoadingSceneName); }
    private static void LoadScene(string sceneName)
    {
        UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(string.Concat(Application.dataPath, "/Scenes/GameScene/", sceneName, ".unity"), UnityEditor.SceneManagement.OpenSceneMode.Single);

    }
}