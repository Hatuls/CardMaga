using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/VFX SO")]
public class VFXSO : ScriptableObject , IStayOnTarget
{
    [PreviewField(100f)]
    [SerializeField] GameObject _vfxPrefab;
    public GameObject VFXPrefab => _vfxPrefab;


    [SerializeField]
    bool _stayOnTarget;

    [InfoBox("Instuctions:\n-1 will mean no delay\n")]
    [SerializeField]
    float _delayUntillDetach;
    public bool StayOnTarget => _stayOnTarget;

    public float DelayUntillDetach => _delayUntillDetach;


}
