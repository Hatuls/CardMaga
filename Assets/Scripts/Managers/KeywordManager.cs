using Battle.Turns;
using CardMaga.Battle;
using CardMaga.Battle.Execution;
using CardMaga.Battle.Players;
using CardMaga.Battle.UI;
using CardMaga.Commands;
using CardMaga.SequenceOperation;
using CardMaga.Tools.Pools;
using Characters.Stats;
using Keywords;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using static Factory.GameFactory;

namespace CardMaga.Keywords
{

    public class KeywordManager : ISequenceOperation<IBattleManager>
    {
        public event Action<CommandType> OnStartTurnKeywordEffect;
        public event Action OnStartTurnKeywordEffectExecuted;
        public event Action OnStartTurnKeywordEffectFinished;

        public event Action<CommandType> OnEndtTurnKeywordEffect;
        public event Action OnEndTurnKeywordEffectExecuted;
        public event Action OnEndTurnKeywordEffectFinished;

        #region Fields
        private Dictionary<KeywordType, BaseKeywordLogic> _keywordDict;
        private TurnHandler _turnHandler;
        private IPlayersManager _playersManager;
        private GameDataCommands _dataCommands;
        private KeywordFactory _keywordFactory;
        private List<BaseKeywordLogic> _activeTurnKeywords;
        #endregion


        public int Priority => 0;

        public KeywordManager(IBattleManager battleManager)
        {
            battleManager.Register(this, OrderType.Before);
            _keywordFactory = Factory.GameFactory.Instance.KeywordFactoryHandler;
            _activeTurnKeywords = new List<BaseKeywordLogic>();
        }


        #region public Functions
        private void AddToTurnKeywords(BaseKeywordLogic baseKeywordLogic)
        {
            if (!_activeTurnKeywords.Contains(baseKeywordLogic))
            {
                _activeTurnKeywords.Add(baseKeywordLogic);
                _activeTurnKeywords.Sort();
            }
        }
        private void RemoveFromTurnKeywords(BaseKeywordLogic baseKeywordLogic)
        {
            bool leftHasValue = HasValue(_playersManager.GetCharacter(true));
            bool rightHasValue = HasValue(_playersManager.GetCharacter(false));
            bool isStillNeeded = (rightHasValue && leftHasValue);

            if (isStillNeeded == false)
                _activeTurnKeywords.Remove(baseKeywordLogic);

            bool HasValue(IPlayer player) => player.StatsHandler.GetStat(baseKeywordLogic.KeywordType).HasValue();
        }
        public void ExecuteTask(ITokenReciever token, IBattleManager bm)
        {
            using (System.IDisposable t = token.GetToken())
            {
                _playersManager = bm.PlayersManager;
                _dataCommands = bm.GameCommands.GameDataCommands;
                _turnHandler = bm.TurnHandler;
                InitParams();
                RegisterTurnEvents(_turnHandler.GetTurn(GameTurnType.LeftPlayerTurn));
                RegisterTurnEvents(_turnHandler.GetTurn(GameTurnType.RightPlayerTurn));

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
            var stunStat = _playersManager.GetCharacter(isPlayer).StatsHandler.GetStat(KeywordType.Stun);
            isStunned = stunStat.HasValue();

            stunStat.Reset();

            return isStunned;
        }

        public IKeyword GetLogic(KeywordSO keywordSO) => GetLogic(keywordSO.GetKeywordType);
        public IKeyword GetLogic(KeywordType keywordType)
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

            var currentPlayer = _playersManager.GetCharacter(isPlayer);

            for (int i = 0; i < _activeTurnKeywords.Count; i++)
            {
                //Filter keywords based on the enum effect type
                OnEndtTurnKeywordEffect?.Invoke(CommandType.AfterPrevious);
                _activeTurnKeywords[i].EndTurnEffect(currentPlayer, _dataCommands);
                OnEndTurnKeywordEffectExecuted?.Invoke();
            }

            OnEndTurnKeywordEffectFinished?.Invoke();
        }
        private void StartTurnKeywords(ITokenReciever token)
        {
            bool isPlayer = _turnHandler.IsLeftCharacterTurn;
            var currentPlayer = _playersManager.GetCharacter(isPlayer);

            for (int i = 0; i < _activeTurnKeywords.Count; i++)
            {
                OnStartTurnKeywordEffect?.Invoke(CommandType.AfterPrevious);
                //Filter keywords based on the enum effect type
                _activeTurnKeywords[i].StartTurnEffect(currentPlayer, _dataCommands);

                OnStartTurnKeywordEffectExecuted?.Invoke();
            }

            OnStartTurnKeywordEffectFinished?.Invoke();
            //var characterStats = currentPlayer.StatsHandler;
            //Debug.Log("Activating Keywords Effect on " + (isPlayer ? "LeftPlayer" : "RightPlayer") + " that are activated on the start of the turn");

            //KeywordData keyword;
            //KeywordCommand command;
            //BaseStat stat = characterStats.GetStat(KeywordType.Bleed);
            //if (stat.Amount > 0)
            //{
            //    keyword = new KeywordData(_keywordFactory.GetKeywordSO(KeywordType.PierceDamage), TargetEnum.MySelf, stat.Amount, 0);
            //    command = new KeywordCommand(keyword, CommandType.WithPrevious);
            //    command.InitKeywordLogic(currentPlayer, GetLogic(KeywordType.PierceDamage), _playersManager);
            //    _dataCommands.DataCommands.AddCommand(command);

            //    keyword = new KeywordData(_keywordFactory.GetKeywordSO(KeywordType.Bleed), TargetEnum.MySelf, -1, 0);
            //    command = new KeywordCommand(keyword, CommandType.WithPrevious);
            //    command.InitKeywordLogic(currentPlayer, GetLogic(KeywordType.Bleed), _playersManager);

            //    _dataCommands.DataCommands.AddCommand(new KeywordCommand(keyword, CommandType.WithPrevious));

            //}


            //stat = characterStats.GetStat(KeywordType.Regeneration);
            //if (stat.Amount > 0)
            //{
            //    keyword = new KeywordData(_keywordFactory.GetKeywordSO(KeywordType.Heal), TargetEnum.MySelf, stat.Amount, 0);
            //    _dataCommands.DataCommands.AddCommand(new KeywordCommand(keyword, CommandType.WithPrevious));
            //    keyword = new KeywordData(_keywordFactory.GetKeywordSO(KeywordType.Regeneration), TargetEnum.MySelf, -1, 0);
            //    _dataCommands.DataCommands.AddCommand(new KeywordCommand(keyword, CommandType.WithPrevious));
            //}
        }



