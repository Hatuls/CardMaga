
using Battle;
using CardMaga.Card;
using Characters.Stats;
using Keywords;
using System;
using System.Collections.Generic;

namespace CardMaga.Commands
{

    public class StaminaCommand : ICommand
    {
        private CardData _staminaCost;
        private StaminaHandler _staminaHandler;


        public StaminaCommand(CardData staminaCost)
        {
            _staminaCost = staminaCost;
        }
        public void Init(StaminaHandler staminaHandler)
        {
            _staminaHandler = staminaHandler;
        }
        public void Execute()
        {
            _staminaHandler.ReduceStamina(_staminaCost);
        }

        public void Undo()
        {
            _staminaHandler.AddStamina(_staminaCost.StaminaCost);
        }
    }
    public class CardDataCommand : ICommand
    {
        private StaminaCommand _staminaCommand;
        private KeywordCommand[] _keywordCommand;
        public CardDataCommand(CardData cardData)
        {
            Keywords.KeywordData[] keywords = cardData.CardKeywords;

            for (int i = 0; i < keywords.Length; i++)
                _keywordCommand[i] = new KeywordCommand(keywords[i]);

            _staminaCommand = new StaminaCommand(cardData);
        }

        public void Init(bool isLeft, IPlayersManager _playersManager,KeywordManager keywordManager )
        {
            var currentPlayer = _playersManager.GetCharacter(isLeft);
            _staminaCommand.Init(currentPlayer.StaminaHandler);

            for (int i = 0; i < _keywordCommand.Length; i++)
                _keywordCommand[i].InitKeywordLogic(currentPlayer, keywordManager.GetLogic(_keywordCommand[i].KeywordType), _playersManager);
        }
        public void Execute()
        {
            _staminaCommand.Execute();
            for (int i = 0; i < _keywordCommand.Length; i++)
                _keywordCommand[i].Execute();
        }

        public void Undo()
        {
            for (int i = _keywordCommand.Length - 1; i >= 0; i--)
                _keywordCommand[i].Undo();


            _staminaCommand.Undo();
        }
    }








    public interface ICommand
    {
        void Execute();
        void Undo();
    }
    public interface ISequenceCommand : ICommand
    {
        event Action OnFinishExecute;
        CommandType CommandType { get; }
    }
    public enum CommandType
    {
        Instant = 0,
        AfterPrevious = 1,
        WithPrevious = 2
    }

    public class CommandHandler<T> where T : ICommand
    {
        public virtual event Action OnCommandAdded;
        public virtual event Action OnCommandUndo;
        protected Stack<T> _commandStack = new Stack<T>();

        public virtual void ResetCommands()
        {
            _commandStack.Clear();
        }
        public virtual void AddCommand(T command)
        {
            command.Execute();
            _commandStack.Push(command);
            OnCommandAdded?.Invoke();
            //Add Limit logic here
        }
        public virtual void UndoCommand()
        {
            if (_commandStack.Count > 0)
            {
                _commandStack.Pop().Undo();
                OnCommandUndo?.Invoke();
            }
        }
        public void UndoAll()
        {
            int amount = _commandStack.Count;
            for (int i = 0; i < amount; i++)
                UndoCommand();
        }
    }

    public class KeywordVisualCommandHandler : CommandHandler<ISequenceCommand>
    {
        public override event Action OnCommandAdded;

        private List<ISequenceCommand> _visualCommands = new List<ISequenceCommand>();
        public override void AddCommand(ISequenceCommand command)
        {
            switch (command.CommandType)
            {
                case CommandType.Instant:
                    command.Execute();
                    break;
                case CommandType.WithPrevious:
                case CommandType.AfterPrevious:
                    _visualCommands.Add(command);
                    break;
                default:
                    break;
            }

            OnCommandAdded?.Invoke();
            _commandStack.Push(command);
   
        }
        public void ExecuteKeywords()
        {
            ISequenceCommand current = _visualCommands[0];
            current.Execute();

            //Execute all
            ExecuteWithPreviousCommand();

            _visualCommands.RemoveAt(0);
        }
        private void ExecuteWithPreviousCommand()
        {
            int index = 1;
            while (_visualCommands.Count > 1 && _visualCommands[index].CommandType == CommandType.WithPrevious)
            {
                _visualCommands[index].Execute();
                _visualCommands.RemoveAt(index);
            }
        }
        public override void ResetCommands()
        {
            _visualCommands.Clear();
            base.ResetCommands();
        }
    }
    public class VisualCommandHandler : CommandHandler<ISequenceCommand>
    {

        public override event Action OnCommandAdded;
        private List<ISequenceCommand> _visualCommands = new List<ISequenceCommand>();
        private bool _isExecuting;
        public bool IsExecuting => _isExecuting;
        public bool IsEmpty => !IsExecuting && _visualCommands.Count == 0;
        public override void AddCommand(ISequenceCommand command)
        {
            switch (command.CommandType)
            {
                case CommandType.Instant:
                    command.Execute();
                    break;
                case CommandType.WithPrevious:
                case CommandType.AfterPrevious:
                    _visualCommands.Add(command);
                    break;
                default:
                    break;
            }

            OnCommandAdded?.Invoke();
            _commandStack.Push(command);
            MoveNext();
        }
        public override void ResetCommands()
        {
            _visualCommands.Clear();
            base.ResetCommands();
        }
        private void MoveNext()
        {
            if (_visualCommands.Count == 0 || IsExecuting)
                return;
            _isExecuting = true;
            ISequenceCommand current = _visualCommands[0];
            current.Execute();
            current.OnFinishExecute += FinishExecution;

            //Execute all
            ExecuteWithPreviousCommand();
        }

        private void ExecuteWithPreviousCommand()
        {
            int index = 1;
            while (_visualCommands.Count > 1 && _visualCommands[index].CommandType == CommandType.WithPrevious)
            {
                _visualCommands[index].Execute();
                _visualCommands.RemoveAt(index);
            }
        }

        private void FinishExecution()
        {
            _isExecuting = false;
            ISequenceCommand current = _visualCommands[0];
            _visualCommands.RemoveAt(0);
            current.OnFinishExecute -= FinishExecution;
            MoveNext();
        }
    }
}