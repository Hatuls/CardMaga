using UnityEngine;
using System;
namespace Characters.Stats
{ 
    public class StatsHandler
    {

        public static RemoveUIIcon _removeUIIcon;
        public static Battles.UI.UpdateUiStats _updateUIStats;
        public Action<SoundsNameEnum> _playSound;
        private StatsHandler()
        {
       
            _removeUIIcon = new RemoveUIIcon();
            _removeUIIcon.AddListener(Battles.UI.BattleUiManager.Instance.RemoveBuffUI);
            _updateUIStats = new Battles.UI.UpdateUiStats();
            _updateUIStats.AddListener(Battles.UI.BattleUiManager.Instance.UpdateUiStats);
            _playSound += AudioManager.Instance.PlayerAudioSource;
        }
         ~StatsHandler()
        {
            _updateUIStats.RemoveAllListeners();
            _removeUIIcon.RemoveAllListeners();
            _playSound -= AudioManager.Instance.PlayerAudioSource;
        }
        private static StatsHandler _instance;
        public static StatsHandler GetInstance
        {
            get
            {
                if (_instance == null)
                    _instance = new StatsHandler();
                
                return _instance;
            }
        }

        public ref CharacterStats GetCharacterStats(bool isPlayerStats)
        {
            if (isPlayerStats)
                return ref Managers.PlayerManager.Instance.GetCharacterStats;
            else
                return ref Battles.EnemyManager.Instance.GetEnemy.GetCharacterStats;
        }
        public void ResetHealth(bool isPlayer)
        {
            GetCharacterStats(isPlayer).Health = GetCharacterStats(isPlayer).MaxHealth;
        }
        public void ResetShield(bool isPlayer)
        {
         
            if (true) // later on add relic check
            {
                GetCharacterStats(isPlayer).Shield = 0; 
                Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, 0);
            }
        }
        public void AddShield(bool isPlayer,int amount)
        {
            if (amount > 0)
            {

                  ref CharacterStats characterStats =ref GetCharacterStats(isPlayer);
                characterStats.Shield += amount;
                Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, characterStats.Shield);
                _playSound.Invoke(SoundsNameEnum.GainArmor);
            }
        }
        public void AddGold(bool isPlayer,int amount)
           => GetCharacterStats(isPlayer).Gold += amount;
        public void AddHealh(bool isPlayer,int amount)
        {
            if (amount <= 0)
            {
                RecieveDamage(isPlayer, amount);
                return;
            }
            ref CharacterStats stat = ref GetCharacterStats(isPlayer);

           // if (GetCharacterStats(isPlayer).Health + amount > GetCharacterStats(isPlayer).MaxHealth)
            if (stat.Health + amount > stat.MaxHealth)
            {
                //Text of Heal
                _updateUIStats?.Invoke(isPlayer,   stat.MaxHealth - (stat.Health), Keywords.KeywordTypeEnum.Heal);
                ResetHealth(isPlayer);
                return;
            }
            else
            {
                //Text of Heal
                stat.Health += amount;
            }
            _updateUIStats?.Invoke(isPlayer, amount, Keywords.KeywordTypeEnum.Heal);
        
        }
        public void SetNewMaxHealth(bool isPlayer, int amount)
        {
            GetCharacterStats(isPlayer).MaxHealth += amount;
            _updateUIStats?.Invoke(isPlayer, GetCharacterStats(isPlayer).MaxHealth, Keywords.KeywordTypeEnum.MaxHealth );
            AddHealh(isPlayer, amount);
        }
        public void RecieveDamage(bool isPlayer, int damageAmount)
        {
            if (damageAmount < 0)
                damageAmount = 0;

            ref CharacterStats stat = ref GetCharacterStats(isPlayer);

            if (stat.Shield > 0)
            {
                
              int remain = damageAmount - stat.Shield;

                //Damage is Blocked
                if (remain <= 0)
                {
                    // playerBlocked the comingDamange
                    stat.Shield -= damageAmount;
                    _updateUIStats?.Invoke(isPlayer, damageAmount, Keywords.KeywordTypeEnum.Defense);
                    Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(isPlayer, stat.Shield);
                }
                else
                {
                    // there is more damage than the armour
                    _updateUIStats?.Invoke(isPlayer, damageAmount - stat.Shield, Keywords.KeywordTypeEnum.Defense);
                    ResetShield(isPlayer);
                    SetHealth(isPlayer,remain);
                }


               return;
            }

            SetHealth(isPlayer,damageAmount);
        }
        public int SpendGold(bool isPlayer,int cost)
        => GetCharacterStats(isPlayer).Gold -= cost; 
        public bool HasEnoughGold(bool isPlayer, int amount)
            => GetCharacterStats(isPlayer).Gold >= amount;
        public void AddBleedPoints(bool isPlayer,int amount)
        {
            _updateUIStats?.Invoke(isPlayer, amount, Keywords.KeywordTypeEnum.Bleed);
             GetCharacterStats(isPlayer).Bleed += amount;
        }
        public void AddStrength(bool isPlayer, int amount)
        {
            _updateUIStats?.Invoke(isPlayer, amount, Keywords.KeywordTypeEnum.Strength);
            GetCharacterStats(isPlayer).Strength += amount;
        }
        public void ApplyBleed(bool isPlayer)
        {
            ref CharacterStats stats =ref GetCharacterStats(isPlayer);
            if (stats.Bleed > 0)
            {
                RecieveDamage(isPlayer,stats.Bleed);
                stats.Bleed--;
                if(stats.Bleed==0)
                {
                    _removeUIIcon?.Invoke(isPlayer, BuffIcons.Bleed);

                }
            }
        }
        private void SetHealth(bool isPlayer, int amount)
        {
            if (amount == 0)
                return;

            ref CharacterStats stats = ref GetCharacterStats(isPlayer);

            if (stats.Health - amount <= 0)
            {
                Debug.Log(string.Concat("The" + (isPlayer ? "Player" : "Enemy") + " Died!"));
                _updateUIStats.Invoke(isPlayer, 0, Keywords.KeywordTypeEnum.Attack);
                Battles.BattleManager.BattleEnded(isPlayer);
                return;
            }
            else
                stats.Health -= amount;


            _updateUIStats?.Invoke(isPlayer, amount, Keywords.KeywordTypeEnum.Attack);
        }
    }
}