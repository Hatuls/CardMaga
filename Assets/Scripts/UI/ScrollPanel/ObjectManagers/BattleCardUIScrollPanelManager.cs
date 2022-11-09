using System.Collections.Generic;
using CardMaga.Card;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class BattleCardUIScrollPanelManager : BaseScrollPanelManager<BattleCardUI,BattleCardData>
    {
        [SerializeField] private BattleCardUiPool battleCardUiPool;

        protected override BasePoolObject<BattleCardUI, BattleCardData> ObjectPool
        {
            get => battleCardUiPool;
        }

        public override void Init()
        {
            base.Init();
            battleCardUiPool.Init();
        }

        private void OnDestroy()
        {
            RemoveAllObjectsFromPanel();
        }
        
    }
}

