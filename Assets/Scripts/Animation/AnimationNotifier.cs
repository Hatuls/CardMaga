
using UnityEngine;

public class AnimationNotifier :  CharacterBaseStateMachine
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    //    animator.transform.position = animator.transform.position - Vector3.left * 16f;

        GetAnimatorController(animator).StartAnimation(stateInfo);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetAnimatorController(animator).FinishAnimation(stateInfo);
    }
}
