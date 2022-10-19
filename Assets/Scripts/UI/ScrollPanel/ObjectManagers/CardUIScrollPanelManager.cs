using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class CardUIScrollPanelManager : BaseScrollPanelManager<CardUI,CardData>
    {
        [SerializeField] private CardUiPool _cardUiPool;
    
        public override BasePoolObject<CardUI, CardData> ObjectPool
        {
            get => _cardUiPool;
        }
        
        #region Testing
#if UNITY_EDITOR
        [Header("Testing")]
        [SerializeField] private CardData[] _cardDatas;
    
        [ContextMenu("Test Add Card to scroll panel")]
        public void TestAddCard()
        {
            AddObjectToPanel(_cardDatas);
        }  
        
        [ContextMenu("Test remove Card from scroll panel")]
        public void TestRemoveCard()
        {
            //RemoveCardUIFromPanel(_cardDatas);
        } 
        
        [ContextMenu("Test remove all Card from scroll panel")]
        public void TestRemoveAllCard()
        {
            RemoveAllObjectsFromPanel();
        }  
#endif
        #endregion
        
    }
}

