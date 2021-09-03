using System.Collections.Generic;
using Unity.Events;
using UnityEngine;
using Cards;
using Rei.Utilities;
public class AnimatorController : MonoBehaviour
{
    #region Events
   
    [SerializeField] VoidEvent _movedToNextAnimation; 
 
    [SerializeField] VoidEvent _onFinishedAnimation; 

    [SerializeField] VoidEvent _onAnimationDoKeyword;
    [SerializeField] IntEvent _moveCameraAngle;
    #endregion

    #region Fields
    [SerializeField] AnimatorController _opponentController;
    [SerializeField] Animator _playerAnimator;
    [SerializeField] Transform targetToLookAt;

    AnimationBundle _currentAnimation;
    Queue<AnimationBundle>  _animationQueue = new Queue<AnimationBundle>();

    [SerializeField] float _rotationSpeed;
    [SerializeField] float _positionSpeed;
    public bool IsMyTurn;

    [SerializeField] float _transitionSpeedBetweenAnimations = 0.1f;
    [SerializeField] float transitionToIdle = 0.1f;

    [SerializeField] bool isPlayer;
    bool _isAnimationPlaying;
    [SerializeField]   bool isFirst;
    [SerializeField]  Vector3 startPos;
    #endregion

    #region Properties
    public bool GetIsAnimationCurrentlyActive => _isAnimationPlaying;
    public AnimationBundle SetCurrentAnimationBundle { set => _currentAnimation = value; }
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
    public void PlayAnimation (Cards.AttackAnimation cardName)
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
        
        if (_currentAnimation != null && _currentAnimation.IsCinemtaic)
            SetCamera(isPlayer ? CameraController.CameraAngleLookAt.Enemy : CameraController.CameraAngleLookAt.Player);

    }
    internal void OnFinishAnimation(AnimatorStateInfo stateInfo)
    {
        if (_animationQueue.Count == 0 && _currentAnimation == null)
            SetCamera(CameraController.CameraAngleLookAt.Both);
    }

    public void CharacterWon()
    {
        _isAnimationPlaying = false;
      
        _playerAnimator.SetBool("IsWon", true);
       //_playerAnimator.SetInteger("AnimNum", -2);
        transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + Vector3.left , transform.position));
    }
    public void CharacterIsDead()
    {
        _playerAnimator.CrossFade("KO_Head", _transitionSpeedBetweenAnimations);
    }


    public void SetCamera(CameraController.CameraAngleLookAt cameraAngleLookAt)
    {
        _moveCameraAngle?.Raise((int)cameraAngleLookAt);
    } 
 



    public void SetAnimationQueue(Card card)
    {
        // When we finish the planning turn we get the cards and start the animation 
        if (card == null )
            return;

         SetAnimationQueue(card.CardSO.GetAnimationBundle);
       
    }
    public void SetAnimationQueue(AnimationBundle animationBundle )
    {
        if (animationBundle == null)
            return;

        _animationQueue.Enqueue(animationBundle);

        IsMyTurn = true;

        if (_animationQueue.Count == 1) //&& isFirst
        {
            TranstionToNextAnimation();
        }
    }
    public void TranstionToNextAnimation()
    {

    
        // we want to go back to idle if we dont have more animation to do
        // and if we have animation in the queue we want to transtion to them and tell all relevants that a transtion happend
        //     LeanTween.move(this.gameObject, startPos, 0.1f);

        transform.position = startPos;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);

        if (_animationQueue.Count == 0)
        {
            OnFinishAnimation();

            return;
        }

        _currentAnimation = _animationQueue.Dequeue();
        PlayAnimation(_currentAnimation._attackAnimation.ToString());

    
        _movedToNextAnimation?.Raise();
    }

    private void PlayAnimation(string Name)
    {
        if (isFirst == true)
        {

             _playerAnimator.CrossFade(Name, _transitionSpeedBetweenAnimations);
          //  _playerAnimator.Play(Name);
            isFirst = false;
        }
        else
            _playerAnimator.CrossFade(Name, _transitionSpeedBetweenAnimations);
    }


    public void PlayHitOrDefenseAnimation()
    {
        _opponentController.SetCurrentAnimationBundle = _currentAnimation;

        if (Characters.Stats.StatsHandler.GetInstance.GetCharacterStats(!isPlayer).Shield > 0)
            _opponentController?.PlayAnimation(_currentAnimation?._shieldAnimation.ToString() );
        
        else
            _opponentController?.PlayAnimation(_currentAnimation?._getHitAnimation.ToString());
        
       
    }
    private void OnFinishAnimation()
    {
        ReturnToIdle();
        ResetBothRotaionAndPosition();
        isFirst = true;
        _onFinishedAnimation?.Raise();
        _currentAnimation = null;

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
        {    
            bool isEmptyList = _animationQueue.Count == 0;
            bool isIdle = true ;
            if (_playerAnimator.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
             
                isIdle=_playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle_1";
            }
            

            if (false==isIdle && isEmptyList== false)
            {
                Debug.Log("The Player is Not in  idle");
            }
            return isEmptyList && isIdle;
        }
    }
 
    #endregion

    #region Private
    private void ReturnToIdle() => _playerAnimator.CrossFade("KB_Idle", transitionToIdle);
    #endregion

    #endregion
   
}
