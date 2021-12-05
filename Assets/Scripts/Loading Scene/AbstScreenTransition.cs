using UnityEngine;
using UI.LoadingScreen;

public abstract class AbstScreenTransition : MonoBehaviour, IScreenTransition
{
    [SerializeField]
    protected Animator _screenAnimator;
    protected int _animHash;
    [SerializeField]
    protected GameObject _objectHolder;
    public virtual void StartTransition()
    {
        _objectHolder.SetActive(true);
    }
    
    public virtual void TransitionCompleted()
    {
        LoadingSceneManager.Instance.ScreenTransitionCompleted();
    }
}
