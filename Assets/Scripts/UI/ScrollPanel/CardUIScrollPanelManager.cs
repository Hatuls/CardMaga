using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class CardUIScrollPanelManager : MonoBehaviour
    {
        [SerializeField] private ScrollPanelHandler _scrollPanel;
        [SerializeField] private CardUiPool _cardUiPool;
    
        #region Testing
#if UNITY_EDITOR
        [Header("Testing")]
        [SerializeField] private CardData[] _cardDatas;
    
        [ContextMenu("Test Add Card to scroll panel")]
        public void TestAddCard()
        {
            AddCardUIToPanel(_cardDatas);
        }  
        
        [ContextMenu("Test remove Card from scroll panel")]
        public void TestRemoveCard()
        {
            //RemoveCardUIFromPanel(_cardDatas);
        } 
        
        [ContextMenu("Test remove all Card from scroll panel")]
        public void TestRemoveAllCard()
        {
            RemoveAllLoadedCardUI();
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
    
        public void AddCardUIToPanel(params CardUI[] cardUI)
        {
            IShowableUI[] showableUis = new IShowableUI[cardUI.Length];

            for (int i = 0; i < cardUI.Length; i++)
            {
                showableUis[i] = cardUI[i];
            }
        
            _scrollPanel.LoadObject(showableUis);
        }

        public void RemoveCardUIFromPanel(params CardUI[] cardUI)
        {
            IShowableUI[] showableUis = new IShowableUI[cardUI.Length];

            for (int i = 0; i < cardUI.Length; i++)
            {
                showableUis[i] = cardUI[i];
            }
        
            _scrollPanel.UnLoadObjects(showableUis);
        }

        public void RemoveAllLoadedCardUI()
        {
            _scrollPanel.UnLoadAllObjects();
        }
    }
}

