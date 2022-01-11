using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New VFX SO", menuName = "ScriptableObjects/VFX/VFX SO")]
public class VFXSO : ScriptableObject , IStayOnTarget
{
    [PreviewField(100f)]
    [SerializeField] GameObject _vfxPrefab;

    public GameObject VFXPrefab => _vfxPrefab;

    public bool StayOnTarget => throw new System.NotImplementedException();

    public float DelayUntillDetach => throw new System.NotImplementedException();


}
