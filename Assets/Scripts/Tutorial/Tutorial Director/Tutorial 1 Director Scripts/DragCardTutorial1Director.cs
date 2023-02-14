using CardMaga.Battle.UI;
using CardMaga.UI;
using CardMaga.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TutorialDirector
{
    public class DragCardTutorial1Director : BaseTutorialDirector
{
        [SerializeField] FirstCardDisplayer _firstCard;

        protected override void UnsubscribeEvent()
        {
            HandUI.OnCardExecute -= CardExecute;
            HandUIState.OnCardDrawnAndAlign -= SetLastSibling;
        }
        protected override void SubscribeEvent()
        {
            HandUI.OnCardExecute += CardExecute;
        }

        protected override void MoveDirectorPosition()
        {
           HandUIState.OnCardDrawnAndAlign += MoveDirectorAfterCardPositionReturnedFromZoom;
            HandUIState.OnCardDrawnAndAlign += SetLastSibling;
            MoveDirectorAfterCardPositionReturnedFromZoom();
        }
        
        private void MoveDirectorAfterCardPositionReturnedFromZoom()
        {
            var parent = transform.parent;
            _directorRect.SetParent(_firstCard.GetFirstCard.transform, false);
            _directorRect.localPosition = Vector3.zero;
            _directorRect.SetParent(parent);
       //     _directorRect.transform.position = .RectTransform.GetWorldPosition();// get position
            HandUIState.OnCardDrawnAndAlign -= MoveDirectorAfterCardPositionReturnedFromZoom;
        }

        private void CardExecute(BattleCardUI cardUI)
        {
            StopDirector();
        }

        private void SetLastSibling()
        {
            _directorRect.transform.SetAsLastSibling();
        }
    }

}
