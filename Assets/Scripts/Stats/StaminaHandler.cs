using Cards;

namespace Characters.Stats
{
    public static class StaminaHandler
    {
        #region Fields
        private static int _stamina;
        private static int _maxStamina=10;

        private static int _extraStamina = 0;
        #endregion

        #region Properties
        public static int Stamina => _stamina;
        public static int MaxStamina => _maxStamina;

        private static StaminaUI _staminaUI;
        public static StaminaUI StaminaUI { set => _staminaUI = value; }
        #endregion


        #region Public Methods

        public static void ResetStamina()
        {
            _stamina = _maxStamina + _extraStamina;

            _staminaUI?.SetText(_stamina, _maxStamina);
            ResetExtraStamina();
        }

        public static bool IsEnoughStamina(Card card)
        => Stamina >= card.GetSetCard.GetStaminaCost;

        public static void ResetExtraStamina()
        {
         _extraStamina = 0;

        }
        public static void ReduceStamina(Card card)
        {
         _stamina -= card.GetSetCard.GetStaminaCost;
            _staminaUI?.SetText(_stamina, _maxStamina);
        }

        public static void AddStamina(int amount)
        {
            _stamina += amount;
            _staminaUI?.SetText(_stamina, _maxStamina);
        }
        #endregion
    }
}