using CardMaga.UI.Card;
using UnityEngine;

public class ZoomInCardStartingMaskInstruction : BaseMaskInstruction
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnZoomInLocation -= CloseCanvas;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnZoomInLocation += CloseCanvas;
    }
}
