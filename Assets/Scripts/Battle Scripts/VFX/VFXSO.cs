using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/VFX SO")]
public class VFXSO : ScriptableObject , IStayOnTarget
{
    [PreviewField(100f)]
    [SerializeField] GameObject _vfxPrefab;
    public GameObject VFXPrefab => _vfxPrefab;

    [SerializeField]
    bool toUseTransformRotation;
    [SerializeField]
    bool _stayOnTarget;
    [SerializeField]
    bool _isFromAnimation;

    [InfoBox("Instuctions:\n-1 will mean no delay\n")]
    [SerializeField]
    float _delayUntillDetach;
    public bool StayOnTarget => _stayOnTarget;
    public bool IsFromAnimation => _isFromAnimation;
    public float DelayUntillDetach => _delayUntillDetach;

    public bool ToUseBodyRotation => toUseTransformRotation;  
}
