using Cards;

namespace Characters.Stats
{
    public static class StaminaHandler
    {
        #region Fields
        private static int _stamina;
        private static int _maxStamina;

        private static int _extraStamina = 0;
        #endregion

        #region Properties
        public static int Stamina => _stamina;
        public static int MaxStamina => _maxStamina;
        #endregion


        #region Public Methods

        public static void ResetStamina()
        {
            _stamina = _maxStamina + _extraStamina;
            ResetExtraStamina();
        }

        public static bool IsEnoughStamina(Card card)
        => Stamina >= card.GetSetCard.GetStaminaCost;

        public static void ResetExtraStamina()
         => _extraStamina = 0;
        public static void ReduceStamina(Card card)
        => _stamina -= card.GetSetCard.GetStaminaCost;
        
        public static void AddStamina(int amount)
            => _stamina += amount;   
        #endregion
    }
}