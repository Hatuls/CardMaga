
using Battle;
using Keywords;
using Managers;

namespace CardMaga.Commands
{
    public class KeywordCommand : ICommand
    {

        private bool _currentPlayer;
        private IPlayersManager _playersManager;
        private IKeyword _keywordLogic;
        private KeywordData _data;
        public KeywordTypeEnum KeywordType => _data.KeywordSO.GetKeywordType;
        public KeywordCommand(KeywordData data)
        {
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
            _keywordLogic.ProcessOnTarget(_currentPlayer, _data, _playersManager);
        }
        public void Undo()
        {
            _keywordLogic.UnProcessOnTarget(_currentPlayer, _data, _playersManager);
        }
    }


    public class VisualKeywordCommand
    {
        KeywordData _keywordData;

    }
}