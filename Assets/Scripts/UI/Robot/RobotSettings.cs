using UnityEngine;
public enum RobotEarAnimationType
{
    Idle,
    ZigZag,
    Wiggle,
    SmallToLarge,
    Wave
}
[CreateAssetMenu(fileName ="New Robot Settings",menuName = "ScriptableObjects/Robot/Settings")]
public class RobotSettings : ScriptableObject
{
    [SerializeField] public RobotEarAnimationType StartingEarAnimation;
    [SerializeField] public RobotEyeSO StartingEyeType;
    [Header("End Animation")]
    [Tooltip("When True will have ending parameters")]
    [SerializeField] public bool IsAnimating = true;
    [SerializeField] public float AnimationTime;
    [SerializeField] public RobotEarAnimationType EndingEarAnimation;
    [SerializeField] public RobotEyeSO EndingEyeType;

    public string GetAnimationTrigger(RobotEarAnimationType animationType)
    {

        switch (animationType)
        {
            case RobotEarAnimationType.Idle:
                return "Idle";
            case RobotEarAnimationType.ZigZag:
                return "ZigZag";
            case RobotEarAnimationType.Wiggle:
                return "Wiggle";
            case RobotEarAnimationType.SmallToLarge:
                return "SmallToLarge";
            case RobotEarAnimationType.Wave:
                return "Wave";
            default:
                return null;
        }
    }
}
