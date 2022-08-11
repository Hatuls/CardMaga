using UnityEngine;
[CreateAssetMenu(fileName = "New Model SO", menuName = "ScriptableObjects/Art/Models/New Model SO")]
public class ModelSO : ScriptableObject
{
    [Sirenix.OdinInspector.PreviewField(75f)]
    [SerializeField]
    private AvatarHandler _model;
    [Sirenix.OdinInspector.PreviewField(75f)]
    [SerializeField]
    private Material _normal;


    [SerializeField]
    private Material[] _tintedColors;

    public Material[] TintedColors { get => _tintedColors; }
    public AvatarHandler Model { get => _model; }
    public Material Normal { get => _normal; }

    public Material GetRandomTintedMaterials()
    {
        int length = _tintedColors.Length;
        if (length == 0)
            return _normal;
        else if (length == 1)
            return _tintedColors[0];
        else
            return _tintedColors[UnityEngine.Random.Range(0, length)];
    }
}

