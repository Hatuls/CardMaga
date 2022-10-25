
using CardMaga.Battle.Visual;
using CardMaga.Card;
using CardMaga.Tools.Pools;
using Keywords;
using Managers;
using System;

namespace CardMaga.Commands
{
    public class AnimationVisualCommand : ISequenceCommand,IPoolable<AnimationVisualCommand>
    {
        public event Action OnFinishExecute;
        public event Action<AnimationVisualCommand> OnDisposed;

        private CommandType _commandType;
        private CardSO _currentCard;
        private AnimatorController _animatorController;

        public CommandType CommandType => _commandType;
        public void Init(AnimatorController animatorController, CardSO currentCard, CommandType commandType)
        {
            _animatorController = animatorController;
            _currentCard = currentCard;
            _commandType = commandType;
        }
        public void Execute()
        {
            if (_currentCard.AnimationBundle.AttackAnimation.Length != 0)
            {
                _animatorController.PlayCrossAnimationQueue(_currentCard.AnimationBundle);
                _animatorController.OnAnimationEnding += FinishAnimation;
            }
            else
                FinishAnimation();
        }
        private void FinishAnimation()
        {
            if (_currentCard.AnimationBundle.AttackAnimation.Length != 0)
                _animatorController.OnAnimationEnding -= FinishAnimation;
            Dispose();
            OnFinishExecute?.Invoke();
        }

        public void Undo()
        {

        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }
    }



   
}