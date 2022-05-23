using UnityEngine;

[CreateAssetMenu(fileName = "New SceneIdentifcationSO", menuName = "ScriptableObjects/Identification/Scenes")]
public class SceneIdentificationSO : ScriptableObject
{
    [SerializeField]
    private int _sceneBuildIndex;

    public int SceneBuildIndex => _sceneBuildIndex;
}