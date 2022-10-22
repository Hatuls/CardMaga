using CardMaga.UI.Card;
using UnityEngine;

public class ZoomInCardStartingMaskInstruction : BaseMaskInstruction
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnZoomInLocation -= SubscribeEvent;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnZoomInLocation += SubscribeEvent;
    }
}
