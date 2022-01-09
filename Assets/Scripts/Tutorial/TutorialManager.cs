using Battles.Turns;
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

        private void Awake()
        {
            if (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
            {
                TurnHandler.OnTurnCountChange += TurnCounter;
            }
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
            if (count == 1|| count ==2)
            {
                ResetAllPages();
                _container.SetActive(true);
                _currentTutorial = _tutorials[count-1];
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
}

