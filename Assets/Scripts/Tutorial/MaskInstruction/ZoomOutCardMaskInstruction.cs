using CardMaga.UI.Card;

public class ZoomOutCardMaskInstruction : BaseMaskInstruction
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnZoomOutTutorial -= CloseCanvas;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnZoomOutTutorial += CloseCanvas;
    }
}
