
using Relics;
using System;
using UnityEngine;
public class AnimatorController : MonoBehaviour
{
   
    [SerializeField]
    Animator _playerAnimator;
    Vector3 startPos;
    [SerializeField] Transform targetToLookAt;
    bool _isAnimationPlaying;
    public bool GetIsAnimationCurrentlyActive => _isAnimationPlaying;

    private void Start()
    {
        ResetAnimator();
    }

    


    public void ResetAnimator()
    {
        startPos = transform.position;
        _isAnimationPlaying = false;
        _playerAnimator.SetBool("IsDead", false);
        _playerAnimator.SetBool("IsWon", false);
        _playerAnimator.SetInteger("AnimNum",-1); // idle animation
    }
    public void PlayAnimation (Cards.CardNamesEnum cardName)
    {
        if(_playerAnimator == null)
        {
            Debug.LogError("Error in PlayAnimation");
            return;
        }

        Debug.Log($"Play Animation {cardName}");
        _playerAnimator.SetInteger("AnimNum",(int)cardName);
       
    }

    public void ResetToIdle()
    {
        if (_playerAnimator == null)
        {
            Debug.LogError("Error in ResetToIdle");
            return;
        }

        transform.position = startPos;
       // transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt,transform);
        _playerAnimator.SetInteger("RelicAnim", -1);
        _playerAnimator.SetInteger("AnimNum", -1);
    }


    public void OnStartAnimation(AnimatorStateInfo info)
    {
     //   Debug.Log("<a>Animation Playing</a> " + _playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        _isAnimationPlaying = true;

        _playerAnimator.SetInteger("AnimNum", -1);
    }   
    internal void OnFinishAnimation(AnimatorStateInfo stateInfo)
    {
        _isAnimationPlaying = false;
       // transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);
    }
    public void CharacterWon()
    {
        _isAnimationPlaying = false;
        ResetToIdle();
        _playerAnimator.SetBool("IsWon", true);
       _playerAnimator.SetInteger("AnimNum", -2);
        transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + Vector3.left , transform.position));
    }
    public void CharacterIsDead()
    {
        _playerAnimator.SetInteger("AnimNum", -2);
        _playerAnimator.SetBool("IsDead", true);
    }



    [SerializeField] float TransitionSpeed= 0.1f;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
       //     _playerAnimator.SetInteger("AnimNum", 13);
       //     _playerAnimator.CrossFade("HeadButt", TransitionSpeed);
           _playerAnimator.CrossFade("HeadButt", TransitionSpeed);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //_playerAnimator.SetInteger("AnimNum", 9);
            _playerAnimator.CrossFade("Jab", TransitionSpeed);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
            _playerAnimator.CrossFade("UpperCut", TransitionSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            _playerAnimator.CrossFade("HighKick", TransitionSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            _playerAnimator.CrossFade("HeelKick", TransitionSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            _playerAnimator.CrossFade("RightBlock", TransitionSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            _playerAnimator.CrossFade("LeftBlock", TransitionSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            _playerAnimator.CrossFade("UpperBlock", TransitionSpeed);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            _playerAnimator.CrossFade("Combo", TransitionSpeed);
        
    }

    internal void PlayRelicAnimation(RelicNameEnum getRelicName)
    {
        if (_playerAnimator == null)
        {
            Debug.LogError("Error in PlayAnimation");
            return;
        }

        Debug.Log($"Play Animation {getRelicName}");
        _playerAnimator.SetInteger("RelicAnim", (int)getRelicName);
    }
}
