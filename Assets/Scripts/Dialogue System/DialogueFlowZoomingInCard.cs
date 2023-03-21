using CardMaga.UI.Card;

public class DialogueFlowZoomingInCard : DialoguesFlow
{
    protected override void UnsubscribeEvent()
    {
        ZoomCardUI.OnEnterZoomTutorial -= EndFlow;
    }
    protected override void SubscribeEvent()
    {
        ZoomCardUI.OnEnterZoomTutorial += EndFlow;
    }
}
