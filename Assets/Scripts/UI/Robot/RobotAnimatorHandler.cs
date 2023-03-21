using CardMaga.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RobotAnimatorHandler : MonoBehaviour,ICheckValidation
{
    [SerializeField] Animator _earAnimator;
    [SerializeField] Image _eyeImage;
    [SerializeField] [ReadOnly] RobotSettings _robotSettings;
    RobotEarAnimationType _currentEarAnimationType;
    private float _animationTime;
    private bool _isAnimating;

    private void Start()
    {
        CheckValidation();
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
    public void InitRobotAnimation(RobotSettings robotSettings)
    {
        _robotSettings = robotSettings;
        _animationTime = _robotSettings.AnimationTime;
        _currentEarAnimationType = _robotSettings.StartingEarAnimation;
        _earAnimator.SetTrigger(_robotSettings.GetAnimationTrigger(_currentEarAnimationType));
        _eyeImage.sprite = _robotSettings.StartingEyeType.EyeSprite;
        _isAnimating = _robotSettings.IsAnimating;
    }
    public void OnTimerEnded()
    {
        _currentEarAnimationType = _robotSettings.EndingEarAnimation;
        _earAnimator.SetTrigger(_robotSettings.GetAnimationTrigger(_currentEarAnimationType));
        _eyeImage.sprite = _robotSettings.EndingEyeType.EyeSprite;
    }

    public void CheckValidation()
    {
        if (!_eyeImage)
        {
            Debug.LogError("Robot animator handler has no eye image");
        }
    }
}
