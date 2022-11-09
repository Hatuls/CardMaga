using CardMaga.Card;
using Characters.Stats;
namespace CardMaga.Commands
{
    public class StaminaCostCommand : ICommand
    {
        private BattleCardData _battleCardData;
        private StaminaHandler _staminaHandler;


        public StaminaCostCommand(BattleCardData battleCardData)
        {
            _battleCardData = battleCardData;
        }
        public void Init(StaminaHandler staminaHandler)
        {
            _staminaHandler = staminaHandler;
        }
        public void Execute()
        {
            _staminaHandler.ReduceStamina(_battleCardData);
        }

        public void Undo()
        {
            _staminaHandler.AddStamina(_battleCardData.StaminaCost);
        }
    }
}