        private void BeforeBattleEnded(IBattleManager bm)
        {
            bm.OnBattleManagerDestroyed -= BeforeBattleEnded;
            foreach (var keyword in _keywordDict)
            {
                keyword.Value.OnKeywordActivated -= AddToTurnKeywords;
                keyword.Value.OnKeywordFinished -= RemoveFromTurnKeywords;
            }

            _activeTurnKeywords.Clear();
        }

        private void InitParams()
        {
            if (_keywordDict == null)
            {
                var pierceDamage = new PierceDamageKeyword(GetKewordSO(KeywordType.PierceDamage), _playersManager);
                var healLogic = new HealKeyword(GetKewordSO(KeywordType.Heal), _playersManager);
                _keywordDict = new Dictionary<KeywordType, BaseKeywordLogic>()
                {
                    {KeywordType.Attack , new AttackKeywordLogic( GetKewordSO(KeywordType.Attack), _playersManager) },
                    {KeywordType.Heal ,healLogic  },
                    {KeywordType.Regeneration, new HealthRegenerationKeyword(healLogic,GetKewordSO(KeywordType.Regeneration), _playersManager) },
                    {KeywordType.Shield , new DefenseKeyword(GetKewordSO(KeywordType.Shield), _playersManager) },
                    {KeywordType.Strength , new StrengthKeyword(GetKewordSO(KeywordType.Strength), _playersManager) },
                    {KeywordType.Stamina, new StaminaKeyword(GetKewordSO(KeywordType.Stamina), _playersManager)},
                    {KeywordType.Dexterity, new DexterityKeyword(GetKewordSO(KeywordType.Dexterity), _playersManager) },
                    {KeywordType.MaxHealth, new MaxHealthKeyword(GetKewordSO(KeywordType.MaxHealth), _playersManager) },
                    {KeywordType.PierceDamage, pierceDamage },
                    {KeywordType.MaxStamina, new MaxStaminaKeyword(GetKewordSO(KeywordType.MaxStamina), _playersManager) },
                    {KeywordType.Interupt, new InteruptKeyword(GetKewordSO(KeywordType.Interupt), _playersManager) },
                    {KeywordType.Draw, new DrawKeyword(GetKewordSO(KeywordType.Draw), _playersManager) },
                    {KeywordType.Clear, new ClearKeyword(GetKewordSO(KeywordType.Clear), _playersManager) },
                    {KeywordType.Shuffle, new ShuffleKeyword(GetKewordSO(KeywordType.Shuffle), _playersManager) },
                    {KeywordType.Stun, new StunKeyword(GetKewordSO(KeywordType.Stun), _playersManager)},
                    {KeywordType.Weak,new WeakKeyword(GetKewordSO(KeywordType.Weak), _playersManager) },
                    {KeywordType.Vulnerable, new VulnerableKeyword(GetKewordSO(KeywordType.Vulnerable), _playersManager) },
                    {KeywordType.ProtectionShard, new ProtectionShardKeyword(GetKewordSO(KeywordType.ProtectionShard), _playersManager) },
                    {KeywordType.RageShard, new RageShardKeyword(GetKewordSO(KeywordType.RageShard), _playersManager) },
                    {KeywordType.StaminaShards, new StaminaShardKeyword(GetKewordSO(KeywordType.StaminaShards), _playersManager) },
                    {KeywordType.StunShard, new StunShardKeyword(GetKewordSO(KeywordType.StunShard), _playersManager) },
                    {KeywordType.Double, new DoubleKeyword(GetKewordSO(KeywordType.Double), _playersManager) },
                    {KeywordType.Fatigue, new  FatigueKeyword(GetKewordSO(KeywordType.Fatigue), _playersManager)},
                    {KeywordType.Bleed , new BleedKeyword(pierceDamage,GetKewordSO(KeywordType.Bleed), _playersManager) },
                };
            }



            foreach (var keyword in _keywordDict)
            {
                keyword.Value.OnKeywordActivated += AddToTurnKeywords;
                keyword.Value.OnKeywordFinished += RemoveFromTurnKeywords;
            }



            KeywordSO GetKewordSO(KeywordType keywordType)
             => _keywordFactory.GetKeywordSO(keywordType);
        }






