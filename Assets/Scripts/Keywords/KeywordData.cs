using System;
using UnityEngine;
using Conditions;

namespace Keywords
{

    [Serializable]
    public class KeywordData 
    {

        
        #region Fields
        [Header("Keyword :")]
        [Tooltip("KeywordData")]
        [SerializeField] KeywordSO _keywordBase;
        
        [Tooltip("is activated only if its the last card")]
        [SerializeField] bool _isLCE;

        [Tooltip("Who is affected by it")]
        [SerializeField] TargetEnum _target;

        [Tooltip("The amount to apply:")]
        [SerializeField] int _amountToApply;

        [Tooltip("When Upgraded, Add This Amount:")]
        [SerializeField]int _upgradeBonusAmount;

        [Tooltip("How Many Times This Card Can Be Upgraded")]
        [SerializeField] int _maxUpgradeLevel = 1 ;


        [Header("Conditions:")]
        [Tooltip("Is There Conditions that first need to be met?")]
        [SerializeField] Condition _conditions;


        #endregion
        #region Properties
        public Condition GetConditions=> _conditions;
        public  TargetEnum GetTarget => _target;
        public int GetAmountToApply => _amountToApply;
        public int GetUpgradedAmount =>  _upgradeBonusAmount;
        public bool GetIsLCE => _isLCE;
        public KeywordSO GetKeywordSO => _keywordBase;
        #endregion

    }
}