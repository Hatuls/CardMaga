
using Battle;
using CardMaga.Animation;
using CardMaga.Card;
using Managers;
using Rei.Utilities;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using Unity.Events;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    #region Events
    public event Action OnAnimationExecuteKeyword;
    public static event Action<bool> OnDeathAnimationFinished;
    public  event Action OnAnimationEnding;
    public static event Action<TransitionCamera, Action> OnAnimationStart;

    [SerializeField] VoidEvent _movedToNextAnimation;

    [SerializeField] VoidEvent _onFinishedAnimation;

    [SerializeField] IntEvent _moveCameraAngle;
    #endregion

    #region Fields

    [SerializeField] AnimatorController _opponentController;

    [SerializeField] Transform targetToLookAt;

    AnimationBundle _previousAnimation;
    AnimationBundle _currentAnimation;
    Queue<AnimationBundle> _animationQueue = new Queue<AnimationBundle>();


    [SerializeField] float _rotationSpeed;
    [SerializeField] float _positionSpeed;
    public bool IsMyTurn;

    [SerializeField] float _transitionSpeedBetweenAnimations = 0.1f;
    [SerializeField] float transitionToIdle = 0.1f;

    [SerializeField] bool _crossFadeBetweenAnimations = false;
    [SerializeField] Vector3 startPos;
    private bool _toLockAtPlace;
    private bool _isAnimationPlaying;
    private Animator _animator;
    private IDisposable _animationToken;
    private IPlayer _player;
    #endregion


    #region Properties
    public Animator Animator => _animator;
    public bool IsCurrentlyIdle
    {
        get
        {
           
            bool result = false;
            if (Animator.GetCurrentAnimatorClipInfo(0).Length > 0)
                result = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle_2";

            return result;

        }
    }
    public bool IsLeft => _player.IsLeft;
    public bool IsAnimationCurrentlyActive => _isAnimationPlaying;
    public AnimationBundle SetCurrentAnimationBundle { set => _currentAnimation = value; }
    #endregion

    #region Methods

    #region MonoBehaviour Callbacks   



    public void Update()
    {
        RotateModel();

        if (!_toLockAtPlace)
            transform.position = Vector3.MoveTowards(transform.position, startPos, _positionSpeed * Time.deltaTime);
    }

    
    #endregion


    #region Public
    public void Init(VisualCharacter vc, IPlayer player)
    {
        _animator = vc.Animator;
        _player = player;
        ResetAnimator();

    }
    public void ResetAnimator()
    {

        startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        _isAnimationPlaying = false;
        Animator.SetBool("IsDead", false);
        Animator.SetBool("IsWon", false);
        _animationQueue.Clear();
        _currentAnimation = null;
        ReturnToIdle();
        _toLockAtPlace = false;
    }




    public void ResetToStartingPosition()
    {
        if (Animator == null)
        {
            Debug.LogError("Error in ResetToIdle");
            return;
        }

        transform.position = startPos;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);
    }


    public void StartAnimation(AnimatorStateInfo info)
    {

        if (_currentAnimation != null && _currentAnimation.CameraDetails != null && _currentAnimation.CameraDetails.CheckCameraDetails(IsLeft))
        {
            // move to camera!
            TransitionCamera transitionCamera = _currentAnimation.CameraDetails.GetTransitionCamera(IsLeft);
            OnAnimationStart?.Invoke(transitionCamera, null);
        }

    }

    internal void FinishAnimation(AnimatorStateInfo stateInfo)
    {
        //if (_animationQueue.Count == 0 && _currentAnimation == null)
        //{

       
     //   ReleaseToken();
        //     }

        //if (_animationQueue.Count > 0)
        //{
        //    TranstionToNextAnimation();
        //}
    }

    public void CharacterWon()
    {
        _isAnimationPlaying = false;
        ReleaseToken();
        Animator.SetBool("IsWon", true);
        //_playerAnimator.SetInteger("AnimNum", -2);
        //    transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + Vector3.left, transform.position));
    }
    public void CharacterIsDead()
    {
        ReleaseToken();
        Animator.SetBool("IsDead", true);
        //      _animator.CrossFade("KO_Head", _transitionSpeedBetweenAnimations);
        _toLockAtPlace = true;

    }


    public void DeathAnimationCompleted()
    {
        OnDeathAnimationFinished?.Invoke(IsLeft);
    }


    public void CheckForRegisterCards()
    {
        //  if (CheckIfMyTurn())
        TranstionToNextAnimation();

    }

    //public bool CheckIfMyTurn()
    //{
    //    return (TurnHandler.IsPlayerTurn == _isPlayer);
    //}
    //public void PlayCrossAnimation()
    //{
    //    var cardQueue = CardExecutionManager.CardsQueue;

    //    if (cardQueue == null)
    //        throw new System.Exception("Cannot Play animation from card\n CardExecutionManager.CardsQueue is null!!");
    //    else if (cardQueue.Count == 0)
    //        return;

    //    PlayCrossAnimationQueue(cardQueue.Peek().CardSO.AnimationBundle);

    //}

    const string duplicateAnimationAddOnString = " 0";
    bool duplicate;
    public void PlayCrossAnimationQueue(AnimationBundle animationBundle)
    {
        _currentAnimation = animationBundle;

        if (_currentAnimation?.AttackAnimation == _previousAnimation?.AttackAnimation && duplicate)
        {
            PlayAnimation(string.Concat(_currentAnimation.AttackAnimation, duplicateAnimationAddOnString));
            duplicate = false;
        }
        else
        {
            duplicate = true;
            PlayAnimation(_currentAnimation.AttackAnimation.ToString());
        }

        _isAnimationPlaying = true;

    }


    private void PlayAnimation(string name, bool toCrossFade = false)
    {
//        Debug.Log("Play Anim " + name);
        if (_crossFadeBetweenAnimations || toCrossFade)
            Animator.CrossFade(name, _transitionSpeedBetweenAnimations);
        else
            Animator.Play(name);

    }



    public void TranstionToNextAnimation()
    {

        transform.SetPositionAndRotation(startPos, ToolClass.RotateToLookTowards(targetToLookAt, transform));


        OnFinishAnimation();



        //if (CardExecutionManager.FinishedAnimation)
        //    CardExecutionManager.FinishedAnimation = false;
        //else
        //    return;


        //Debug.Log($"Dequeue animations {cardQueue.Peek().CardSO.CardName} and more animations: {cardQueue.Count}");
        //_previousAnimation = cardQueue.Dequeue().CardSO.AnimationBundle;

        //if (cardQueue.Count != 0)
        //{
        //    CardExecutionManager.Instance.CardFinishExecuting();
        //}
        //else
        //{
        //    OnFinishAnimation();
        //}



    }

    public void ResetLayerWeight()
    {
        Animator.SetLayerWeight(1, 0);
        Animator.SetLayerWeight(2, 0);
    }



    public void PlayHitOrDefenseAnimation()
    {
        _opponentController.SetCurrentAnimationBundle = _currentAnimation;


        if (CardExecutionManager.Instance.CanDefendIncomingAttack(!IsLeft))
            _opponentController?.PlayAnimation(_currentAnimation?.ShieldAnimation.ToString(), true);
        else
            _opponentController?.PlayAnimation(_currentAnimation?.GetHitAnimation.ToString(), true);
    }

    public void ExecuteKeyword() => OnAnimationExecuteKeyword?.Invoke();

    public void ResetModelPosition() =>
        transform.position = startPos;

    public void ResetModelRotaion() => transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);

    public void ResetBothRotaionAndPosition()
    {
        ResetModelPosition();
        ResetModelRotaion();

    }



    #endregion

    #region Private
    private void ReleaseToken() => _animationToken?.Dispose();
    private void OnFinishAnimation()
    {
        ReleaseToken();
        ResetBothRotaionAndPosition();
        OnAnimationEnding?.Invoke();
        _isAnimationPlaying = false;
        //  isFirst = true;
        //   _onFinishedAnimation?.Raise();
        //  _currentAnimation = null;

    }
    private void RotateModel()
    {
        if (IsMyTurn)
            transform.rotation = Quaternion.Lerp(transform.rotation, ToolClass.RotateToLookTowards(targetToLookAt, transform), _rotationSpeed * Time.deltaTime);
        else
            transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + (IsLeft ? Vector3.left : Vector3.right), transform.position));
    }
    private void ReturnToIdle() => Animator.CrossFade("Idle_1", transitionToIdle);


    public void PlayAnimation(CardData cardData, ITokenReciever tokenMachine)
    {
        _animationToken = tokenMachine.GetToken();
    
        PlayCrossAnimationQueue(cardData.CardSO.AnimationBundle);
    }
    #endregion

    #endregion

}
