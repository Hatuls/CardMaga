using CardMaga.UI.Card;
namespace CardMaga.Input
{
    public class CardUIInputHandler : TouchableItem<CardUI>
    {
        private void OnEnable()
        {
            ForceChangeState(false);
        }

    }
}