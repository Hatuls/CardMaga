using CardMaga.Animation;
using CardMaga.Battle.Execution;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Keywords;
using Rei.Utilities;
using ReiTools.TokenMachine;
using System;
using Unity.Events;
using UnityEngine;
namespace CardMaga.Battle.Visual
{
    public class AnimatorController : MonoBehaviour
    {
        #region Events
        public event Action OnAnimationExecuteKeyword;
        public static event Action<bool> OnDeathAnimationFinished;
        public event Action OnAnimationEnding;
        public static event Action<TransitionCamera, Action> OnAnimationStart;

        [SerializeField] VoidEvent _movedToNextAnimation;

        [SerializeField] VoidEvent _onFinishedAnimation;

        [SerializeField] IntEvent _moveCameraAngle;
        #endregion

        #region Fields
        private const string duplicateAnimationAddOnString = " 0";
        private bool duplicate;
        [SerializeField] AnimatorController _opponentController;

        [SerializeField] Transform targetToLookAt;

        AnimationBundle _previousAnimation;
        AnimationBundle _currentAnimation;

        [SerializeField] float _rotationSpeed;
        [SerializeField] float _positionSpeed;

        public bool IsMyTurn;

        [SerializeField] float _transitionSpeedBetweenAnimations = 0.1f;
        [SerializeField] float transitionToIdle = 0.1f;

        [SerializeField] bool _crossFadeBetweenAnimations = false;
        [SerializeField] Vector3 startPos;
        private bool _toLockAtPlace;
        private bool _isAnimationPlaying;
        private bool _isLeft;
        private Animator _animator;
        private IDisposable _animationToken;
        private EndBattleHandler _endBattleHandler;
        private GameVisualCommands _visualCommandHandler;

        #endregion


        #region Properties
        public Animator Animator => _animator;
        //public bool IsCurrentlyIdle
        //{
        //    get
        //    {
        //        bool result = false;
        //        if (Animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        //            result = Animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle_2";

        //        return result;
        //    }
        //}
        public bool IsLeft => _isLeft;
   
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
        public void Init(IVisualPlayer visualPlayer, EndBattleHandler endBattleHandler, GameVisualCommands gameVisualCommands)
        {
            _visualCommandHandler = gameVisualCommands;
            _animator = visualPlayer.Animator;
            ResetAnimator();
            _isLeft = visualPlayer.PlayerData.IsLeft;
            _endBattleHandler = endBattleHandler;
            visualPlayer.PlayerData.EndTurnHandler.IsCharacterPlayingIdleAnimation += IsAnimationFinished;

            if (IsLeft)
            {
                _endBattleHandler.OnLeftPlayerWon += CharacterWon;
                _endBattleHandler.OnRightPlayerWon += CharacterIsDead;
            }else
            {
                _endBattleHandler.OnRightPlayerWon += CharacterWon;
                _endBattleHandler.OnLeftPlayerWon += CharacterIsDead;
            }

            _endBattleHandler.OnBattleFinished += ResetLayerWeight;

        }

        public void BeforeDestroy(IVisualPlayer visualPlayer)
        {
            visualPlayer.PlayerData.EndTurnHandler.IsCharacterPlayingIdleAnimation -= IsAnimationFinished;
            _endBattleHandler.OnBattleFinished -= ResetLayerWeight;

            if (IsLeft)
            {
                _endBattleHandler.OnLeftPlayerWon -= CharacterWon;
                _endBattleHandler.OnRightPlayerWon -= CharacterIsDead;
            }
            else
            {
                _endBattleHandler.OnRightPlayerWon-= CharacterWon;
                _endBattleHandler.OnLeftPlayerWon -= CharacterIsDead;
            }
        }



        public void ResetAnimator()
        {
            startPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            _isAnimationPlaying = false;
            Animator.SetBool("IsDead", false);
            Animator.SetBool("IsWon", false);
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

        }

        public void CharacterWon()
        {
            _isAnimationPlaying = false;
            ReleaseToken();
            Animator.SetBool("IsWon", true);

        }
        public void CharacterIsDead()
        {
            ReleaseToken();
            Animator.SetBool("IsDead", true);
            _toLockAtPlace = true;
        }

        public bool IsAnimationFinished() => !_isAnimationPlaying;
        public void DeathAnimationCompleted()
        {
            OnDeathAnimationFinished?.Invoke(IsLeft);
        }


        public void CheckForRegisterCards()
        {
            //  if (CheckIfMyTurn())
            TranstionToNextAnimation();

        }

        public void PlayCrossAnimationQueue(AnimationBundle animationBundle)
        {
            _currentAnimation = animationBundle;

            if (_currentAnimation?.AttackAnimation == _previousAnimation?.AttackAnimation && duplicate)
            {
                PlayAnimation(string.Concat(_currentAnimation.AttackAnimation, duplicateAnimationAddOnString));
                //   _animator.SetTrigger("Duplicate");
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
            _previousAnimation = _currentAnimation;
            OnFinishAnimation();
        }

        public void ResetLayerWeight()
        {
            Animator.SetLayerWeight(1, 0);
            Animator.SetLayerWeight(2, 0);
        }



        public void PlayHitOrDefenseAnimation()
        {
            _opponentController.SetCurrentAnimationBundle = _currentAnimation;

            if (CanDefendIncomingAttack())
                _opponentController?.PlayAnimation(_currentAnimation?.ShieldAnimation.ToString(), true);
            else
                _opponentController?.PlayAnimation(_currentAnimation?.GetHitAnimation.ToString(), true);
        }
        public bool CanDefendIncomingAttack()
        {
            System.Collections.Generic.IReadOnlyCollection<ISequenceCommand> commandStack = _visualCommandHandler.VisualKeywordCommandHandler.CommandStack;
            foreach (var item in commandStack)
            {
                switch (item)
                {
                    case VisualKeywordCommand cmd:
                        if (cmd.KeywordType == KeywordType.Shield)
                            return cmd.Amount > 0;
                        break;
                    case VisualKeywordsPackCommands pack:
                        foreach (var visualkeword in pack.VisualKeywordCommands)
                        {
                            if (visualkeword.KeywordType == KeywordType.Shield)
                                return visualkeword.Amount > 0;
                        }

                        break;
                    default:
                        break;
                }
            }
            return false;
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
            ResetBothRotaionAndPosition();
            OnAnimationEnding?.Invoke();
            _isAnimationPlaying = false;
            ReleaseToken();

        }
        private void RotateModel()
        {
            if (IsMyTurn)
                transform.rotation = Quaternion.Lerp(transform.rotation, ToolClass.RotateToLookTowards(targetToLookAt, transform), _rotationSpeed * Time.deltaTime);
            else
                transform.rotation = Quaternion.LookRotation(ToolClass.GetDirection(transform.position + (IsLeft ? Vector3.left : Vector3.right), transform.position));
        }
        private void ReturnToIdle() => Animator.CrossFade("Idle_1", transitionToIdle);


        public void PlayAnimation(BattleCardData battleCardData, ITokenReciever tokenMachine)
        {
            _animationToken = tokenMachine.GetToken();

            PlayCrossAnimationQueue(battleCardData.CardSO.AnimationBundle);
        }

        #endregion

        #endregion

    }
}