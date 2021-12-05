using UnityEngine;
using UI.LoadingScreen;

public abstract class AbstScreenTransition : MonoBehaviour, IScreenTransition
{
    [SerializeField]
    protected Animator _screenAnimator;
    protected int _animHash;
    public abstract void StartTransition();
    public virtual void TransitionCompleted()
    {
        LoadingSceneManager.Instance.ScreenTransitionCompleted();
    }
}
