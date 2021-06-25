
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
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt,transform);
        _playerAnimator.SetInteger("RelicAnim", -1);
        _playerAnimator.SetInteger("AnimNum", -1);
    }


    public void OnStartAnimation(AnimatorStateInfo info)
    {
        Debug.Log("<a>Animation Playing</a> " + _playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        _isAnimationPlaying = true;

        _playerAnimator.SetInteger("AnimNum", -1);
    }   
    internal void OnFinishAnimation(AnimatorStateInfo stateInfo)
    {
        _isAnimationPlaying = false;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        _playerAnimator.SetBool("IsWon", true);
        }
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
