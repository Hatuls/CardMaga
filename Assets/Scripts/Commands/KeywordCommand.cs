
using Battle;
using CardMaga.Battle.Visual;
using CardMaga.Tools.Pools;
using Keywords;
using Managers;
using System;

namespace CardMaga.Commands
{
    public class KeywordCommand : ICommand
    {
        public static event Action<KeywordCommand> OnKeywordCommandActivated;
        private bool _currentPlayer;
        private IPlayersManager _playersManager;
        private IKeyword _keywordLogic;
        private KeywordData _data;
        private CommandType _commandType;

        public KeywordData KeywordData => _data;
        public KeywordTypeEnum KeywordType => _data.KeywordSO.GetKeywordType;
        public CommandType CommandType { get => _commandType; private set => _commandType = value; }

        public KeywordCommand(KeywordData data, CommandType commandType)
        {
            CommandType = commandType;
            _data = data;
        }
        public void InitKeywordLogic(IPlayer current, IKeyword keyword, IPlayersManager playersManager)
        {
            if (_playersManager == null)
            {
                _playersManager = playersManager;
                _keywordLogic = keyword;
                _currentPlayer = current.IsLeft;
            }
        }

        public void Execute()
        {
            OnKeywordCommandActivated?.Invoke(this);
            // Open Listener for visual keyword (KeywordData.GetTarget);
            _keywordLogic.ProcessOnTarget(_currentPlayer, _data, _playersManager);
            // Stop Listen
        }
        public void Undo()
        {
            OnKeywordCommandActivated?.Invoke(this);
            _keywordLogic.UnProcessOnTarget(_currentPlayer, _data, _playersManager);
        }
    }
    public class VisualKeywordCommand : ISequenceCommand, IPoolable<VisualKeywordCommand>
    {

        public event Action<VisualKeywordCommand> OnDisposed;
        public event Action OnFinishExecute;

        public int Amount;
        public KeywordTypeEnum KeywordType;
        private VisualStatHandler _visualStatHandler;
        private CommandType _commandType;

        public CommandType CommandType { get => _commandType; set => _commandType = value; }

        public void Init(KeywordTypeEnum keywordType, int amount, VisualStatHandler visualStat)
        {
            _visualStatHandler = visualStat;
            KeywordType = keywordType;
            Amount = amount;
        }

        public void Execute()
        {
            _visualStatHandler.VisualStatsDictionary[KeywordType].Amount = Amount;
            Dispose();
        }
        public void Undo()
        {
            _visualStatHandler.VisualStatsDictionary[KeywordType].Amount = Amount;
            Dispose();
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }
    }

}