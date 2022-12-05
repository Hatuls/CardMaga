using CardMaga.Battle.UI;
using CardMaga.UI;
using CardMaga.UI.Card;
using TutorialDirector;
using UnityEngine;
using System.Collections;
using TutorialCardDrawn;
using CardMaga.Battle;

public class DragCardTutorial3Director : BaseTutorialDirector
{
    [SerializeField] private TutorialCardDrawnHandler _tutorialCardDrawnHandler;
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
        BattleManager.Instance.BattleUIManager.HandUI.HandUIState.OnAllCardsDrawnAndAlign += MoveDirector;
    }

    private void MoveDirector()
    {
        _directorRect.transform.position = _tutorialCardDrawnHandler.DrawnCard.RectTransform.position;
        _playableDirector.Play();
    }

    private void CardExecute(BattleCardUI cardUI)
    {
        StopDirector();
    }
}
