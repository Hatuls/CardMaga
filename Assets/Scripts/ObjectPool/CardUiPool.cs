using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

public class CardUiPool : MonoBehaviour
{
    [SerializeField] private CardUI _cardUIPrefab;
    [SerializeField] private RectTransform _parent;

    private ObjectPool<CardUI> _cardUiPool;

    private void Awake()
    {
        _cardUiPool = new ObjectPool<CardUI>(_cardUIPrefab, _parent);
    }

    public List<CardUI> GetCardUIs(params CardData[] cardDatas)
    {
        if (cardDatas == null || cardDatas.Length == 0)
            return null;
            
        List<CardUI> output = new List<CardUI>(cardDatas.Length);

        for (int i = 0; i < cardDatas.Length; i++)
        {
            CardUI cache = _cardUiPool.Pull();
            
            cache.AssignCard(cardDatas[i]);
            
            output.Add(cache);
        }

        return output;
    }
}
