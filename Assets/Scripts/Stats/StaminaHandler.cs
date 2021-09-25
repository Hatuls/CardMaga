using Cards;
namespace Characters.Stats
{
    public class StaminaHandler
    {
        #region StaminaUI
        private static StaminaUI _staminaUI;
        public static StaminaUI StaminaUI { set => _staminaUI = value; }
        #endregion

        private static StaminaHandler _instance;
        public static StaminaHandler Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StaminaHandler();

                return _instance;
            }
        }
        public StaminaHandler()
        {
            _playerStamina = new CharacterStamina(
                StatsHandler.GetInstance.GetCharacterStats(true).StartStamina
                );

            _opponentStamina = new CharacterStamina(
                StatsHandler.GetInstance.GetCharacterStats(false).StartStamina
                );
        }
        #region Character Stamina
        public CharacterStamina _playerStamina;
        public CharacterStamina _opponentStamina;
        public class CharacterStamina
        {
            public int Stamina { get; set; }
            public int StartStamina { get; private set; }
            public int StaminaShards { get;private set; }
            public int StaminaAddition { get; set; }


            public void StartTurn() => Stamina = StartStamina + StaminaAddition;

            public void AddStaminaAddition(int addition)
            => StaminaAddition += addition;

            public void ResetStaminaAddition() => StaminaAddition = 0;
           
            public void AddStaminaShard(int shards)
            {
                StaminaShards += shards;

                if (StaminaShards/ StartStamina >0)
                {
                    StartStamina++;
                    StaminaShards = 0;
                }
            }

            public CharacterStamina(int startAmount)
            {
                StaminaAddition = 0;
                StartStamina = startAmount;
                Stamina = startAmount;
                StaminaShards = 0;
            }
        }


        #endregion

        private CharacterStamina GetCharacterStamina(bool playersStamina)
           => playersStamina ? _playerStamina : _opponentStamina;



        #region Public Methods
        public void ResetStamina(bool isPlayer)
        {
            var charactersStamina = GetCharacterStamina(isPlayer);

            charactersStamina.StartTurn();
            charactersStamina.ResetStaminaAddition();

            if (isPlayer)
                _staminaUI?.SetText(charactersStamina.Stamina);
        }
        public bool IsEnoughStamina(bool isPlayer,Card card)
         =>  GetCharacterStamina(isPlayer).Stamina >= card.StaminaCost;
        public bool HasStamina(bool isPlayer) => GetCharacterStamina(isPlayer).Stamina > 0;
        public  void ReduceStamina(bool isPlayer ,Card card)
        {
            var character = GetCharacterStamina(isPlayer);
            character.Stamina -= card.StaminaCost;

            if(isPlayer)
            _staminaUI?.SetText(character.Stamina);
        }

        public void AddStamina(bool isPlayer, int amount)
        {
            var character = GetCharacterStamina(isPlayer);
            character.Stamina += amount;
            if(isPlayer)
            _staminaUI?.SetText(character.Stamina);
        }
        #endregion
    }
}