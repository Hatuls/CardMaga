using CardMaga.UI.Card;
using System.Collections;
using TutorialDirector;
using UnityEngine;

public class ZoomingInTapCardTutorial1Director : BaseTutorialDirector
{
    [SerializeField] FirstCardDisplayer _firstCard;

    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnEnterZoomTutorial -= StopDirector;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnEnterZoomTutorial += StopDirector;
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
        _playableDirector.Play();
    }
}
