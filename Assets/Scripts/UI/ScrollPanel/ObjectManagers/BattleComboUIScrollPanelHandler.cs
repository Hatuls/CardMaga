using Battle.Combo;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class BattleComboUIScrollPanelHandler : BaseScrollPanelManager<BattleComboUI,BattleComboData>
    {
        [SerializeField] private BattleComboUIPool battleComboUIPool;
    
        protected override BasePoolObject<BattleComboUI, BattleComboData> ObjectPool
        {
            get => battleComboUIPool;
        }

        public override void Init()
        {
            base.Init();
            battleComboUIPool.Init();
        }

        private void OnDestroy()
        {
            RemoveAllObjectsFromPanel();
        }
    }

}
