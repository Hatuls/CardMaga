﻿using CardMaga.UI;
using CardMaga.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TutorialDirector
{
    public class DragCardTutorial1 : BaseTutorialDirector
{
        [SerializeField] FirstCardDisplayer _firstCard;

        protected override void UnsubscribeEvent()
        {
            HandUI.OnCardExecute -= CardExecute;
        }
        protected override void SubscribeEvent()
        {
            HandUI.OnCardExecute += CardExecute;
        }

        protected override void MoveDirectorPosition()
        {
            _directorRect.transform.position= _firstCard.FirstCard[0].RectTransform.GetWorldPosition();
            Debug.Log(_directorRect.rect);
        }

        private void CardExecute(CardUI cardUI)
        {
            StopDirector();
        }
    }

}
