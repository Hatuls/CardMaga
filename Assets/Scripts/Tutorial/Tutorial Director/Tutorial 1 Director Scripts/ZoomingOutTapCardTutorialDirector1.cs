using System.Collections;
using System.Collections.Generic;
using TutorialDirector;
using UnityEngine;

public class ZoomingOutTapCardTutorialDirector1 : BaseTutorialDirector
{
    [SerializeField] FirstCardDisplayer _firstCard;

    protected override void UnsubscribeEvent()
    {
        _firstCard.OnZoomingOutCard -= StopDirector;
    }
    protected override void SubscribeEvent()
    {
        _firstCard.OnZoomingOutCard += StopDirector;
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
