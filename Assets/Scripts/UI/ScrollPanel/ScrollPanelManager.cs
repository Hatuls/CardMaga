using System.Collections;
using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

public class ScrollPanelManager : MonoBehaviour
{
    [SerializeField] private ScrollPanelHandler _scrollPanel;
    [SerializeField] private CardUiPool _cardUiPool;


    #region Testing
#if UNITY_EDITOR
    [Header("Testing")]
    [SerializeField] private CardData[] _cardDatas;
    
    [ContextMenu("Test Add Card to scroll panel")]
    public void Test()
    {
        AddCardUIToPanel(_cardDatas);
    }    
#endif
    #endregion


    
    public void AddCardUIToPanel(params CardData[] cardDatas)
    {
        List<CardUI> cache = _cardUiPool.GetCardUIs(cardDatas);
        
        IShowableUI[] showableUis = new IShowableUI[cache.Count];

        for (int i = 0; i < cache.Count; i++)
        {
            showableUis[i] = cache[i];
        }
        
        _scrollPanel.LoadObject(showableUis);
    }
}
