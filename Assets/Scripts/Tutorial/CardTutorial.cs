using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class CardTutorial : TutorialAbst
    {
        [SerializeField]
        GameObject _cardContainer;
        public override void EndTutorial()
        {
            _cardContainer.SetActive(false);
            base.EndTutorial();
        }
        public override void StartTutorial()
        {
            base.StartTutorial();
            _cardContainer.SetActive(true);
        }
    }
}
