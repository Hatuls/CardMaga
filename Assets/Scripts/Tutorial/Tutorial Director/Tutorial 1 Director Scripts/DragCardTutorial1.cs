using CardMaga.Battle.UI;
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
            BattleUiManager.Instance.HandUI.HandUIState.OnCardDrawnAndAlign += MoveDirectorAfterCardPositionReturnedFromZoom;
        }
        
        private void MoveDirectorAfterCardPositionReturnedFromZoom()
        {
            _directorRect.transform.position = _firstCard.FirstCard[0].RectTransform.GetWorldPosition();
            BattleUiManager.Instance.HandUI.HandUIState.OnCardDrawnAndAlign -= MoveDirectorAfterCardPositionReturnedFromZoom;
        }

        private void CardExecute(BattleCardUI cardUI)
        {
            StopDirector();
        }
    }

}
