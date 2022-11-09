using System;

namespace CardMaga.Battle.Execution
{
    public class GameCommands : IDisposable
    {
        private readonly GameDataCommands _gameDataCommands;
        private readonly GameVisualCommands _gameVisualCommands;

        public GameCommands(IBattleManager battleManager)
        {
            _gameDataCommands = new GameDataCommands(battleManager.PlayersManager, battleManager.KeywordManager);
            _gameVisualCommands = new GameVisualCommands(battleManager.BattleUIManager);
        }

        public GameDataCommands GameDataCommands => _gameDataCommands;
        public GameVisualCommands GameVisualCommands => _gameVisualCommands;

        public void Dispose()
        {
            GameDataCommands.Dispose();
            GameVisualCommands.Dispose();
        }
    }


}