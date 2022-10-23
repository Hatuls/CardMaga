using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class CardUIScrollPanelManager : BaseScrollPanelManager<CardUI,CardData>
    {
        [SerializeField] private CardUiPool _cardUiPool;

        protected override BasePoolObject<CardUI, CardData> ObjectPool
        {
            get => _cardUiPool;
        }

        public override void Init()
        {
            base.Init();
            _cardUiPool.Init();
        }

        private void OnDestroy()
        {
            RemoveAllObjectsFromPanel();
        }
        
    }
}

