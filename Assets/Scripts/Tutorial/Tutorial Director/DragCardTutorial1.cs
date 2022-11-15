using CardMaga.UI;
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
            StartCoroutine(WaitFrame());
        }
        
        IEnumerator WaitFrame()
        {
            yield return null;
            yield return null;
            _directorRect.transform.position = _firstCard.FirstCard[0].RectTransform.GetWorldPosition();
            Debug.Log(_directorRect.rect);
        }

        private void CardExecute(BattleCardUI cardUI)
        {
            StopDirector();
        }
    }

}
