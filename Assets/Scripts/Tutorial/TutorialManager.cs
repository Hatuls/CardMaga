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
        CardTutorial _cardTutorial;
        [SerializeField]
        ComboTutorial _comboTutorial;
        TutorialAbst _currentTutorial;

        int _currentPage = 0;

        [SerializeField]
        GameObject _exitBtn;
        [SerializeField]
        GameObject _goToLeftBtn;
        [SerializeField]
        GameObject _goToRightBtn;
        [SerializeField]
        TutorialType _currentTutorialEnum = TutorialType.Card;
        public void StartTutorial()
        {
            _currentTutorialEnum = TutorialType.Card;
            StartTutorial(_currentTutorialEnum);
            _exitBtn.SetActive(false);

            _goToLeftBtn.SetActive(false);

            _cardTutorial.gameObject.SetActive(true);
            gameObject.SetActive(true);

        }
        private void StartTutorial(TutorialType type)
        {
            _currentPage = 0;
            switch (type)
            {
                case TutorialType.Card:
                    _comboTutorial.gameObject.SetActive(false);
                    _comboTutorial.ResetPages();
                    _currentTutorial = _cardTutorial;
                    _cardTutorial.StartTutorial();

                    break;
                case TutorialType.Combo:
                    _cardTutorial.gameObject.SetActive(false);
                    _cardTutorial.ResetPages();
                    _currentTutorial = _comboTutorial;
                    _comboTutorial.StartTutorial();
                    break;
                default:
                    break;
            }
        }

        public void ChangePage(int value)
        {
            _exitBtn.SetActive(false);
            _currentPage += value;
            _goToLeftBtn.SetActive(true);
            _goToRightBtn.SetActive(true);


            if (_currentPage <= 0 && _currentTutorialEnum == TutorialType.Card)
            {
                _goToLeftBtn.SetActive(false);
                _currentPage = 0;

            }
            else if (_currentPage < 0 && _currentTutorialEnum == TutorialType.Combo)
            {
                _currentTutorialEnum = TutorialType.Card;
                StartTutorial(_currentTutorialEnum);
                _currentPage = _currentTutorial.PageLength - 1;

            }
            else if (_currentPage == _currentTutorial.PageLength && _currentTutorialEnum == TutorialType.Card)
            {
                _currentTutorialEnum = TutorialType.Combo;
                StartTutorial(_currentTutorialEnum);
                return;
            }
            else if (_currentPage >= _currentTutorial.PageLength && _currentTutorialEnum == TutorialType.Combo)
            {
                _goToRightBtn.SetActive(false);
                _exitBtn.SetActive(true);
                return;
            }


            _currentTutorial.SetPages(_currentPage);
        }
        public void CloseTutorial() => gameObject.SetActive(false);
    }
}

