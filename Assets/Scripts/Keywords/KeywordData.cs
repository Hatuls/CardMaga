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

        [Tooltip("Who is affected by it")]
        [SerializeField] TargetEnum _target;

        [Tooltip("The amount to apply:")]
        [SerializeField] int _amountToApply;

        [Tooltip("On which animation event on the animation should it be activate:")]
        [SerializeField] int _animationIndex;

        [Header("Conditions:")]
        [Tooltip("Is There Conditions that first need to be met?")]
        [SerializeField] Condition _conditions;

        #endregion
        #region Properties
        public int AnimationIndex => _animationIndex;
        public Condition GetConditions=> _conditions;
        public  TargetEnum GetTarget => _target;
        public int GetAmountToApply { get => _amountToApply; set => _amountToApply = value; }
        public KeywordSO GetKeywordSO => _keywordBase;
        #endregion

    }
}