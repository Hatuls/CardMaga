using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Test : Locomotion
{
    void Start()
    {
        rectTransform.DOAnchorPos(Motions[0]._dis, Motions[0].timeToTransition).SetEase(Motions[0].Ease);
        rectTransform.DOSizeDelta(Motions[1]._dis, Motions[1].timeToTransition).SetEase(Motions[1].Ease);
    }
}
