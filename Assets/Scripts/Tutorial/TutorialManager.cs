using System.Collections;
using System.Collections.Generic;
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

        public void StartTutorial(TutorialType type)
        {
            switch (type)
            {
                case TutorialType.Card:
                    _currentTutorial = _cardTutorial;
                    _cardTutorial.StartTutorial();
                       
                    break;
                case TutorialType.Combo:
                    _currentTutorial = _comboTutorial;
                    _comboTutorial.StartTutorial();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// true = right, false = left
        /// </summary>
        /// <param name="directon"></param>
        public void ChangePageRight(bool directon) 
        {
            _currentTutorial.ChangePageRight(directon);
        }
    }
}

