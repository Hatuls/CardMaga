using UnityEngine;
using System.Linq;
using CardMaga.Card;

public class CardDataSort : MonoBehaviour
{
    private CardData[] _cardDatas = new CardData[3];

    private void Awake()
    {
        IOrderedEnumerable<CardData> carddata = _cardDatas.OrderBy(cardData => cardData.CardTypeData.CardType)
            .ThenBy(cardData => cardData.CardKeywords).ThenBy(cardData => cardData.CardLevel);
    }
}
