using CardMaga.Battle.UI;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Tools.Pools;
using System;

namespace CardMaga.Battle.Execution
{
    public class GameVisualCommands : IDisposable
    {

        public readonly IPoolObject<AnimationVisualCommand> ModelAnimationCommandsPool;
        public readonly IPoolObject<VisualKeywordCommand> VisualKeywordCommandsPool;

        //Handling the animation of the character
        private VisualCommandHandler _animationCommands;
        private VisualKeywordCommandHandler _visualKeywordCommandHandler;
        private VisualCommandHandler _shieldKeywordCommands;
       

        private VisualCharactersManager _visualCharactersManager;
        public VisualCommandHandler AnimationCommands => _animationCommands;
        public VisualCommandHandler ShieldKeywordCommands => _shieldKeywordCommands;
        public VisualKeywordCommandHandler VisualKeywordCommandHandler { get => _visualKeywordCommandHandler; }


        public GameVisualCommands(IBattleUIManager battleManager)
        {

            _visualCharactersManager = battleManager.VisualCharactersManager;
            bool isLeft = true;

            _visualCharactersManager.GetVisualCharacter(isLeft).AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;
            _visualCharactersManager.GetVisualCharacter(!isLeft).AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;


            const int SIZE = 5;

            ModelAnimationCommandsPool = new ObjectPool<AnimationVisualCommand>(SIZE);
            VisualKeywordCommandsPool = new ObjectPool<VisualKeywordCommand>(SIZE);

            _shieldKeywordCommands = new VisualCommandHandler();
            _animationCommands = new VisualCommandHandler();
            _visualKeywordCommandHandler = new VisualKeywordCommandHandler();
        }
        public void InsertCardsCommands(bool isLeft, BattleCardData battleCard)
        {
            AnimationVisualCommand visualCommand = ModelAnimationCommandsPool.Pull();
            visualCommand.Init(_visualCharactersManager.GetVisualCharacter(isLeft).AnimatorController, battleCard.CardSO, CommandType.AfterPrevious);
            _animationCommands.AddCommand(visualCommand);

        }





        public void ExecuteKeywords()
        {
            _visualKeywordCommandHandler.ExecuteKeywords();
        }

        public void Dispose()
        {
            VisualKeywordCommandHandler.ResetCommands();
            AnimationCommands.ResetCommands();

            bool isLeft = true;

            _visualCharactersManager.GetVisualCharacter(isLeft).AnimatorController.OnAnimationExecuteKeyword -= ExecuteKeywords;
            _visualCharactersManager.GetVisualCharacter(!isLeft).AnimatorController.OnAnimationExecuteKeyword -= ExecuteKeywords;
        }
    }


}