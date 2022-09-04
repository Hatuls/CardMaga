﻿
using Battle;
using Battle.Turns;
using Characters.Stats;
using Managers;
using ReiTools.TokenMachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Keywords
{
    [System.Serializable]
    public class KeywordEvent : UnityEvent<KeywordAbst> { }
    public class KeywordManager : MonoBehaviour, ISequenceOperation<BattleManager>
    {
        [SerializeField]
        KeywordEvent keywordEvent;
        #region Fields
        private  Dictionary<KeywordTypeEnum, KeywordAbst> _keywordDict;



        public int Priority => 0;
        private GameTurnHandler _turnHandler;
        private IPlayersManager _playersManager;
        #endregion






        #region public Functions


        public void ExecuteTask(ITokenReciever token, BattleManager bm)
        {
            using (token.GetToken())
            {
                InitParams();
                _turnHandler = bm.TurnHandler;
                RegisterTurnEvents(_turnHandler.GetTurn(GameTurnType.LeftPlayerTurn));
                RegisterTurnEvents(_turnHandler.GetTurn(GameTurnType.RightPlayerTurn));
                _playersManager = bm.PlayersManager;
            }
            void RegisterTurnEvents(GameTurn turn)
            {
                turn.StartTurnOperations.Register(StartTurnKeywords);
                turn.EndTurnOperations.Register(EndTurnKeywords);
            }
        }


        public void ActivateKeyword(bool isPlayerTurn, KeywordData keyword)
        {
            if (Battle.BattleManager.isGameEnded)
                return;

            if (keyword == null)
            {
                Debug.Log("KeywordManager: Tried to activate a Null keyword!");
                return;
            }
            else if (keyword.GetTarget == TargetEnum.None)
            {
                Debug.Log("KeywordManager: The target Enum is None");
                return;
            }
            else if (keyword.KeywordSO.GetKeywordType == KeywordTypeEnum.None)
            {
                Debug.Log("KeywordManager: The Keyword Type Enum is None");
                return;
            }

            if (_keywordDict != null && _keywordDict.Count > 0 && _keywordDict.TryGetValue(keyword.KeywordSO.GetKeywordType, out KeywordAbst keywordEffect))
            {
                keywordEffect.ProcessOnTarget(isPlayerTurn, keyword, _playersManager);

                keywordEvent?.Invoke(keywordEffect);

            }


        }

        public bool IsCharcterIsStunned(bool isPlayer)
        {
            bool isStunned;
            var stunStat = CharacterStatsManager.GetCharacterStatsHandler(isPlayer).GetStats(KeywordTypeEnum.Stun);
            isStunned = stunStat.HasValue();

            stunStat.Reset();

            return isStunned;
        }
        private void StartTurnKeywords(ITokenReciever token)
        {
            bool isPlayer = _turnHandler.IsLeftCharacterTurn;
            var characterStats = CharacterStatsManager.GetCharacterStatsHandler(isPlayer);
            Debug.Log("Activating Keywords Effect on " + (isPlayer ? "Player" : "Enemy") + " that are activated on the start of the turn");



            characterStats.ApplyBleed();
            characterStats.ApplyHealRegeneration();


            //  yield return Battles.Turns.Turn.WaitOneSecond;
        }


        private void EndTurnKeywords(ITokenReciever token)
        {
            bool isPlayer = _turnHandler.IsLeftCharacterTurn;
            Debug.Log("Activating Keywords Effect on " + (isPlayer ? "Player" : "Enemy") + " that are activated on the end of the turn");
            var characterStats = CharacterStatsManager.GetCharacterStatsHandler(isPlayer);
            var vulnrable = characterStats.GetStats(KeywordTypeEnum.Vulnerable);
            if (vulnrable.Amount > 0)
                vulnrable.Reduce(1);

            var Weak = characterStats.GetStats(KeywordTypeEnum.Weak);
            if (Weak.Amount > 0)
                Weak.Reduce(1);



        }
        #endregion

        #region Private Functions


        private void InitParams()
        {
            if (_keywordDict == null)
            {
                _keywordDict = new Dictionary<KeywordTypeEnum, KeywordAbst>() {
                {KeywordTypeEnum.Attack , new AttackKeyword() },
                {KeywordTypeEnum.Heal , new HealKeyword() },
                {KeywordTypeEnum.Shield , new DefenseKeyword() },
                {KeywordTypeEnum.Strength , new StrengthKeyword() },
                {KeywordTypeEnum.Bleed , new BleedKeyword() },
                {KeywordTypeEnum.Stamina, new StaminaKeyword()},
                {KeywordTypeEnum.Dexterity, new DexterityKeyword() },
                {KeywordTypeEnum.Regeneration, new HealthRegenerationKeyword() },
                {KeywordTypeEnum.MaxHealth, new MaxHealthKeyword() },
                {KeywordTypeEnum.Coins, new CoinKeyword() },
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
            CardExecutionManager.OnKeywordExecute += ActivateKeyword;
        }

        private void OnDestroy()
        {
            CardExecutionManager.OnKeywordExecute -= ActivateKeyword;
        }

        #endregion
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
        Coins = 18,
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