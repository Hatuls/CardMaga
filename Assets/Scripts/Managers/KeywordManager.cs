using Battle;
using Battle.Turns;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
using CardMaga.Tools.Pools;
using Characters.Stats;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Keywords
{
    [System.Serializable]
    public class KeywordEvent : UnityEvent<BaseKeywordLogic> { }
    public class KeywordManager : MonoBehaviour, ISequenceOperation<IBattleManager>
    {




        [SerializeField]
        KeywordEvent keywordEvent;
        #region Fields
        private Dictionary<KeywordTypeEnum, BaseKeywordLogic> _keywordDict;



        private GameTurnHandler _turnHandler;
        private IPlayersManager _playersManager;

        #endregion






        #region public Functions

        public int Priority => 0;

        public void ExecuteTask(ITokenReciever token, IBattleManager bm)
        {
            using (System.IDisposable t = token.GetToken())
            {
                InitParams();

                _turnHandler = bm.TurnHandler;
                RegisterTurnEvents(_turnHandler.GetTurn(GameTurnType.LeftPlayerTurn));
                RegisterTurnEvents(_turnHandler.GetTurn(GameTurnType.RightPlayerTurn));
                _playersManager = bm.PlayersManager;

                bm.OnBattleManagerDestroyed += BeforeBattleEnded;
            }

            void RegisterTurnEvents(GameTurn turn)
            {
                turn.StartTurnOperations.Register(StartTurnKeywords);
                turn.EndTurnOperations.Register(EndTurnKeywords);
            }
        }


        public bool IsCharcterIsStunned(bool isPlayer)
        {
            bool isStunned;
            var stunStat = _playersManager.GetCharacter(isPlayer).StatsHandler.GetStat(KeywordTypeEnum.Stun);
            isStunned = stunStat.HasValue();

            stunStat.Reset();

            return isStunned;
        }

        public IKeyword GetLogic(KeywordSO keywordSO) => GetLogic(keywordSO.GetKeywordType);
        public IKeyword GetLogic(KeywordTypeEnum keywordType)
        {
            if (_keywordDict != null && _keywordDict.Count > 0 && _keywordDict.TryGetValue(keywordType, out BaseKeywordLogic keywordEffect))
                return keywordEffect;
            throw new System.Exception($"KeywordManager does not find the logic of {keywordType.ToString()}");
        }
        #endregion

        #region Private Functions
        private void EndTurnKeywords(ITokenReciever token)
        {
            bool isPlayer = _turnHandler.IsLeftCharacterTurn;
            //            Debug.Log("Activating Keywords Effect on " + (isPlayer ? "LeftPlayer" : "RightPlayer") + " that are activated on the end of the turn");
            var characterStats = _playersManager.GetCharacter(isPlayer).StatsHandler;
            var vulnrable = characterStats.GetStat(KeywordTypeEnum.Vulnerable);
            if (vulnrable.Amount > 0)
                vulnrable.Reduce(1);

            var Weak = characterStats.GetStat(KeywordTypeEnum.Weak);
            if (Weak.Amount > 0)
                Weak.Reduce(1);



        }
        private void StartTurnKeywords(ITokenReciever token)
        {
            bool isPlayer = _turnHandler.IsLeftCharacterTurn;
            var characterStats = _playersManager.GetCharacter(isPlayer).StatsHandler;
            Debug.Log("Activating Keywords Effect on " + (isPlayer ? "LeftPlayer" : "RightPlayer") + " that are activated on the start of the turn");



            characterStats.ApplyBleed();
            characterStats.ApplyHealRegeneration();


            //  yield return Battles.Turns.Turn.WaitOneSecond;
        }
        private void BeforeBattleEnded(IBattleManager bm)
        {
            bm.OnBattleManagerDestroyed -= BeforeBattleEnded;

        }

        private void InitParams()
        {
            if (_keywordDict == null)
            {
                _keywordDict = new Dictionary<KeywordTypeEnum, BaseKeywordLogic>() {
                {KeywordTypeEnum.Attack , new AttackKeywordLogic() },
                {KeywordTypeEnum.Heal , new HealKeyword() },
                {KeywordTypeEnum.Shield , new DefenseKeyword() },
                {KeywordTypeEnum.Strength , new StrengthKeyword() },
                {KeywordTypeEnum.Bleed , new BleedKeyword() },
                {KeywordTypeEnum.Stamina, new StaminaKeyword()},
                {KeywordTypeEnum.Dexterity, new DexterityKeyword() },
                {KeywordTypeEnum.Regeneration, new HealthRegenerationKeyword() },
                {KeywordTypeEnum.MaxHealth, new MaxHealthKeyword() },
            //    {KeywordTypeEnum.Coins, new DoubleKeyword() },
                {KeywordTypeEnum.MaxStamina, new MaxStaminaKeyword() },
                {KeywordTypeEnum.Interupt, new InteruptKeyword() },
                {KeywordTypeEnum.Draw, new DrawKeyword() },
                {KeywordTypeEnum.Clear, new ClearKeyword() },
                {KeywordTypeEnum.Shuffle, new ShuffleKeyword() },
                {KeywordTypeEnum.Stun, new StunKeyword()},
                {KeywordTypeEnum.Weak,new WeakKeyword() },
                {KeywordTypeEnum.Vulnerable, new VulnerableKeyword() },
                {KeywordTypeEnum.ProtectionShard, new ProtectionShardKeyword() },
                {KeywordTypeEnum.RageShard, new RageShardKeyword() },
                {KeywordTypeEnum.StaminaShards, new StaminaShardKeyword() },
                {KeywordTypeEnum.StunShard, new StunShardKeyword() },
                {KeywordTypeEnum.Double, new DoubleKeyword() },
                {KeywordTypeEnum.Fatigue, new  FatigueKeyword()}
            };
            }
            if (_keywordDict == null)
                Debug.LogError("Keyword Manager: Dictionary of keywords was not assigned!");
        }






        #endregion


        #region Monobehaviour Callbacks 
        public void Awake()
        {
            BattleManager.Register(this, OrderType.Before);
        }


        #endregion
    }
    public class VisualKeywordsHandler : ISequenceOperation<IBattleManager>
    {
        public readonly IPoolObject<VisualKeywordCommand> VisualKeywordCommandsPool;
        public readonly IPoolObject<VisualKeywordsPackCommands> VisualKeywordPackCommandsPool;

        private IPlayersManager _playersManager;
        private VisualKeywordsPackCommands _current;
        private GameVisualCommands _gameVisualCommands;
        public int Priority => 0;
        public VisualKeywordsHandler()
        {
            VisualKeywordCommandsPool = new ObjectPool<VisualKeywordCommand>();
            VisualKeywordPackCommandsPool = new ObjectPool<VisualKeywordsPackCommands>();
        }
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleManager data)
        {
            _gameVisualCommands = data.GameCommands.GameVisualCommands;
            _playersManager = data.PlayersManager;
            _playersManager.GetCharacter(false).StatsHandler.OnStatValueChanged += StatChanged;
            _playersManager.GetCharacter(true).StatsHandler.OnStatValueChanged += StatChanged;

            CardsKeywordsCommands.OnCardsKeywordsStartedExecuted += DrawNewVisualKeywordCommandPack;
            CardsKeywordsCommands.OnCardsKeywordsFinishedExecuted += RegisterVisualCommand;

            data.OnBattleManagerDestroyed += BeforeBattleDestroyed;
        }

        private void BeforeBattleDestroyed(IBattleManager obj)
        {
            _playersManager.GetCharacter(false).StatsHandler.OnStatValueChanged -= StatChanged;
            _playersManager.GetCharacter(true).StatsHandler.OnStatValueChanged -= StatChanged;

            CardsKeywordsCommands.OnCardsKeywordsStartedExecuted -= DrawNewVisualKeywordCommandPack;
            CardsKeywordsCommands.OnCardsKeywordsFinishedExecuted -= RegisterVisualCommand;

        }

        private void DrawNewVisualKeywordCommandPack(CommandType command)
        {
            _current = VisualKeywordPackCommandsPool.Pull();
            _current.Init(command);
        }

        private void RegisterVisualCommand() => _gameVisualCommands.VisualKeywordCommandHandler.AddCommand(_current);
        private void StatChanged(bool isPlayer, KeywordTypeEnum keywordTypeEnum, int value)
        {
            VisualKeywordCommand cmd = VisualKeywordCommandsPool.Pull();
            cmd.Init(keywordTypeEnum, value, _playersManager.GetCharacter(isPlayer).VisualCharacter.VisualStats);

            _current.AddVisualKeywordCommands(cmd);
        }
    }
    public class VisualKeywordsPackCommands : ISequenceCommand, IPoolable<VisualKeywordsPackCommands>
    {
        public event Action OnFinishExecute;
        public event Action<VisualKeywordsPackCommands> OnDisposed;

        private List<VisualKeywordCommand> _visualKeywordCommands;
        public IReadOnlyList<VisualKeywordCommand> VisualKeywordCommands => _visualKeywordCommands;
        private CommandType _commandType;

        public CommandType CommandType { get => _commandType; private set => _commandType = value; }



        public void Init(CommandType commandType)
        {
            _commandType = commandType;
            _visualKeywordCommands = new List<VisualKeywordCommand>();
        }
        public void AddVisualKeywordCommands(VisualKeywordCommand visualKeywordCommand) => _visualKeywordCommands.Add(visualKeywordCommand);
        public void Execute()
        {
            for (int i = 0; i < _visualKeywordCommands.Count; i++)
                _visualKeywordCommands[i].Execute();
            OnFinishExecute?.Invoke();
            Dispose();
        }

        public void Undo()
        {
            for (int i = 0; i < _visualKeywordCommands.Count; i++)
                _visualKeywordCommands[i].Execute();
            OnFinishExecute?.Invoke();
            Dispose();
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }
    }


    public enum KeywordTypeEnum
    {
        None = 0,
        Attack = 1,
        Shield = 2,
        Heal = 3,
        Strength = 4,
        Bleed = 5,
        MaxHealth = 6,
        Interupt = 7,
        Weak = 8,
        Vulnerable = 9,
        Fatigue = 10,
        Regeneration = 11,
        Dexterity = 12,
        Draw = 13,
        MaxStamina = 14,
        LifeSteal = 15,
        Remove = 16,
        Counter = 17,
        //Coins = 18,
        StaminaShards = 19,
        Discard = 20,
        Double = 21,
        Stamina = 22,
        Fast = 23,
        Freeze = 24,
        Burn = 25,
        Lock = 26,
        Empower = 27,
        Reinforce = 28,
        Clear = 29,
        Find = 30,
        Shuffle = 31,
        Push = 32,
        Stun = 33,
        StunShard = 34,
        RageShard = 35,
        Rage = 36,
        ProtectionShard = 37,
        Protected = 38,
        Reset = 39,
        SpiritLoss = 40,
        Brused = 41,
        Frail = 42,
        Confuse = 43,
        Deny = 44,
        Intimidate = 45,
        Taunt = 46,
        Disable = 47,
        Limit = 48,
        BloodLoss = 49,
        Sacrifice = 50,
        Shield_Bash = 51,
        Permanent_Defense = 52,
    };

}