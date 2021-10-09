using Keywords;
namespace Characters.Stats
{
    public class CharacterStatsHandler
    {
      
        const int StatsCapacity=10;
        private System.Collections.Generic.Dictionary<KeywordTypeEnum, StatAbst> _statsDictionary;
        public CharacterStatsHandler(bool isPlayer,ref CharacterStats stats)
        {

            MaxHealthStat _max = null;
            HealthStat _health=null;
            ShieldStat _defense = null ;
            StrengthStat _str = new StrengthStat(isPlayer, stats.Strength);
            DexterityStat _dex = new DexterityStat(isPlayer,  stats.Dexterity);
            BleedStat _bleed = new BleedStat(isPlayer, stats.Bleed);
            HealthRegenerationStat _regen = new HealthRegenerationStat(isPlayer,  stats.RegenerationPoints);
            CoinStat _coin = new CoinStat(isPlayer,  stats.Gold);
            StaminaStat _stamina = new StaminaStat(isPlayer,  stats.StartStamina);
            DrawCardStat _draw = new DrawCardStat(isPlayer,  stats.DrawCardsAmount);

            _max = new MaxHealthStat(_health, isPlayer,  stats.MaxHealth);
            _health = new HealthStat(_max, isPlayer,  stats.Health);
            _defense = new ShieldStat(_health, isPlayer,  stats.Shield);



            _statsDictionary = new System.Collections.Generic.Dictionary<KeywordTypeEnum, StatAbst>(StatsCapacity) {
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
            };
        }

        public StatAbst GetStats(KeywordTypeEnum keyword)
        {
            if (_statsDictionary.TryGetValue(keyword , out StatAbst t))      
                return t;
            
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
            if (_statsDictionary[KeywordTypeEnum.Bleed].Amount>0)
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