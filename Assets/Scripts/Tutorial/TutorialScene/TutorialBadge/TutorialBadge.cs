using CardMaga.Input;
using UnityEngine;

public class TutorialBadge : MonoBehaviour
{
    private static int OffAnimationHash = Animator.StringToHash("Tutorial_Off_Animation");
    private static int CompletedAnimationHash = Animator.StringToHash("Tutorial_Complete_Animation");
    private static int CurrentAnimationHash = Animator.StringToHash("Tutorial_Open_Animation");

    [SerializeField] private Animator _animator;
    [SerializeField] private TouchableItem _Input;



    public void Init()
    {
        _animator.Play(OffAnimationHash);
    }

    public void Completed()
    {
        _animator.Play(CompletedAnimationHash);
        _Input.Lock();
    }

    public void Open()
    {
        _animator.Play(CurrentAnimationHash);
        _Input.UnLock();
    }
}
