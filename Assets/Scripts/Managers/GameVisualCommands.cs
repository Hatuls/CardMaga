using CardMaga.Battle.UI;
using CardMaga.Battle.Visual;
using CardMaga.Card;
using CardMaga.Commands;
using CardMaga.Keywords;
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
        private CardExecutionManager _cardExecutionManager;
        private KeywordManager _keywordManager;
        private VisualCharactersManager _visualCharactersManager;
        public VisualCommandHandler AnimationCommands => _animationCommands;
        public VisualCommandHandler ShieldKeywordCommands => _shieldKeywordCommands;
        public VisualKeywordCommandHandler VisualKeywordCommandHandler { get => _visualKeywordCommandHandler; }


        public GameVisualCommands(IBattleUIManager battleManager)
        {
            var dataManager = battleManager.BattleDataManager;
            _visualCharactersManager = battleManager.VisualCharactersManager;
            _keywordManager = dataManager.KeywordManager;

            _cardExecutionManager = dataManager.CardExecutionManager;
            _cardExecutionManager.OnCardDataExecute += InsertCardsCommands;
        
            _animationCommands = new VisualCommandHandler();

            _shieldKeywordCommands = new VisualCommandHandler();
            _visualKeywordCommandHandler = new VisualKeywordCommandHandler();

            const int SIZE = 5;
            ModelAnimationCommandsPool = new ObjectPool<AnimationVisualCommand>(SIZE);
            VisualKeywordCommandsPool = new ObjectPool<VisualKeywordCommand>(SIZE);

            bool isLeft = true;
            RegisterCharacter(_visualCharactersManager.GetVisualCharacter(isLeft));
            RegisterCharacter(_visualCharactersManager.GetVisualCharacter(!isLeft));

            _keywordManager.OnEndTurnKeywordEffectFinished +=ExecuteKeywords;
            _keywordManager.OnStartTurnKeywordEffectFinished += ExecuteKeywords;

            void RegisterCharacter(IVisualPlayer visualPlayer)
            {
                visualPlayer.AnimatorController.OnAnimationExecuteKeyword += ExecuteKeywords;
                visualPlayer.PlayerData.EndTurnHandler.IsFinishedVisualAnimationCommands += _animationCommands.IsEmpty;
            }
        }
        public void InsertCardsCommands(bool isLeft, CardData card)
        {
            AnimationVisualCommand visualCommand = ModelAnimationCommandsPool.Pull();
            visualCommand.Init(_visualCharactersManager.GetVisualCharacter(isLeft).AnimatorController, card.CardSO, CommandType.AfterPrevious);
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

            _keywordManager.OnEndTurnKeywordEffectFinished   -= ExecuteKeywords;
            _keywordManager.OnStartTurnKeywordEffectFinished -= ExecuteKeywords;
            bool isLeft = true;

           UnRegisterCharacter(_visualCharactersManager.GetVisualCharacter(isLeft));
           UnRegisterCharacter(_visualCharactersManager.GetVisualCharacter(!isLeft));
            _cardExecutionManager.OnCardDataExecute -= InsertCardsCommands;

            void UnRegisterCharacter(IVisualPlayer visualPlayer)
            {
                visualPlayer.AnimatorController.OnAnimationExecuteKeyword       -= ExecuteKeywords;
                visualPlayer.PlayerData.EndTurnHandler.IsFinishedVisualAnimationCommands -= _animationCommands.IsEmpty;
            }
        }
    }


}