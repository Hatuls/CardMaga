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
        private WeightAdditionToTypes _sectionC;

        [SerializeField]
        private WeightAdditionToTypes _sectionD;
        [SerializeField]
        private HealthParameter _sectionE;

        [SerializeField]
        private KeywordParameter _sectionF;

        [SerializeField]
        private WeightAdditionToTypes _sectionG;

        [SerializeField]
        private HealthParameter _sectionH;

        [SerializeField]
        private KeywordParameter _sectionI;

        [SerializeField]
        private WeightAdditionToTypes _sectionJ;

        [SerializeField]
        private HealthParameter _sectionK;

        [SerializeField]
        private KeywordParameter _sectionL;

        [SerializeField]
        private WeightAdditionToTypes _sectionM;

        [SerializeField]
        private ValueComparer _sectionN;

        #region Properties
        public HealthParameter SectionA { get => _sectionA;  }
        public ContainKeyword SectionB { get => _sectionB;  }
        public WeightAdditionToTypes SectionC { get => _sectionC; }
        public WeightAdditionToTypes SectionD { get => _sectionD; }
        public HealthParameter SectionE { get => _sectionE; }
        public KeywordParameter SectionF { get => _sectionF; }
        public WeightAdditionToTypes SectionG { get => _sectionG; }
        public HealthParameter SectionH { get => _sectionH; }
        public KeywordParameter SectionI { get => _sectionI; }
        public WeightAdditionToTypes SectionJ { get => _sectionJ;}
        public HealthParameter SectionK { get => _sectionK;  }
        public KeywordParameter SectionL { get => _sectionL; }
        public WeightAdditionToTypes SectionM { get => _sectionM; }
        public ValueComparer SectionN { get => _sectionN; }
        #endregion
    }


    [Serializable]
    public class HealthParameter
    {
        public ValueComparer HealthParams;
        public WeightAdditionToTypes Weights;

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
        public KeywordTypeEnum Keyword;
        public WeightAdditionToTypes Weights;
    }
    [Serializable]
    public class WeightAdditionToTypes
    {
        public int AttackWeightAddition;
        public int DefenseWeightAddition;
        public int UtilityWeightAddition;
    }
}