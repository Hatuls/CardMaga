
using CardMaga.Battle.Visual;
using CardMaga.Card;
using Keywords;
using Managers;
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
            OnFinishExecute?.Invoke();
        }

        public void Undo()
        {

        }
    }



    public class VisualKeywordCommand : ISequenceCommand
    {
        public event Action OnFinishExecute;
        public int Amount;
        public KeywordTypeEnum KeywordType;
        private CommandType _commandType;
        private VisualStatHandler _visualStatHandler;
        public VisualKeywordCommand(CommandType commandType, KeywordTypeEnum keywordType, int amount, IPlayer player) : this(commandType, keywordType, amount, player.VisualCharacter.VisualStats) { }
    
        public VisualKeywordCommand(CommandType commandType, KeywordTypeEnum keywordType, int amount, VisualStatHandler visualStat)
        {
            _commandType = commandType;
            KeywordType = keywordType;
            Amount = amount;
            _visualStatHandler = visualStat;
        }
        public CommandType CommandType => _commandType;

        public void Execute()
        {

            _visualStatHandler.VisualStatsDictionary[KeywordType].Amount += Amount;
            OnFinishExecute?.Invoke();
        }

        public void Undo()
        {
            _visualStatHandler.VisualStatsDictionary[KeywordType].Amount -= Amount;
        }
    }
}