
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Characters.Stats;

namespace Keywords
{
    public class KeywordManager : MonoSingleton<KeywordManager>
    {


        #region Fields
        private static Dictionary<KeywordTypeEnum, KeywordAbst> _keywordDict;

        #endregion






        #region public Functions


        public override void Init()
        {
            InitParams();
        }

  
        public void ActivateKeyword(bool isPlayerTurn, KeywordData keyword)
        {
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
                keywordEffect.ProcessOnTarget(isPlayerTurn, keyword);
            }


        }


        public IEnumerator OnStartTurnKeywords(bool isPlayer)
        {
            Debug.Log("Activating Keywords Effect on " + (isPlayer? "Player":"Enemy") + " that are activated on the start of the turn");
           
            CharacterStatsManager.GetCharacterStatsHandler(isPlayer).ApplyBleed();
            CharacterStatsManager.GetCharacterStatsHandler(isPlayer).ApplyHealRegeneration();

            yield return Battles.Turns.Turn.WaitOneSecond;
        }


        public IEnumerator OnEndTurnKeywords(bool isPlayer)
        {
          
            Debug.Log("Activating Keywords Effect on " + (isPlayer ? "Player" : "Enemy") + " that are activated on the end of the turn");
        
            
            //     var statCache = GetCharacterStats(targetEnum);
            yield return Battles.Turns.Turn.WaitOneSecond;

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
                    {KeywordTypeEnum.StaminaShards, new StaminaShardKeyword() }
            };
            }
            if (_keywordDict == null)
                Debug.LogError("Keyword Manager: Dictionary of keywords was not assigned!");
        }
        #endregion

    }

    public enum KeywordTypeEnum
    {
        None =0,
        Attack = 1,
        Shield= 2,
        Heal = 3 ,
        Strength =4,
        Bleed = 5,
        MaxHealth = 6,
        Interupt =7,
        Weak = 8,
        Vulnerable = 9,
        Fatigue = 10,
        Regeneration = 11,
        Dexterity =12,
        Draw = 13,
        MaxStamina = 14,
        Coins=18,
        StaminaShards = 19,
        Stamina = 22,
        Clear = 29,
        Shuffle = 31,
    };

}