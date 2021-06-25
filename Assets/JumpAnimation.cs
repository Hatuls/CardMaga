
using UnityEngine;

public class JumpAnimation : CharacterBaseStateMachine
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
        GetAnimatorController(animator).OnStartAnimation(stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetAnimatorController(animator).OnFinishAnimation(stateInfo);
    }

}
public class CharacterBaseStateMachine : StateMachineBehaviour
{
    private AnimatorController _animatorController;

    public AnimatorController GetAnimatorController(Animator animator)
    {
        if (_animatorController == null)
            _animatorController = animator.GetComponent<AnimatorController>();

        return _animatorController;
    }
}