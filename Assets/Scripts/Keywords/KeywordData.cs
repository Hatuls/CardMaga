﻿using CardMaga.Battle;
using Conditions;
using System;
using UnityEngine;


namespace CardMaga.Keywords
{

    [Serializable]
    public class KeywordData : IComparable<KeywordData>
    {

        public KeywordData() { }
        public KeywordData(KeywordSO keywords, TargetEnum targetEnum, int amount, int animationIndex)
        {
            _keywordBase = keywords;
            _target = targetEnum;
            _animationIndex = animationIndex;
            _amountToApply = amount;
        }
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
        public Condition GetConditions => _conditions;
        public TargetEnum GetTarget => _target;
        public int GetAmountToApply { get => _amountToApply; set => _amountToApply = value; }
        public KeywordSO KeywordSO => _keywordBase;

        #endregion



        public int CompareTo(KeywordData other)
        {
            if (_animationIndex > other.AnimationIndex)
                return 1;
            else if (_animationIndex < other.AnimationIndex)
                return -1;
            else return 0;
        }
    }



}