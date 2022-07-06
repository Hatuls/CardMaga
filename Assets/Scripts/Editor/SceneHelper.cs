using UnityEditor;
using UnityEngine;

public class SceneHelper
{
    static string NetworkSceneName = "NetworkScene";
    static string LoadingSceneName = "LoadingScene";
    static string BattleSceneName = "Game Battle Scene";
    static string MainMenuSceneName = "Main Menu Scene";
    static string MapSceneName = "Map Scene";
    static string LoreSceneName = "Lore Scene";
    static string PersistentSceneName = "PersistentScene";
    static string MatchLookingSceneName = "Match Making Scene";



    [MenuItem("Scenes/Persistent Scene")]
    public static void PersistentScene()=>
        LoadScene(PersistentSceneName);

    [MenuItem("Scenes/Network Scene")]
    public static void NetworkScene()
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

    [MenuItem("Scenes/Loading Scene")]
    private static void LoadingScene()
    {
        LoadScene(LoadingSceneName); 
    }

    [MenuItem("Scenes/Match Making Scene")]
    public static void MatchLookingScene() =>
    LoadScene(MatchLookingSceneName);


    [MenuItem("Scenes/Lore Scene")]
    private static void LoreScene()
    {
        LoadScene(LoreSceneName);
    }
    private static void LoadScene(string sceneName)
    {
        if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(string.Concat(Application.dataPath, "/Scenes/GameScene/", sceneName, ".unity"), UnityEditor.SceneManagement.OpenSceneMode.Single);

    }
}