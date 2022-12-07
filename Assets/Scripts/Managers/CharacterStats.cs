using CardMaga.Keywords;
using System;
using UnityEngine;
namespace Characters.Stats
{
    [Serializable]
    public struct CharacterStats
    {
        #region Fields
        [Header("Character Stats: ")]
        [Tooltip("Character Max Health:")]
        [SerializeField] int _maxHealth;

        [Tooltip("Character Health:")]
        [SerializeField] int _health;

        [Tooltip("Character Defense:")]
        [SerializeField] int _defense;

        [Tooltip("Character Gold:")]
        [SerializeField] int _gold;

        [Tooltip("Character Strength:")]
        [SerializeField] int _strengthPoint;

        [Tooltip("Character Bleed:")]
        [SerializeField] int _bleedPoints;

        [Tooltip("Character Cards Draw")]
        [SerializeField] int _cardDraw;

        [SerializeField] int _startStamina;

        [SerializeField] int _regenerationPoints;

        [SerializeField] int _stunShards;

        [SerializeField] int _staminaShard;

        [SerializeField] int _protectionShard;

        [SerializeField] int _rageShard;

        [SerializeField] int _ragePoints;

        [SerializeField] int _protectionPoints;


        [SerializeField] int _weak;
        [SerializeField] int _vulnerable;





        #endregion

        #region Properties
        public int StaminaShard
        {
            get => _staminaShard;
            set
            {
                _staminaShard = value;
                var keywordHandler = Factory.GameFactory.Instance.KeywordFactoryHandler;
                int maxAmount = keywordHandler.GetKeywordSO(KeywordType.StaminaShards).InfoAmount;

                if (_staminaShard / maxAmount > 0)
                {
                    StartStamina += _staminaShard / maxAmount;
                    _staminaShard %= maxAmount;
                }
            }
        }
        public int RagePoint { get => _ragePoints; private set => _ragePoints = value; }
        public int RageShard
        {
            get => _rageShard; 
            set
            {
                _rageShard = value;
                var keywordHandler = Factory.GameFactory.Instance.KeywordFactoryHandler;
                int maxAmount = keywordHandler.GetKeywordSO(KeywordType.RageShard).InfoAmount;

                if (_rageShard / maxAmount > 0)
                {
                    RagePoint += _rageShard / maxAmount;
                    _rageShard %= maxAmount;
                }
            }
        }
        public int ProtectionPoints { get => _protectionPoints; private set => _protectionPoints = value; }
        public int ProtectionShards
        {
            get => _protectionShard; private set
            {
                _protectionShard = value;
                var keywordHandler = Factory.GameFactory.Instance.KeywordFactoryHandler;
                int maxAmount = keywordHandler.GetKeywordSO(KeywordType.ProtectionShard).InfoAmount;

                if (_protectionShard / maxAmount > 0)
                {
                    ProtectionShards += _protectionShard / maxAmount;
                    _protectionShard %= maxAmount;
                }
            }
        }
        public int Vulnerable => _vulnerable;
        public int Weakend => _weak;
        public int StunShard { get => _stunShards;
            set => _stunShards = value;
        }
        public int RegenerationPoints { get => _regenerationPoints; set => _regenerationPoints = value; }
        public int Dexterity { get; set; }
        public int StartStamina { get => _startStamina; set => _startStamina = value; }
        public int DrawCardsAmount { get => _cardDraw; set => _cardDraw = value; }
        public int Health { get => _health; set => _health = value; }
        public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public int Shield { get => _defense; set => _defense = value; }
        public int Gold { get => _gold; set => _gold = value; }
        public int Strength { get => _strengthPoint; set => _strengthPoint = value; }
        public int Bleed { get => _bleedPoints; set => _bleedPoints = value; }

        #endregion


    }
}