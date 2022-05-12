
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
    
        public void InitStaminaHandler()
        {
            bool isPlayer = true;
            var character = CharacterStatsManager.GetCharacterStatsHandler(isPlayer);
            _playerStamina = new CharacterStamina(
              character.GetStats(Keywords.KeywordTypeEnum.Stamina).Amount, character.GetStats(Keywords.KeywordTypeEnum.StaminaShards).Amount
                );

             character = CharacterStatsManager.GetCharacterStatsHandler(!isPlayer);
            _opponentStamina = new CharacterStamina(
              character.GetStats(Keywords.KeywordTypeEnum.Stamina).Amount, character.GetStats(Keywords.KeywordTypeEnum.StaminaShards).Amount
                );
        }

        #region Character Stamina
        private CharacterStamina _playerStamina;
        public CharacterStamina PlayerStamina => _playerStamina;
        
        private CharacterStamina _opponentStamina;
        public CharacterStamina OpponentStamina => _opponentStamina;
        public class CharacterStamina
        {
            public int Stamina { get; set; }
            public int StartStamina { get; set; }
            public int StaminaShards { get;private set; }
            public int StaminaAddition { get; set; }



            public void StartTurn()
            {
                Stamina = StartStamina + StaminaAddition;
                ResetStaminaAddition();
            }

            public void AddStaminaAddition(int addition)
            => StaminaAddition += addition;

            public void ResetStaminaAddition() => StaminaAddition = 0;
            public bool HasStamina => Stamina > 0;
        

            public CharacterStamina(int startAmount ,int stamminaShards)
            {
                StaminaAddition = 0;
                StartStamina = startAmount;
                Stamina = startAmount;
                StaminaShards = stamminaShards;
            }
        }


        #endregion

        public CharacterStamina GetCharacterStamina(bool playersStamina)
           => playersStamina ? PlayerStamina : OpponentStamina;



        #region Public Methods

        public void OnEndTurn(bool isPlayer)
        {
            var charactersStamina = GetCharacterStamina(isPlayer);

            charactersStamina.Stamina = 0;
            //  charactersStamina.ResetStaminaAddition();

            if (isPlayer)
                _staminaUI?.SetText(charactersStamina.Stamina);
        }
        public void OnStartTurn(bool isPlayer)
        {
            var charactersStamina = GetCharacterStamina(isPlayer);

            charactersStamina.StartTurn();
          //  charactersStamina.ResetStaminaAddition();

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
        
        public  void ReduceStamina(bool isPlayer ,int amount)
        {
            var character = GetCharacterStamina(isPlayer);
            character.Stamina -= amount;

            if(isPlayer)
                _staminaUI?.SetText(character.Stamina);
        }
        
        public void AddStartStamina(bool isPlayer,int Amount)
        {
            var character = GetCharacterStamina(isPlayer);
            character.AddStaminaAddition (Amount);
           // character.StartStamina += Amount;
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