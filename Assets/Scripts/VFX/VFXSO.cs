using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/VFX SO")]
public class VFXSO : ScriptableObject , IStayOnTarget
{
    [PreviewField(100f)]
    [SerializeField] GameObject _vfxPrefab;
    public GameObject VFXPrefab => _vfxPrefab;

    [SerializeField]
    BodyPartEnum _defaultBodyPart;


    [SerializeField]
    bool toUseTransformRotation;

    [SerializeField]
    bool _isFromAnimation;

    public bool IsFromAnimation => _isFromAnimation;


    public bool ToUseBodyRotation => toUseTransformRotation;

    public BodyPartEnum DefaultBodyPart => _defaultBodyPart;
}
