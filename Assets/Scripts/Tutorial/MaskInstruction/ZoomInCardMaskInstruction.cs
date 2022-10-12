using CardMaga.UI.Card;

public class ZoomInCardMaskInstruction : BaseMaskInstruction
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnZoomInTutorial -= CloseCanvas;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnZoomInTutorial += CloseCanvas;
    }
}
