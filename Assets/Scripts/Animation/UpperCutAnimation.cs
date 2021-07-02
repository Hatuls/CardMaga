
using UnityEngine;

public class UpperCutAnimation :  CharacterBaseStateMachine
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    //    animator.transform.position = animator.transform.position - Vector3.left * 16f;

        GetAnimatorController(animator).OnStartAnimation(stateInfo);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetAnimatorController(animator).OnFinishAnimation(stateInfo);
    }
}
