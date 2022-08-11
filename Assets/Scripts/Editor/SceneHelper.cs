using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;




public class SceneEditor : OdinMenuEditorWindow
{

    [MenuItem("Tools/Scenes")]
    private static void OpenWindow()
    {
        SceneEditor window = GetWindow<SceneEditor>();
        window.Show();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SceneEditorCache.OnSceneDeleted += ForceMenuTreeRebuild;
    }
    protected override void OnDestroy()
    {
        SceneEditorCache.OnSceneDeleted -= ForceMenuTreeRebuild;
        base.OnDestroy();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.DefaultMenuStyle.Offset = 1f;

        var result = AssetDatabase.FindAssets("t:SceneAsset", new string[] { "Assets/Scenes/Production Scenes" });
        SceneEditorCache[] productionScenes = new SceneEditorCache[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(result[i]);
            var currentScene = AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)) as SceneAsset;
            productionScenes[i] = new SceneEditorCache(false, currentScene);
            // Debug.Log("Path " + AssetDatabase.GUIDToAssetPath(guid));
            tree.Add("Production/" + currentScene.name, productionScenes[i]);
        }

        result = AssetDatabase.FindAssets("t:SceneAsset", new string[] { "Assets/Scenes/Test Scenes" });
        productionScenes = new SceneEditorCache[result.Length];
        for (int i = 0; i < result.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(result[i]);
            var currentScene = AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)) as SceneAsset;
            productionScenes[i] = new SceneEditorCache(true, currentScene);
            // Debug.Log("Path " + AssetDatabase.GUIDToAssetPath(guid));
            tree.Add("Others/" + currentScene.name, productionScenes[i]);
        }
        //    tree.AddAllAssetsAtPath("Test Scenes", "Assets/Scenes/Test Scenes", typeof(SceneAsset));

        return tree;
    }

    public class SceneEditorCache
    {
        public static event Action OnSceneDeleted;
        private bool _canBeDeleted;
        private SceneAsset _current;
        private List<Scene> _scenes = new List<Scene>();
        public SceneEditorCache(bool canBeDeleted, SceneAsset scene)
        {
            _current = scene;
            _canBeDeleted = canBeDeleted;
        }
        [Sirenix.OdinInspector.Button]
        public void LoadSingle()
        {

            Debug.Log($"Loading {_current.name} Scene");
            Reset();
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_current), OpenSceneMode.Single);
        }
        [Sirenix.OdinInspector.Button]
        public void LoadAdditive()
        {
            _scenes.Add(EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(_current), OpenSceneMode.Additive));
            Debug.Log($"Loading {_current.name} Scene Additive ");
        }

        [Sirenix.OdinInspector.Button]
        public void Unload()
        {
            for (int i = 0; i < _scenes.Count; i++)
                EditorSceneManager.CloseScene(_scenes[i], false);
            Reset();
            Debug.Log($"Unloading {_current.name} Scenes");
        }

        [Sirenix.OdinInspector.Button]
        public void Delete()
        {
            if (_canBeDeleted == false)
            {
                Debug.LogError("Scene Cannot be deleted from editor window!");
                return;
            }

            var path = AssetDatabase.GetAssetPath(_current);

            if (Directory.Exists(path))
            {
                Directory.Delete(path);
                OnSceneDeleted?.Invoke();
            }

            Debug.Log("Scene Deleted");
        }

        private void Reset() => _scenes.Clear();

    }
}

