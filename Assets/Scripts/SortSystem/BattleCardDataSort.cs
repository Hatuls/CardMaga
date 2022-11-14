using System.Collections.Generic;
using System.Linq;
using CardMaga.Card;

public class BattleCardDataSort
{
   public List<BattleCardData> SortCardData(IEnumerable<BattleCardData> cardDatas)
   {
        var cardTypeOrder = cardDatas.OrderBy(cardData => cardData.CardTypeData.CardType);

        var rarityOrder = cardTypeOrder.ThenBy(cardData => cardData.CardSO.Rarity);

        var nameOrder = rarityOrder.ThenBy(cardData => cardData.CardSO.CardName).ToList();

        return nameOrder;
   }
}
