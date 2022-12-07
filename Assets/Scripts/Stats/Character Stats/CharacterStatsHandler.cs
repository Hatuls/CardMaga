 using CardMaga.Keywords;
using System;
using System.Collections.Generic;

namespace Characters.Stats
{
    public class CharacterStatsHandler : IDisposable
    {
        private readonly bool IsPlayer;
        public event Action<bool, CharacterStatsHandler> OnStatAssigned;
        public event Action<bool, KeywordType, int> OnStatValueChanged;
        public event Action OnUpdateStatNeeded;

        [Sirenix.OdinInspector.ReadOnly,Sirenix.OdinInspector.ShowInInspector]
        private Dictionary<KeywordType, BaseStat> _statsDictionary;

        public IReadOnlyDictionary<KeywordType, BaseStat> StatDictionary => _statsDictionary;



        public CharacterStatsHandler(bool isPlayer, ref CharacterStats stats, StaminaHandler staminaHandler)
        {
            IsPlayer = isPlayer;


            //stats
            MaxHealthStat _max = new MaxHealthStat(stats.MaxHealth);
            HealthStat _health = new HealthStat(_max, isPlayer, stats.Health); ;
            StrengthStat _str = new StrengthStat(stats.Strength);
            DexterityStat _dex = new DexterityStat(stats.Dexterity);
            ShieldStat _defense = new ShieldStat(_health, stats.Shield);
            BleedStat _bleed = new BleedStat(stats.Bleed);
            HealthRegenerationStat _regen = new HealthRegenerationStat(stats.RegenerationPoints);
            // CoinStat _coin = new CoinStat(isPlayer, stats.Gold);
            StaminaStat _stamina = new StaminaStat(stats.StartStamina);
            DrawCardStat _draw = new DrawCardStat(stats.DrawCardsAmount);
            RageStat _rage = new RageStat(stats.RagePoint);
            ProtectedStat _protected = new ProtectedStat(stats.ProtectionPoints);
            WeakStat _weakStat = new WeakStat(stats.Weakend);
            VulnerableKeyword _vulnerableKeyword = new VulnerableKeyword(stats.Weakend);
            //Effects
            StunStat _stun = new StunStat(stats.Stun);

            //shards
            StaminaShard _staminaShards = new StaminaShard(stats.StaminaShard, staminaHandler, _stamina);
            StunShard _stunShards = new StunShard(stats.StunShard, _stun);
            RageShard _rageShard = new RageShard(stats.RageShard, _rage);
            ProtectionShard _protectionShard = new ProtectionShard(stats.ProtectionShards, _protected);

            _max.HealthStat = _health;

            const int StatsCapacity = 20;

            _statsDictionary = new Dictionary<KeywordType, BaseStat>(StatsCapacity) {
                {_health.Keyword,_health },
                {_defense.Keyword,_defense },
                { _max.Keyword,_max },
                {_str.Keyword,_str },
                {_dex.Keyword,_dex },
                {_bleed.Keyword,_bleed },
                {_regen.Keyword,_regen },
              //  {_coin.Keyword,_coin },
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

            BaseStat stat = null;
            foreach (var item in StatDictionary)
            {
                stat = item.Value;
                stat.OnStatsUpdated += StatChangedDetected;
            }


            OnStatAssigned?.Invoke(IsPlayer, this);
        }


        public BaseStat GetStat(KeywordType keyword)
        {
            if (_statsDictionary.TryGetValue(keyword, out BaseStat stat))
                return stat;

            UnityEngine.Debug.LogError("Keyword Was Not Found!");
            return null;
   }

   

        private void StatChangedDetected(int value, KeywordType keywordTypeEnum)
        => OnStatValueChanged?.Invoke(IsPlayer, keywordTypeEnum, value);

        public void Dispose()
        {

            foreach (var item in StatDictionary)
                item.Value.OnStatsUpdated -= StatChangedDetected;

        }
    }
}