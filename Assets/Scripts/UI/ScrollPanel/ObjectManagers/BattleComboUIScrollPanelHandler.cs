using Battle.Combo;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.UI.ScrollPanel
{
    public class BattleComboUIScrollPanelHandler : BaseScrollPanelManager<ComboUI,ComboData>
    {
        [SerializeField] private ComboUIPool _comboUIPool;
    
        protected override BasePoolObject<ComboUI, ComboData> ObjectPool
        {
            get => _comboUIPool;
        }

        public override void Init()
        {
            base.Init();
            _comboUIPool.Init();
        }

        private void OnDestroy()
        {
            RemoveAllObjectsFromPanel();
        }
    }

}
