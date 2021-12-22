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
        int _currentTutorial = 0;
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

        private void TurnCounter(int count)
        {
            if (count == 0)
            {
                _currentTutorial = count;
                StatTutorial(_tutorials[_currentTutorial]);
            }

        }
        private void StatTutorial(TutorialPage currentTutorial)
        {
            _currentPage = 0;
            currentTutorial.StartTutorial();
        }
        public void CloseTutorial() => _container.SetActive(false);


        public void ArrowsActivation(int currentPage)
        {
            _goToLeftBtn.SetActive(currentPage != 0);
            _goToRightBtn.SetActive(_tutorials[_currentTutorial].PageLength - 1 != currentPage);
        }
    }
}

