using CardMaga.Keywords;
using Keywords;
using System;
using UnityEngine;
namespace CardMaga.AI
{
    [CreateAssetMenu(fileName = "New AI Brain", menuName ="ScriptableObjects/AI/Brain")]
    public class AIBrain : ScriptableObject
    {
        [SerializeField]
        private HealthParameter _sectionA;

        [SerializeField]
        private ContainKeyword _sectionB;

        [SerializeField]
        private WeightAdditionForCardsAndCombos _sectionC;

        [SerializeField]
        private WeightAdditionForCardsAndCombos _sectionD;
        [SerializeField]
        private HealthParameter _sectionE;

        [SerializeField]
        private KeywordParameter _sectionF;

        [SerializeField]
        private WeightAdditionForCardsAndCombos _sectionG;

        [SerializeField]
        private HealthParameter _sectionH;

        [SerializeField]
        private KeywordParameter _sectionI;

        [SerializeField]
        private WeightAdditionForCardsAndCombos _sectionJ;

        [SerializeField]
        private HealthParameter _sectionK;

        [SerializeField]
        private KeywordParameter _sectionL;

        [SerializeField]
        private WeightAdditionForCardsAndCombos _sectionM;

        [SerializeField]
        private ValueComparer _sectionN;

        #region Properties
        public HealthParameter SectionA { get => _sectionA;  }
        public ContainKeyword SectionB { get => _sectionB;  }
        public WeightAdditionForCardsAndCombos SectionC { get => _sectionC; }
        public WeightAdditionForCardsAndCombos SectionD { get => _sectionD; }
        public HealthParameter SectionE { get => _sectionE; }
        public KeywordParameter SectionF { get => _sectionF; }
        public WeightAdditionForCardsAndCombos SectionG { get => _sectionG; }
        public HealthParameter SectionH { get => _sectionH; }
        public KeywordParameter SectionI { get => _sectionI; }
        public WeightAdditionForCardsAndCombos SectionJ { get => _sectionJ;}
        public HealthParameter SectionK { get => _sectionK;  }
        public KeywordParameter SectionL { get => _sectionL; }
        public WeightAdditionForCardsAndCombos SectionM { get => _sectionM; }
        public ValueComparer SectionN { get => _sectionN; }
        #endregion
    }


    [Serializable]
    public class HealthParameter
    {
        public ValueComparer HealthParams;
        public WeightAdditionForCardsAndCombos Weights;

    }
    [Serializable]
    public class ValueComparer
    {
        public OperatorType MathOperation;
        public float Amount;
    }
    [Serializable]
    public class KeywordParameter
    {
        public ValueComparer Values;
        public ContainKeyword Keyword;
    }
    [Serializable]
    public class ContainKeyword
    {
        public KeywordType Keyword;
        public WeightAdditionForCardsAndCombos Weights;
    }
    [Serializable]
    public class WeightAdditionToTypes
    {
        public int AttackWeightAddition;
        public int DefenseWeightAddition;
        public int UtilityWeightAddition;
    }
    [Serializable]
    public class WeightAdditionForCardsAndCombos
    {
        public WeightAdditionToTypes Combos;
        public WeightAdditionToTypes Cards;
    }

}