using Battle.Deck;
using CardMaga.Card;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardMaga.Commands
{
    public class AddNewCardToDeck : ICommand
    {
        private readonly DeckHandler _deckHandler;
        private readonly DeckEnum _toDeck;
        private readonly CardData _newCard;
        public AddNewCardToDeck(DeckEnum toDeck, CardData newCard, DeckHandler deckHandler)
        {
            _deckHandler = deckHandler;
            _toDeck = toDeck;
            _newCard = newCard;
        }
        public void Execute()
        {
            _deckHandler.AddCardToDeck(_newCard, _toDeck);
        }

        public void Undo()
        {
            _deckHandler[_toDeck].DiscardCard(_newCard);
        }
    }
    public class ResetCraftingSlotCommand : ICommand
    {
        private readonly CraftingHandler _craftingHandler;
        private List<CardTypeData> _cardTypeDatas;
        public ResetCraftingSlotCommand(CraftingHandler craftingHandler)
        {
            this._craftingHandler = craftingHandler;
        }

        public void Execute()
        {
            var datas = _craftingHandler.CardsTypeData;
            _cardTypeDatas = new List<CardTypeData>(datas.Count());
            foreach (var item in datas)
                _cardTypeDatas.Add(item);

            _craftingHandler.ResetCraftingSlots();
        }

        public void Undo()
        {
            _craftingHandler.AssignCraftingSlots(_cardTypeDatas);
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
        public IReadOnlyCollection<T> CommandStack => _commandStack;
        public virtual void ResetCommands()
        {
            _commandStack.Clear();
        }
        public virtual void AddCommand(params T[] command)
        {
            for (int i = 0; i < command.Length; i++)
                AddCommand(command[i]);
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

    public class VisualKeywordCommandHandler : CommandHandler<ISequenceCommand>
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
                    if (_visualCommands.Count == 0)
                        command.Execute();
                    else
                        _visualCommands.Add(command);
                    break;
                case CommandType.AfterPrevious:
                    _visualCommands.Add(command);
                    break;
                default:
                    break;
            }

            OnCommandAdded?.Invoke();
        }
        public void ExecuteKeywords()
        {
            if (_visualCommands.Count == 0)
                return;
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
            current.OnFinishExecute += FinishExecution;
            current.Execute();

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
            if (_visualCommands.Count == 0)
                return;
            ISequenceCommand current = _visualCommands[0];
            _visualCommands.RemoveAt(0);
            current.OnFinishExecute -= FinishExecution;
            MoveNext();
        }
    }
}