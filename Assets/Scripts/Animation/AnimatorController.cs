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

    AnimationBundle _previousAnimation;
    AnimationBundle _currentAnimation;
    Queue<AnimationBundle>  _animationQueue = new Queue<AnimationBundle>();


    [SerializeField] float _rotationSpeed;
    [SerializeField] float _positionSpeed;
    public bool IsMyTurn;

    [SerializeField] float _transitionSpeedBetweenAnimations = 0.1f;
    [SerializeField] float transitionToIdle = 0.1f;

    [SerializeField] bool isPlayer;
    bool _isAnimationPlaying;
    [SerializeField]  Vector3 startPos;
    #endregion

    #region Properties
    public bool GetIsAnimationCurrentlyActive => _isAnimationPlaying;
    public AnimationBundle SetCurrentAnimationBundle { set => _currentAnimation = value; }
    #endregion

    #region Methods

    #region MonoBehaviour Callbacks   

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

         startPos = new Vector3( transform.position.x, transform.position.y, transform.position.z);
        _isAnimationPlaying = false;
        _playerAnimator.SetBool("IsDead", false);
        _playerAnimator.SetBool("IsWon", false);
        _animationQueue.Clear();
        _currentAnimation = null;
        ReturnToIdle();
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
    }


    public void OnStartAnimation(AnimatorStateInfo info)
    {
        
        if (_currentAnimation != null && _currentAnimation.IsCinemtaic)
            SetCamera(isPlayer ? CameraController.CameraAngleLookAt.Enemy : CameraController.CameraAngleLookAt.Player);

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


    public void DeathAnimationCompleted()
    {
        Battles.BattleManager.ReturnToMainMenu();
    }


    public void CheckForRegisterCards()
    {
        if (CheckIfMyTurn())
       Battles.CardExecutionManager.Instance.CardFinishExecuting();
        _previousAnimation = null;
    }

    public bool CheckIfMyTurn()
    {  
            return (Battles.Turns.TurnHandler.IsPlayerTurn == isPlayer);
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


   
    public void PlayCrossAnimationQueue(AnimationBundle animationBundle)
    {

        _currentAnimation = animationBundle;

        if (_currentAnimation?._attackAnimation == _previousAnimation?._attackAnimation)
            _playerAnimator.SetTrigger("Duplicate");
        else
        {
            var cardQueue = Battles.CardExecutionManager.CardsQueue;
            string output = "";
            foreach (var card in cardQueue)
            {
                output += " " + card.CardSO.AnimationBundle._attackAnimation.ToString();
            }
            Debug.LogError(output);

            PlayAnimation(_currentAnimation._attackAnimation.ToString());
        }
        
    }
    [SerializeField] bool _crossFadeBetweenAnimations = false;
    private void PlayAnimation(string name)
    {
        if (_crossFadeBetweenAnimations)
            _playerAnimator.CrossFade(name, _transitionSpeedBetweenAnimations);
        else
             _playerAnimator.Play(name);

    }



    public void TranstionToNextAnimation()
    {
        transform.position = startPos;
        transform.rotation = ToolClass.RotateToLookTowards(targetToLookAt, transform);
        var cardQueue = Battles.CardExecutionManager.CardsQueue;
        if (cardQueue.Count == 0)
        {
            OnFinishAnimation();
            return;
        }

        _previousAnimation = cardQueue.Dequeue().CardSO.AnimationBundle;

        if (cardQueue.Count != 0)
        {
            Battles.CardExecutionManager.Instance.CardFinishExecuting();
        }

    

    }
    private void OnFinishAnimation()
    {
            _previousAnimation = null;
        //ReturnToIdle();
        ResetBothRotaionAndPosition();
        //  isFirst = true;
        //   _onFinishedAnimation?.Raise();
        //  _currentAnimation = null;

    }
 
    public void ResetLayerWeight()
    {
        _playerAnimator.SetLayerWeight(1, 0);
        _playerAnimator.SetLayerWeight(2, 0);
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
                _playerAnimator.SetLayerWeight(2, 1);
                break;
            case Cards.BodyPartEnum.Knee:
            case Cards.BodyPartEnum.Leg:
                _playerAnimator.SetLayerWeight(1, 1);
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

        if (Characters.Stats.CharacterStatsManager.GetCharacterStatsHandler(!isPlayer).GetStats(Keywords.KeywordTypeEnum.Shield).Amount > 0)
            _opponentController?.PlayAnimation(_currentAnimation?._shieldAnimation.ToString());
        
        else
            _opponentController?.PlayAnimation(_currentAnimation?._getHitAnimation.ToString());
        
       
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
