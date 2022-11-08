using CardMaga.Card;
using Characters.Stats;
namespace CardMaga.Commands
{
    public class StaminaCostCommand : ICommand
    {
        private CardData _cardData;
        private StaminaHandler _staminaHandler;


        public StaminaCostCommand(CardData cardData)
        {
            _cardData = cardData;
        }
        public void Init(StaminaHandler staminaHandler)
        {
            _staminaHandler = staminaHandler;
        }
        public void Execute()
        {
            _staminaHandler.ReduceStamina(_cardData);
        }

        public void Undo()
        {
            _staminaHandler.AddStamina(_cardData.StaminaCost);
        }
    }
}