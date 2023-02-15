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
    [SerializeField] RobotEyeSO _startingEyeType;
    [SerializeField] RobotEyeSO _endingEyeType;
    [SerializeField] Image _eyeImage;
    [SerializeField] float _animationTime;
    [Tooltip("When True will have ending changes")]
    [SerializeField] bool _isAnimating = true;
    private void Start()
    {
        CheckValidation();
        _earAnimator.SetTrigger(GetAnimationTrigger());
        _eyeImage.sprite = _startingEyeType.EyeSprite;
    }
    private void Update()
    {
        if (_isAnimating)
        {
            if (_animationTime > 0)
            {
                _animationTime -= Time.deltaTime;
            }
            else
            {
                _isAnimating = false;
                OnTimerEnded();
            }
        }
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
    public void OnTimerEnded()
    {
        _earType = RobotEarAnimationType.Idle;
        _earAnimator.SetTrigger(GetAnimationTrigger());
        _eyeImage.sprite = _endingEyeType.EyeSprite;
    }

    public void CheckValidation()
    {
        if (!_eyeImage)
        {
            Debug.LogError("Robot animator handler has no eye image");
        }
    }
}