        #endregion


    }


    public class VisualKeywordsHandler : ISequenceOperation<IBattleUIManager>
    {
        public readonly IPoolObject<VisualKeywordCommand> VisualKeywordCommandsPool;
        public readonly IPoolObject<VisualKeywordsPackCommands> VisualKeywordPackCommandsPool;

        private IPlayersManager _playersManager;
        private VisualKeywordsPackCommands _current;
        private GameVisualCommands _gameVisualCommands;
        private VisualCharactersManager _visualCharactersManager;
        public int Priority => 0;
        public VisualKeywordsHandler()
        {
            VisualKeywordCommandsPool = new ObjectPool<VisualKeywordCommand>();
            VisualKeywordPackCommandsPool = new ObjectPool<VisualKeywordsPackCommands>();
        }
        public void ExecuteTask(ITokenReciever tokenMachine, IBattleUIManager battleUIManager)
        {
            var data = battleUIManager.BattleDataManager;
            _gameVisualCommands = data.GameCommands.GameVisualCommands;
            _playersManager = data.PlayersManager;
            _playersManager.GetCharacter(false).StatsHandler.OnStatValueChanged += StatChanged;
            _playersManager.GetCharacter(true).StatsHandler.OnStatValueChanged += StatChanged;
            _visualCharactersManager = battleUIManager.VisualCharactersManager;
            CardsKeywordsCommands.OnCardsKeywordsStartedExecuted += DrawNewVisualKeywordCommandPack;
            CardsKeywordsCommands.OnCardsKeywordsFinishedExecuted += RegisterVisualCommand;

            data.KeywordManager.OnEndtTurnKeywordEffect += DrawNewVisualKeywordCommandPack;
            data.KeywordManager.OnEndTurnKeywordEffectExecuted += RegisterVisualCommand;
            data.KeywordManager.OnEndTurnKeywordEffectFinished += _gameVisualCommands.ExecuteKeywords;

            data.KeywordManager.OnStartTurnKeywordEffect += DrawNewVisualKeywordCommandPack;
            data.KeywordManager.OnStartTurnKeywordEffectExecuted += RegisterVisualCommand;
            data.KeywordManager.OnStartTurnKeywordEffectFinished += _gameVisualCommands.ExecuteKeywords;

            data.OnBattleManagerDestroyed += BeforeBattleDestroyed;
        }

        private void BeforeBattleDestroyed(IBattleManager obj)
        {
            _playersManager.GetCharacter(false).StatsHandler.OnStatValueChanged -= StatChanged;
            _playersManager.GetCharacter(true).StatsHandler.OnStatValueChanged -= StatChanged;

            CardsKeywordsCommands.OnCardsKeywordsStartedExecuted -= DrawNewVisualKeywordCommandPack;
            CardsKeywordsCommands.OnCardsKeywordsFinishedExecuted -= RegisterVisualCommand;


            obj.KeywordManager.OnEndtTurnKeywordEffect -= DrawNewVisualKeywordCommandPack;
            obj.KeywordManager.OnEndTurnKeywordEffectExecuted -= RegisterVisualCommand;
            obj.KeywordManager.OnEndTurnKeywordEffectFinished -= _gameVisualCommands.ExecuteKeywords;

            obj.KeywordManager.OnStartTurnKeywordEffect -= DrawNewVisualKeywordCommandPack;
            obj.KeywordManager.OnStartTurnKeywordEffectExecuted -= RegisterVisualCommand;
            obj.KeywordManager.OnStartTurnKeywordEffectFinished -= _gameVisualCommands.ExecuteKeywords;
        }

        private void DrawNewVisualKeywordCommandPack(CommandType command)
        {
            _current = VisualKeywordPackCommandsPool.Pull();
            _current.Init(command);
        }

        private void RegisterVisualCommand() => _gameVisualCommands.VisualKeywordCommandHandler.AddCommand(_current);
        private void StatChanged(bool isPlayer, KeywordType keywordTypeEnum, int value)
        {
            if (_current == null)
                DrawNewVisualKeywordCommandPack(CommandType.Instant);

              VisualKeywordCommand cmd = VisualKeywordCommandsPool.Pull();
            cmd.Init(keywordTypeEnum, value, _visualCharactersManager.GetVisualCharacter(isPlayer).VisualStats);

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


    public enum KeywordType
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
        PierceDamage = 18,
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