using System.Collections.Generic;
using System.Linq;
using CardMaga.Card;

public class CardDataSort
{
   public List<CardData> SortCardData(List<CardData> cardDatas)
   {
      List<CardData> output = cardDatas;
      
      output.OrderBy(cardData => cardData.CardTypeData.CardType)
         .ThenBy(cardData => cardData.CardSO.Rarity).ThenBy(cardData => cardData.CardSO.CardName).ToList();

      return output;
   } 
   
}
