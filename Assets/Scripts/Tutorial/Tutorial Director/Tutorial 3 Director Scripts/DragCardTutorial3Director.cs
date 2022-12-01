using CardMaga.Battle.UI;
using CardMaga.UI;
using CardMaga.UI.Card;
using TutorialDirector;
using UnityEngine;
using System.Collections;

public class DragCardTutorial3Director : BaseTutorialDirector
{
    [SerializeField] private BarrierTutorialHandler _barrierTutorialHandler;
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
        StartCoroutine(MoveDirector());
    }

    IEnumerator MoveDirector()
    {
        yield return new WaitForSeconds(0.2f);
        _directorRect.transform.position = _barrierTutorialHandler.BarrierCard.RectTransform.position;
        _playableDirector.Play();
    }

    private void CardExecute(BattleCardUI cardUI)
    {
        StopDirector();
    }
}
