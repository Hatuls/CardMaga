using Battles.Turns;
using UnityEngine;

namespace Tutorial
{
    public enum TutorialType
    {
        Card = 0,
        Combo = 1,
    }




    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        TutorialPage[] _tutorials;

        [SerializeField]
        int _currentPage = 0;
        [SerializeField]
        [HideInInspector]
        TutorialPage _currentTutorial;
        [SerializeField]
        GameObject _exitBtn;
        [SerializeField]
        GameObject _goToLeftBtn;
        [SerializeField]
        GameObject _goToRightBtn;
        [SerializeField]
        GameObject _container;

        private void Awake()
        {
            if (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
            {
                TurnHandler.OnTurnCountChange += TurnCounter;
            }
        }
        private void OnDestroy()
        {
            TurnHandler.OnTurnCountChange -= TurnCounter;
        }
        private void ResetAllPages()
        {
            for (int i = 0; i < _tutorials.Length; i++)
                            _tutorials[i].EndTutorial();
            
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
        private void StartTutorial(TutorialPage currentTutorial)
        {
            _exitBtn.SetActive(false);
            _currentPage = 0;
            currentTutorial.StartTutorial();
        }

        public void ArrowsActivation(int currentPage)
        {
            _goToLeftBtn.SetActive(currentPage != 0);
            _goToRightBtn.SetActive(_currentTutorial.PageLength - 1 != currentPage);
        }
        public void MovePagesWithArrows(int goToPage)
        {
            _currentPage += goToPage;
            _currentTutorial.SetPages(_currentPage);

        }
        public void ShowCloseButton() => _exitBtn.SetActive(true);
        public void CloseTutorial()
        {
            _currentTutorial.EndTutorial();
            _container.SetActive(false);
        }
    }
}

