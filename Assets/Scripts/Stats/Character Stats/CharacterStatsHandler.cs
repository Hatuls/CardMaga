using Keywords;
namespace Characters.Stats
{
    public class CharacterStatsHandler
    {
        public static event System.Action<bool, CharacterStatsHandler> OnStatAssigned;

        private System.Collections.Generic.Dictionary<KeywordTypeEnum, BaseStat> _statsDictionary;
        public CharacterStatsHandler(bool isPlayer, ref CharacterStats stats)
        {
            //stats
            MaxHealthStat _max = new MaxHealthStat(isPlayer, stats.MaxHealth);
            HealthStat _health = new HealthStat(_max, isPlayer, stats.Health); ;
            StrengthStat _str = new StrengthStat(isPlayer, stats.Strength);
            DexterityStat _dex = new DexterityStat(isPlayer, stats.Dexterity);
            ShieldStat _defense = new ShieldStat(_health,isPlayer, stats.Shield);
            BleedStat _bleed = new BleedStat(isPlayer, stats.Bleed);
            HealthRegenerationStat _regen = new HealthRegenerationStat(isPlayer, stats.RegenerationPoints);
            CoinStat _coin = new CoinStat(isPlayer, stats.Gold);
            StaminaStat _stamina = new StaminaStat(isPlayer, stats.StartStamina);
            DrawCardStat _draw = new DrawCardStat(isPlayer, stats.DrawCardsAmount);
            RageStat _rage = new RageStat(isPlayer, stats.RagePoint);
            ProtectedStat _protected = new ProtectedStat(isPlayer, stats.ProtectionPoints);
            WeakStat _weakStat = new WeakStat(isPlayer, stats.Weakend);
            VulnerableKeyword _vulnerableKeyword = new VulnerableKeyword(isPlayer, stats.Weakend);
            //Effects
            StunStat _stun = new StunStat(isPlayer);

            //shards
            StaminaShard _staminaShards = new StaminaShard(isPlayer, stats.StaminaShard);
            StunShard _stunShards = new StunShard(isPlayer, stats.StunShard);
            RageShard _rageShard = new RageShard(isPlayer, stats.RageShard);
            ProtectionShard _protectionShard = new ProtectionShard(isPlayer, stats.ProtectionShards);

            _max._healthStat = _health;

            const int StatsCapacity = 20;

            _statsDictionary = new System.Collections.Generic.Dictionary<KeywordTypeEnum, BaseStat>(StatsCapacity) {
                {_health.Keyword,_health },
                {_defense.Keyword,_defense },
                { _max.Keyword,_max },
                {_str.Keyword,_str },
                {_dex.Keyword,_dex },
                {_bleed.Keyword,_bleed },
                {_regen.Keyword,_regen },
                {_coin.Keyword,_coin },
                {_stamina.Keyword,_stamina },
                {_draw.Keyword,_draw },
                {_rage.Keyword,_rage },
                {_protected.Keyword,_protected },
                {_staminaShards.Keyword,_staminaShards },
                {_stunShards.Keyword,_stunShards },
                {_rageShard.Keyword,_rageShard },
                {_protectionShard.Keyword,_protectionShard },
                {_stun.Keyword,_stun },
                {_weakStat.Keyword,_weakStat },
                {_vulnerableKeyword.Keyword,_vulnerableKeyword  },
            };

            OnStatAssigned?.Invoke(isPlayer, this);
        }

        public BaseStat GetStats(KeywordTypeEnum keyword)
        {
            if (_statsDictionary.TryGetValue(keyword, out BaseStat stat))
                return stat;

            UnityEngine.Debug.LogError("Keyword Was Not Found!");
            return null;
        }
        public void ApplyHealRegeneration()
        {
            if (_statsDictionary[KeywordTypeEnum.Regeneration].Amount > 0)
            {
                _statsDictionary[KeywordTypeEnum.Heal].Add(_statsDictionary[KeywordTypeEnum.Regeneration].Amount);
                _statsDictionary[KeywordTypeEnum.Regeneration].Reduce(1);
            }
        }
        public void ApplyBleed()
        {
            if (_statsDictionary[KeywordTypeEnum.Bleed].Amount > 0)
            {
                _statsDictionary[KeywordTypeEnum.Heal].Reduce(_statsDictionary[KeywordTypeEnum.Bleed].Amount);
                _statsDictionary[KeywordTypeEnum.Bleed].Reduce(1);
            }
        }
        public void RecieveDamage(int amount, bool pierceThroughTheArmour = false)
        {
            if (pierceThroughTheArmour)
                GetStats(KeywordTypeEnum.Heal).Reduce(amount);
            else
                GetStats(KeywordTypeEnum.Shield).Reduce(amount);
        }


    }
}