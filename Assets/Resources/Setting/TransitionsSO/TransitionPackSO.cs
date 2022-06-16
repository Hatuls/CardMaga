using UnityEngine;

[CreateAssetMenu(fileName = "New Transition SO", menuName = "ScriptableObjects/Transitions/New Transition SO")]
public class TransitionPackSO : ScriptableObject, ITransitionReciever
{
    [Header("Movement:")] [SerializeField] private Transition3DData movement2DData;
    [Header("Scale:")] [SerializeField] private Transition1DData scale2DData;
    [Header("Rotation:")] [SerializeField] private Transition1DData rotation2DData;
    public ITransitionable3D Movement => movement2DData;
    public ITransitionable1D Scale => scale2DData;
    public ITransitionable1D Rotation => rotation2DData;
}

public interface ITransitionReciever
{
    ITransitionable3D Movement { get; }
    ITransitionable1D Scale { get; }
    ITransitionable1D Rotation { get; }
}