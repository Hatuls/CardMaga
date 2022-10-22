
using CardMaga.Card;
using System;

namespace CardMaga.Commands
{
    public class AnimationVisualCommand : ISequenceCommand
    {
        public event Action OnFinishExecute;
        private CommandType _commandType;
        private CardSO _currentCard;
        private AnimatorController _animatorController;

        public CommandType CommandType => _commandType;

        public AnimationVisualCommand(CardSO currentCard, CommandType commandType)
        {
            _currentCard = currentCard;
            _commandType = commandType;
        }
        public void Init(AnimatorController animatorController)
        {
            _animatorController = animatorController;

        }
        public void Execute()
        {
            _animatorController.PlayCrossAnimationQueue(_currentCard.AnimationBundle);
            _animatorController.OnAnimationEnding += FinishAnimation;
        }
        private void FinishAnimation()
        {
            _animatorController.OnAnimationEnding -= FinishAnimation;
            OnFinishExecute?.Invoke();
        }

        public void Undo()
        {

        }
    }


 

}