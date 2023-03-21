using CardMaga.UI.Card;

public class ZoomOutCardMaskInstruction : BaseMaskInstruction
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnEnterZoomTutorial -= CloseCanvas;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnEnterZoomTutorial += CloseCanvas;
    }
}
