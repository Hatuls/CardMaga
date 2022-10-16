
using CardMaga.Card;
using ReiTools.TokenMachine;
using System;

namespace Characters.Stats
{
    public class StaminaHandler
    {
        public event Action OnStaminaDepleted;
        public event Action<int> OnStaminaValueChanged;
        public event Action<int> OnStaminaShardValueChanged;


        private bool _isFlag;
        private int _stamina;
        private int _startStamina;
        private int _staminaShards;
        private int _staminaAddition;

        public bool HasStamina => Stamina > 0;
        public int StartStamina { get => _startStamina; private set => _startStamina = value; }
        public int StaminaAddition { get => _staminaAddition; private set => _staminaAddition = value; }
        public int Stamina
        {
            get => _stamina;
            set
            {
                _stamina = value;
                OnStaminaValueChanged?.Invoke(_stamina);

            }
        }
        public int StaminaShards
        {
            get => _staminaShards;
            private set
            {
                _staminaShards = value;
                OnStaminaShardValueChanged?.Invoke(_staminaShards);
            }
        }

        public StaminaHandler(int startAmount, int stamminaShards, int staminaAddition = 0)
        {
            StaminaAddition = staminaAddition;
            StartStamina = startAmount;
            StaminaShards = stamminaShards;
            Stamina = StartStamina;
        }


        public void StartTurn(ITokenReciever tokenMachine)
        {
            _isFlag = true;
            IDisposable t = tokenMachine.GetToken();
            Stamina = StartStamina + StaminaAddition;
            ResetStaminaAddition();
            t.Dispose();
        }
        public void EndTurn(ITokenReciever tokenMachine)
        {
            _stamina = 0;
            OnStaminaValueChanged?.Invoke(Stamina);
        }
        public void AddStaminaAddition(int addition) => StaminaAddition += addition;
        public void AddStartStamina(int startStamina) => StartStamina += startStamina;
        public void ResetStamina() => Stamina = 0;
        public void ResetStaminaAddition() => StaminaAddition = 0;
        public bool CanPlayCard(CardData card) => card.StaminaCost <= Stamina;
        public void ReduceStamina(CardData card) => Stamina -= card.StaminaCost;
   
        public void CheckStaminaEmpty()
        {
            if (Stamina <= 0&& _isFlag)
            {
                OnStaminaDepleted?.Invoke();
                _isFlag = false;
            }
        }

        public void AddStamina(int amount)
        {
           
            Stamina += amount;
            //if (isPlayer)
            //        _staminaTextManager?.UpdateCurrentStamina(character.Stamina);
        }

    }





    //public class StaminaHandler
    //{
    //    #region StaminaTextManager
    //    private static StaminaTextManager _staminaTextManager;
    //    public static StaminaTextManager StaminaTextManager { set => _staminaTextManager = value; }
    //    #endregion

    //    private static StaminaHandler _instance;
    //    public static StaminaHandler Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //                _instance = new StaminaHandler();


    //            return _instance;
    //        }
    //    }

    //    private CharacterStamina _playerStamina;
    //    public CharacterStamina PlayerStamina => _playerStamina;

    //    private CharacterStamina _opponentStamina;
    //    public CharacterStamina OpponentStamina => _opponentStamina;
    //    public void InitStaminaHandler()
    //    {
    //        bool isPlayer = true;
    //        var character = CharacterStatsManager.GetCharacterStatsHandler(isPlayer);
    //        _playerStamina = new CharacterStamina(
    //          character.GetStats(Keywords.KeywordTypeEnum.Stamina).Amount, character.GetStats(Keywords.KeywordTypeEnum.StaminaShards).Amount
    //            );

    //        character = CharacterStatsManager.GetCharacterStatsHandler(!isPlayer);
    //        _opponentStamina = new CharacterStamina(
    //          character.GetStats(Keywords.KeywordTypeEnum.Stamina).Amount, character.GetStats(Keywords.KeywordTypeEnum.StaminaShards).Amount
    //            );
    //    }

    //    #region Character Stamina



    //    #endregion

    //    public CharacterStamina GetCharacterStamina(bool playersStamina)
    //       => playersStamina ? PlayerStamina : OpponentStamina;



    //    #region Public Methods

    //    public void OnEndTurn(bool isPlayer)
    //    {
    //        var charactersStamina = GetCharacterStamina(isPlayer);

    //        charactersStamina.ResetStamina();
    //        //  charactersStamina.ResetStaminaAddition();

    //        if (isPlayer)
    //            _staminaTextManager?.UpdateCurrentStamina(charactersStamina.Stamina);
    //    }
    //    public void OnStartTurn(bool isPlayer)
    //    {
    //        var charactersStamina = GetCharacterStamina(isPlayer);

    //        charactersStamina.StartTurn();
    //        //  charactersStamina.ResetStaminaAddition();

    //        if (isPlayer)
    //            _staminaTextManager?.UpdateCurrentStamina(charactersStamina.Stamina);
    //    }
    //    public bool IsEnoughStamina(bool isPlayer, CardData card)
    //     => GetCharacterStamina(isPlayer).Stamina >= card.StaminaCost;
    //    public bool HasStamina(bool isPlayer) => GetCharacterStamina(isPlayer).Stamina > 0;
    //    public void ReduceStamina(bool isPlayer, CardData card)
    //    {
    //        var character = GetCharacterStamina(isPlayer);
    //        character.Stamina -= card.StaminaCost;

    //        if (isPlayer)
    //            _staminaTextManager?.UpdateCurrentStamina(character.Stamina);
    //    }

    //    public void ReduceStamina(bool isPlayer, int amount)
    //    {
    //        var character = GetCharacterStamina(isPlayer);
    //        character.Stamina -= amount;

    //        if (isPlayer)
    //            _staminaTextManager?.UpdateCurrentStamina(character.Stamina);
    //    }

    //    public void AddStartStamina(bool isPlayer, int Amount)
    //    {
    //        var character = GetCharacterStamina(isPlayer);
    //        character.AddStaminaAddition(Amount);
    //        // character.StartStamina += Amount;
    //    }
    //    public void AddStamina(bool isPlayer, int amount)
    //    {
    //        var character = GetCharacterStamina(isPlayer);
    //        character.Stamina += amount;
    //        if (isPlayer)
    //            _staminaTextManager?.UpdateCurrentStamina(character.Stamina);
    //    }




    //    #endregion
    //}

}