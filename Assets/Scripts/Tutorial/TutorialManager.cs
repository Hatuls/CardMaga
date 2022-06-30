using Battle.Turns;
using UnityEngine;

namespace Tutorial
{




    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        TutorialPage[] _tutorials;
        [SerializeField]
        TutorialPage[] _detailedTutorial;
        [SerializeField]
        int _currentPage = 0;
        [SerializeField]
        [HideInInspector]
        TutorialPage _currentTutorial;

        [SerializeField]
        GameObject _container;

        private void Awake()    // Need To be Re-Done
        {
            //if (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
            //{
            //    TurnHandler.OnTurnCountChange += TurnCounter;
            //}
        }
        private void OnDisable()
        {
            TurnHandler.OnTurnCountChange -= TurnCounter;
        }
        private void ResetAllPages()
        {
            for (int i = 0; i < _tutorials.Length; i++)
                _tutorials[i].EndTutorial();
            for (int i = 0; i < _detailedTutorial.Length; i++)
                _detailedTutorial[i].EndTutorial();


        }
        private void TurnCounter(int count)
        {
            if (count == 1 || count == 2)
            {
                ResetAllPages();
                _container.SetActive(true);
                _currentTutorial = _tutorials[count - 1];
                StartTutorial(_currentTutorial);
            }

        }
        public void OpenTutorial(int tutorial)
        {
            ResetAllPages();
            _container.SetActive(true);
            _currentTutorial = _tutorials[tutorial];
            StartTutorial(_currentTutorial);
        }
        public void OpenDetailedTutorial(int tutorial)
        {
            ResetAllPages();
            _container.SetActive(true);
            _currentTutorial = _detailedTutorial[tutorial];
            StartTutorial(_currentTutorial);

        }
        private void StartTutorial(TutorialPage currentTutorial)
        {
            _currentPage = 0;
            currentTutorial.StartTutorial();
        }


        public void MovePagesWithArrows(int goToPage)
        {
            _currentPage += goToPage;
            _currentTutorial.SetPages(_currentPage);

        }

        public void CloseTutorial()
        {
            _currentTutorial.EndTutorial();
            _container.SetActive(false);
        }
    }





    public class TutorialMainMenu : MonoBehaviour
    {
    }




    //public class ConditionChecker<T> : IConditionsCheck<T>
    //{
    //    [SerializeField]
    //    IConditionCheck<T>
    //    public IConditionCheck<T>[] ConditionChecks => throw new System.NotImplementedException();

    //}

    public class UshortCondition : IConditionCheck<ushort>
    {
        [SerializeField]
        ushort _valueCompareTo;
        [SerializeField]
        CompareOperation compareOperation;
        public ushort ValueToMatch => _valueCompareTo;
        public bool IsConditionMet(ushort val)
        => ValueToMatch.EqualTo(compareOperation, val);

    }
    [System.Serializable]
    public class IntCondition : IConditionCheck<int>
    {
        [SerializeField]
        int _valueCompareTo;
        [SerializeField]
        CompareOperation compareOperation;
        public int ValueToMatch => _valueCompareTo;
        public bool IsConditionMet(int val)
        => ValueToMatch.EqualTo(compareOperation, val);

    }
    public interface IConditionCheck<T>
    {
        T ValueToMatch { get; }
        bool IsConditionMet(T val);

    }





    public enum CompareOperation
    {
        Equal = 0,
        Not_Equal = 1,
        BiggerThan = 2,
        BiggerOrEqualTo = 3,
        SmallerThan = 4,
        SmallerOrEqualTo = 5,
    }
    public static class ComparerHelper
    {
        public static bool EqualTo(this ushort value, CompareOperation compareMethod, ushort otherValue)
        {
            switch (compareMethod)
            {
                case CompareOperation.Equal:
                    return value == otherValue;

                case CompareOperation.Not_Equal:
                    return value != otherValue;

                case CompareOperation.BiggerThan:
                    return value > otherValue;

                case CompareOperation.BiggerOrEqualTo:
                    return value >= otherValue;

                case CompareOperation.SmallerThan:
                    return value < otherValue;

                case CompareOperation.SmallerOrEqualTo:
                    return value <= otherValue;

                default:
                    return false;
            }
        }
        public static bool EqualTo(this int value, CompareOperation compareMethod, int otherValue)
        {
            switch (compareMethod)
            {
                case CompareOperation.Equal:
                    return value == otherValue;

                case CompareOperation.Not_Equal:
                    return value != otherValue;

                case CompareOperation.BiggerThan:
                    return value > otherValue;

                case CompareOperation.BiggerOrEqualTo:
                    return value >= otherValue;

                case CompareOperation.SmallerThan:
                    return value < otherValue;

                case CompareOperation.SmallerOrEqualTo:
                    return value <= otherValue;

                default:
                    return false;
            }
        }



    }
}

