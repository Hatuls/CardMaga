using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class BattleCardUIScrollPanelManager : BaseScrollPanelManager<BattleCardUI,BattleCardData>
    {
        [SerializeField] private CardUiPool _cardUiPool;

        protected override BasePoolObject<BattleCardUI, BattleCardData> ObjectPool
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

