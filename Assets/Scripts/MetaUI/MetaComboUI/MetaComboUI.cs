using System;
using Account.GeneralData;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaComboUI : BaseUIElement, IPoolableMB<MetaComboUI>,IVisualAssign<ComboCore>,IVisualAssign<ComboInstance> ,IEquatable<MetaComboUI>
    {
        public event Action<MetaComboUI> OnDisposed;
        
        [SerializeField] private BattleComboUI _comboVisual;
        [SerializeField] private RectTransform _emptyCombo;

        private ComboCore _comboData;
        public ComboCore ComboData => _comboData;
        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }

        public override void Init()
        {
            base.Init();
            Hide();
        }

        public override void Hide()
        {
            base.Hide();
            Dispose();
        }


        public void AssignVisual(ComboCore data)
        {
            _comboData = data;
            _comboVisual.AssignVisual(data);
        }
        
        public void AssignVisual(ComboInstance data)
        {
            AssignVisual(data.ComboCore);
        }

        public bool Equals(MetaComboUI other)
        {
            if (ReferenceEquals(other, null)) return false;
            return ComboData.CoreID == other.ComboData.CoreID;
        }
    }
}