using System;

namespace CardMaga.Battle.Execution
{
    public class GameCommands : IDisposable
    {
        private readonly GameDataCommands _gameDataCommands;

        public GameCommands(IBattleManager battleManager)
        {
            _gameDataCommands = new GameDataCommands();

        }

        public GameDataCommands GameDataCommands => _gameDataCommands;


        public void Dispose()
        {
            GameDataCommands.Dispose();
      
        }
    }


}