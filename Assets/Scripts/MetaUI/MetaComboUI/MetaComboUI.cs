using System;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaComboUI : BaseSlot<MetaComboUI>, IPoolableMB<MetaComboUI>,IVisualAssign<MetaComboData> ,IEquatable<MetaComboUI>
    {
        public event Action<MetaComboUI> OnDisposed;
        
        [SerializeField] private ComboVisualHandler _comboVisual;
        
        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }
        
        public void AssignVisual(MetaComboData data)
        {
            _comboVisual.Init(data.BattleComboData);
        }

        public bool Equals(MetaComboUI other)
        {
            throw new NotImplementedException();
        }
    }
}