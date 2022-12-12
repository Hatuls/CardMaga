using CardMaga.UI.Card;
using UnityEngine;
public class DialogueFlowZoomingOutCard : DialoguesFlow
{
    [SerializeField] FirstCardDisplayer _firstCard;
    protected override void UnsubscribeEvent()
    {
        _firstCard.OnZoomingOutCard -= EndFlow;
    }
    protected override void SubscribeEvent()
    {
        _firstCard.OnZoomingOutCard += EndFlow;
    }
}
