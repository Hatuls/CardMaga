using FMOD;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transition SO", menuName = "ScriptableObjects/Transitions/New Transition SO")]
public class TransitionPackSO : ScriptableObject
{
    public bool HaveMovement = true; 
    public Transition3D Movement;

    public enum ScaleTypeEnum
    {
        ByFloat,
        ByVector
    };

    public ScaleTypeEnum ScaleType;
    public bool HaveScale = false; 
    public float ScaleMultiplier;
    public Vector3 ScaleVector;
    public Transition3D Scale;
    
    public bool HaveRotation = false; 
    public Vector3 Rotate; 
    public Transition3D Rotation;
}
