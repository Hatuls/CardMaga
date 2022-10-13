using System.Linq;
using CardMaga.Card;


public class TestFilter : BaseFilter<CardData>
{
    public override bool Filter(CardData obj)
    {
        if (obj.CardSO.CardType.CardType == CardTypeEnum.Attack)
        {
            return true;
        }
        
        obj.

        return false;
    }
}
