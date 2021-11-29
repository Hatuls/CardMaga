using System.Collections.Generic;
using Unity.Events;
using UnityEngine;
using Cards;
using Rei.Utilities;
using UnityEngine.Events;

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
    [SerializeField] Animator _animator;
    [SerializeField] Transform targetToLookAt;

    AnimationBundle _previousAnimation;
    AnimationBundle _currentAnimation;
    Queue<AnimationBundle>  _animationQueue = new Queue<AnimationBundle>();


    [SerializeField] float _rotationSpeed;
    [SerializeField] float _positionSpeed;
    public bool IsMyTurn;

    [SerializeField] float _transitionSpeedBetweenAnimations = 0.1f;
    [SerializeField] float transitionToIdle = 0.1f;

    [SerializeField] bool _isPlayer;


    bool _isAnimationPlaying;
    [SerializeField]  Vector3 startPos;
    #endregion


    #region Properties
    public bool AnimatorIsPlayer => _isPlayer;
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
        if (_isPlayer)
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
    [SerializeField]
    UnityEvent OnKickEvent;
    [SerializeField]
    UnityEvent OnPunchEvent;
    public void OnKickSound() => OnKickEvent?.Invoke();
    public void OnPunchSound() => OnPunchEvent?.Invoke();
    public void ResetAnimator()
    {

         startPos = new Vector3( transform.position.x, transform.position.y, transform.position.z);
        _isAnimationPlaying = false;
        _animator.SetBool("IsDead", false);
        _animator.SetBool("IsWon", false);
        _animationQueue.Clear();
        _currentAnimation = null;
        ReturnToIdle();
    }

    public void ResetToStartingPosition()
    {
        if (_animator == null)
        {
            Debug.LogError("Error in ResetToIdle");
            return;
        }

        transform.position = startPos;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);
    }


    public void OnStartAnimation(AnimatorStateInfo info)
    {
        
        if (_currentAnimation != null && _currentAnimation.IsCinemtaic)
            SetCamera(_isPlayer ? CameraController.CameraAngleLookAt.Enemy : CameraController.CameraAngleLookAt.Player);

    }
    internal void OnFinishAnimation(AnimatorStateInfo stateInfo)
    {
        if (_animationQueue.Count == 0 && _currentAnimation == null)
        {
            SetCamera(CameraController.CameraAngleLookAt.Both);

        }
        //if (_animationQueue.Count > 0)
        //{
        //    TranstionToNextAnimation();
        //}
    }

    public void CharacterWon()
    {
        _isAnimationPlaying = false;
      
        _animator.SetBool("IsWon", true);
       //_playerAnimator.SetInteger("AnimNum", -2);
        transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + Vector3.left , transform.position));
    }
    public void CharacterIsDead()
    {
        _animator.SetBool("IsDead", true);
  //      _animator.CrossFade("KO_Head", _transitionSpeedBetweenAnimations);
    }


    public void SetCamera(CameraController.CameraAngleLookAt cameraAngleLookAt)
    {
        _moveCameraAngle?.Raise((int)cameraAngleLookAt);
    }


    public void DeathAnimationCompleted()
    {
        Battles.BattleManager.DeathAnimationFinished(_isPlayer);
    }


    public void CheckForRegisterCards()
    {
        if (CheckIfMyTurn())
            TranstionToNextAnimation();
       //Battles.CardExecutionManager.Instance.CardFinishExecuting();
      //  _previousAnimation = null;
    }

    public bool CheckIfMyTurn()
    {  
            return (Battles.Turns.TurnHandler.IsPlayerTurn == _isPlayer);
    }
    public void PlayCrossAnimation()
    {
        var cardQueue = Battles.CardExecutionManager.CardsQueue;

        if (cardQueue == null)
            throw new System.Exception("Cannot Play animation from card\n CardExecutionManager.CardsQueue is null!!");
        else if (cardQueue.Count == 0)
            return;

        PlayCrossAnimationQueue(cardQueue.Peek().CardSO.AnimationBundle);

    }

    const string duplicateAnimationAddOnString = " 0";
    bool duplicate;
    public void PlayCrossAnimationQueue(AnimationBundle animationBundle)
    {

        _currentAnimation = animationBundle;

        if (_currentAnimation?._attackAnimation == _previousAnimation?._attackAnimation && duplicate)
        {
            PlayAnimation(string.Concat(_currentAnimation._attackAnimation, duplicateAnimationAddOnString));
            duplicate = false;
        }
        else
        {
            duplicate = true;
            PlayAnimation(_currentAnimation._attackAnimation.ToString());
        }
        
        
    }
    [SerializeField] bool _crossFadeBetweenAnimations = false;
    private void PlayAnimation(string name , bool toCrossFade = false)
    {
        Debug.Log("Play Anim " + name);
        if (_crossFadeBetweenAnimations || toCrossFade)
            _animator.CrossFade(name, _transitionSpeedBetweenAnimations);
        else
             _animator.Play(name);

    }



    public void TranstionToNextAnimation()
    {

        transform.SetPositionAndRotation(startPos, ToolClass.RotateToLookTowards(targetToLookAt, transform));
        if (Battles.Turns.TurnHandler.IsPlayerTurn != _isPlayer)
        {

            _previousAnimation = null;
        return;
        }

        var cardQueue = Battles.CardExecutionManager.CardsQueue;
        if (cardQueue.Count == 0)
        {
            OnFinishAnimation();
            return;
        }


        if (Battles.CardExecutionManager.FinishedAnimation)
            Battles.CardExecutionManager.FinishedAnimation = false;
        else
            return;


        Debug.Log($"Dequeue animations {cardQueue.Peek().CardSO.CardName} and more animations: {cardQueue.Count}");
        _previousAnimation = cardQueue.Dequeue().CardSO.AnimationBundle;

        if (cardQueue.Count != 0)
        {
            Battles.CardExecutionManager.Instance.CardFinishExecuting();
        }
        else
        {
            OnFinishAnimation();
        }

    

    }
    private void OnFinishAnimation()
    {
        Battles.CardExecutionManager.FinishedAnimation = true;
     
        //ReturnToIdle();
        ResetBothRotaionAndPosition();
        //  isFirst = true;
        //   _onFinishedAnimation?.Raise();
        //  _currentAnimation = null;

    }
 
    public void ResetLayerWeight()
    {
        _animator.SetLayerWeight(1, 0);
        _animator.SetLayerWeight(2, 0);
    }

    private void SetLayerWeight(Cards.BodyPartEnum bodyPartEnum)
    {
        ResetLayerWeight();

        switch (bodyPartEnum)
        {
            case Cards.BodyPartEnum.Head:
                break;
            case Cards.BodyPartEnum.Elbow:
            case Cards.BodyPartEnum.Hand:
                _animator.SetLayerWeight(2, 1);
                break;
            case Cards.BodyPartEnum.Knee:
            case Cards.BodyPartEnum.Leg:
                _animator.SetLayerWeight(1, 1);
                break;
            case Cards.BodyPartEnum.Joker:
                break;
            default:
                break;
        }
    }

 


    public void PlayHitOrDefenseAnimation()
    {
        _opponentController.SetCurrentAnimationBundle = _currentAnimation;

        if (Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(!_isPlayer).GetStats(Keywords.KeywordTypeEnum.Shield).Amount > 0)
            _opponentController?.PlayAnimation(_currentAnimation?._shieldAnimation.ToString(), true);
        
        else
            _opponentController?.PlayAnimation(_currentAnimation?._getHitAnimation.ToString(), true);
        
       
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
            if (_animator.GetCurrentAnimatorClipInfo(0).Length > 0)
            {
             
                isIdle=_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle_1";
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
    private void ReturnToIdle() => _animator.CrossFade("KB_Idle", transitionToIdle);
    #endregion

    #endregion
   
}
