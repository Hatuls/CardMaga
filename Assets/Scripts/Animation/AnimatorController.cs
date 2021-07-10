﻿
using Relics;
using System.Collections.Generic;
using Unity.Events;
using UnityEngine;
using Cards;

public class AnimatorController : MonoBehaviour
{
    #region Events
    [HideInInspector]
    [SerializeField] VoidEvent _movedToNextAnimation; 
    [HideInInspector]
    [SerializeField] VoidEvent _onFinishedAnimation; 
    [HideInInspector]
    [SerializeField] VoidEvent _onAnimationDoKeyword;
    [SerializeField] IntEvent _moveCameraAngle;
    #endregion

    #region Fields
    [SerializeField] Animator _playerAnimator;
    [SerializeField] Transform targetToLookAt;


    Queue<string>  _animationQueue = new Queue<string>();

    [SerializeField] float _rotationSpeed;
    [SerializeField] float _positionSpeed;
    public bool IsMyTurn;

    [SerializeField] float _transitionSpeedBetweenAnimations = 0.1f;
    [SerializeField] float transitionToIdle = 0.1f;

    [SerializeField] bool isPlayer;
    bool _isAnimationPlaying;
    bool isFirst;
    [SerializeField]  Vector3 startPos;
    #endregion

    #region Properties
    public bool GetIsAnimationCurrentlyActive => _isAnimationPlaying;
    #endregion

    #region Methods

    #region MonoBehaviour Callbacks   
    private void Start()
    {
        ResetAnimator();
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (IsMyTurn)
                transform.rotation = Quaternion.Lerp(transform.rotation, ToolClass.RotateToLookTowards(targetToLookAt, transform), _rotationSpeed * Time.deltaTime);
            else
                transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + Vector3.left, transform.position));
        }

        transform.position = Vector3.MoveTowards(transform.position, startPos, _positionSpeed * Time.deltaTime);
    }
    #endregion


    #region Public
    public void ResetAnimator()
    {
        isFirst = true;

                   startPos = new Vector3( transform.position.x, transform.position.y, transform.position.z);
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

    public void ResetToStartingPosition()
    {
        if (_playerAnimator == null)
        {
            Debug.LogError("Error in ResetToIdle");
            return;
        }

        transform.position = startPos;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);

        if (isPlayer == false)
        {
            _playerAnimator.SetInteger("RelicAnim", -1);
            _playerAnimator.SetInteger("AnimNum", -1);
        }
    }


    public void OnStartAnimation(AnimatorStateInfo info)
    {
  //     Debug.Log("<a>Animation Playing</a> " + _playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        _isAnimationPlaying = true;
        ResetBothRotaionAndPosition();
        _playerAnimator.SetInteger("AnimNum", -1);
    }   
    internal void OnFinishAnimation(AnimatorStateInfo stateInfo)
    {
        _isAnimationPlaying = false;
        //    transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);
        ResetBothRotaionAndPosition();
    }
    public void CharacterWon()
    {
        _isAnimationPlaying = false;
        ReturnToIdle();
        _playerAnimator.SetBool("IsWon", true);
       _playerAnimator.SetInteger("AnimNum", -2);
        transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + Vector3.left , transform.position));
    }
    public void CharacterIsDead()
    {
        _playerAnimator.SetInteger("AnimNum", -2);
        _playerAnimator.SetBool("IsDead", true);
    }


    public void SetCamera(CameraController.CameraAngleLookAt cameraAngleLookAt)
    {
        _moveCameraAngle?.Raise((int)cameraAngleLookAt);
    } 
 



    public void SetAnimationQueue(string animation)
    {
        // When we finish the planning turn we get the cards and start the animation 
        if (animation == null || animation.Length == 0)
            return;

         _animationQueue.Enqueue(animation);
            
        IsMyTurn = true;
  
        if (_animationQueue.Count ==1 && isFirst)
        {
        TranstionToNextAnimation();
       
        }
    }

    public void TranstionToNextAnimation()
    {

        if (!isPlayer)
            return;
        // we want to go back to idle if we dont have more animation to do
        // and if we have animation in the queue we want to transtion to them and tell all relevants that a transtion happend
        //     LeanTween.move(this.gameObject, startPos, 0.1f);

        transform.position = startPos;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);

        if (_animationQueue.Count == 0)
        {
            ReturnToIdle();
            ResetBothRotaionAndPosition();
            isFirst = true;
            _onFinishedAnimation?.Raise();

            return;
        }
        if (isFirst == true)
        {

        //  _playerAnimator.CrossFade(_animationQueue.Dequeue().ToString(),_transitionSpeedBetweenAnimations);
            _playerAnimator.Play(_animationQueue.Dequeue().ToString());
            isFirst = false;
        }
        else
            _playerAnimator.CrossFade(_animationQueue.Dequeue().ToString(), _transitionSpeedBetweenAnimations);



        //          ResetToStartingPosition();
        _movedToNextAnimation?.Raise();


    }
    public void ExecuteKeyword() => _onAnimationDoKeyword?.Raise();

    public void ResetModelPosition() =>
        transform.position = startPos;
     
    public void ResetModelRotaion() => transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);

    public void ResetBothRotaionAndPosition() {
        ResetModelPosition();
        ResetModelRotaion();

    }
    public bool IsCurrentlyIdle
    {
        get
        {       bool isEmptyList = _animationQueue.Count == 0;
            bool isIdle = _playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "MC Mma idle";

            if (false==isIdle && isEmptyList== false)
            {
                Debug.LogError("The Player is Not in  idle");
            }
            return isEmptyList && isIdle;
        }
    }
    public void ExecuteKeywordInCombo()
    {
        _onAnimationDoKeyword?.Raise();
    }
    #endregion

    #region Private
    private void ReturnToIdle() => _playerAnimator.CrossFade("Idle", transitionToIdle);
    #endregion

    #endregion
}
