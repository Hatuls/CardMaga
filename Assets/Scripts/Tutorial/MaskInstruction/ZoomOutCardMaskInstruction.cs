using CardMaga.UI.Card;

public class ZoomOutCardMaskInstruction : BaseMaskInstruction
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnExitZoomTutorial -= CloseCanvas;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnExitZoomTutorial += CloseCanvas;
    }
}
