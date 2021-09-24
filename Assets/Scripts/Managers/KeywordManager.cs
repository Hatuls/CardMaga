
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

           StatsHandler.GetInstance.ApplyBleed(isPlayer);
            yield return new WaitForSeconds(1f);

            
        }


        public IEnumerator OnEndTurnKeywords(bool isPlayer)
        {
          
            Debug.Log("Activating Keywords Effect on " + (isPlayer ? "Player" : "Enemy") + " that are activated on the end of the turn");
        
            
            //     var statCache = GetCharacterStats(targetEnum);
            yield return new WaitForSeconds(1f);

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
                {KeywordTypeEnum.Defense , new DefenseKeyword() },
                {KeywordTypeEnum.Strength , new StrengthKeyword() },
                {KeywordTypeEnum.Bleed , new BleedKeyword() },
                {KeywordTypeEnum.Stamina, new StaminaKeyword()}
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
        Defense= 2,
        Heal = 3 ,
        Strength =4,
        Bleed = 5,
        MaxHealth =6,
        Stamina = 23,
    };

}