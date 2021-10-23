
using UnityEngine;

public class IdleAnimationStateMachine : CharacterBaseStateMachine
{ 
    bool toDetect = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        toDetect = true;  
        if (toDetect)
        {
            GetAnimatorController(animator).CheckForRegisterCards();
            toDetect = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
   
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