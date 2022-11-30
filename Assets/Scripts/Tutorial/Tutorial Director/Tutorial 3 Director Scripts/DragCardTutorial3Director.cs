using CardMaga.Battle.UI;
using CardMaga.UI;
using CardMaga.UI.Card;
using TutorialDirector;
using UnityEngine;

public class DragCardTutorial3Director : BaseTutorialDirector
{
    [SerializeField]BarrierTutorialHandler barrierTutorialHandler;
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
        _directorRect.transform.position = barrierTutorialHandler.BarrierCard.RectTransform.position;
    }

    private void CardExecute(BattleCardUI cardUI)
    {
        StopDirector();
    }
}
