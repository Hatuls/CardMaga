
using Battle;
using Keywords;
using Managers;
using System;

namespace CardMaga.Commands
{
    public class KeywordCommand : ISequenceCommand
    {

        public event Action OnFinishExecute;
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
            _data = data;
            CommandType = commandType;
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
            _keywordLogic.ProcessOnTarget(_currentPlayer, _data, _playersManager);
            if (OnFinishExecute != null)
                OnFinishExecute.Invoke();
        }
        public void Undo()
        {
            _keywordLogic.UnProcessOnTarget(_currentPlayer, _data, _playersManager);
        }
    }


}