using CardMaga.UI;
using UnityEngine;
using UnityEngine.UI;

enum RobotEarAnimationType
{
    Idle,
    ZigZag,
    Wiggle,
    SmallToBig,
    Wave
}
public class RobotAnimatorHandler : MonoBehaviour,ICheckValidation
{
    [SerializeField] Animator _earAnimator;
    [SerializeField] RobotEarAnimationType _earType;
    [SerializeField] RobotEyeSO _eyeType;
    [SerializeField] Image _eyeImage;

    private void Start()
    {
        CheckValidation();
        _earAnimator.SetTrigger(GetAnimationTrigger());
        _eyeImage.sprite = _eyeType.EyeSprite;
    }
    private string GetAnimationTrigger()
    {
        switch (_earType)
        {
            case RobotEarAnimationType.Idle:
                return "Idle";
            case RobotEarAnimationType.ZigZag:
                return "ZigZag";
            case RobotEarAnimationType.Wiggle:
                return "Wiggle";
            case RobotEarAnimationType.SmallToBig:
                return "SmallToBig";
            case RobotEarAnimationType.Wave:
                return "Wave";
            default:
                return null;
        }
    }

    public void CheckValidation()
    {
        if (!_eyeImage)
        {
            Debug.LogError("Robot animator handler has no eye image");
        }
    }
}
