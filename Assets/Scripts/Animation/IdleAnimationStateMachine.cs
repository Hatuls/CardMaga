﻿
using UnityEngine;

public class IdleAnimationStateMachine : CharacterBaseStateMachine
{
   public bool? isPlayer;
    bool toDetect = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        toDetect = true;  
        if (toDetect)
        {
            GetAnimatorController(animator).CheckForRegisterCards();
            toDetect = false;
        }

        if (isPlayer == null)
            isPlayer = GetAnimatorController(animator).AnimatorIsPlayer;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GetAnimatorController(animator).AnimatorIsPlayer)
                   CheckPlayerTurn();

    }


    // redo
    private void CheckPlayerTurn()
    {
  
     //   Battle.Turns.TurnHandler.CheckPlayerTurnForAvailableAction();

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