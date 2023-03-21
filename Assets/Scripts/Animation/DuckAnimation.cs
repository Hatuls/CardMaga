
using UnityEngine;

public class DuckAnimation : CharacterBaseStateMachine
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetAnimatorController(animator).StartAnimation(stateInfo);
    }
   
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GetAnimatorController(animator).FinishAnimation(stateInfo);
    }
}
