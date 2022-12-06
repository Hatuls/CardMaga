using System;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaComboUI : BaseUIElement, IPoolableMB<MetaComboUI>,IVisualAssign<MetaComboData> ,IEquatable<MetaComboUI>
    {
        public event Action<MetaComboUI> OnDisposed;
        
        [SerializeField] private ComboVisualHandler _comboVisual;
        [SerializeField] private RectTransform _emptyCombo;

        private MetaComboData _metaComboData;

        public bool IsEmpty => _emptyCombo.gameObject.activeSelf && !_comboVisual.gameObject.activeSelf;
        
        public MetaComboData MetaComboData => _metaComboData;
        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }

        public override void Show()
        {
            _comboVisual.gameObject.SetActive(true);
            _emptyCombo.gameObject.SetActive(false);
        }

        public override void Hide()
        {
            _comboVisual.gameObject.SetActive(false);
            _emptyCombo.gameObject.SetActive(true);
        }


        public void AssignVisual(MetaComboData data)
        {
            _metaComboData = data;
            _comboVisual.Init(data.BattleComboData);
        }

        public bool Equals(MetaComboUI other)
        {
            if (ReferenceEquals(other, null)) return false;
            return MetaComboData.ID == other.MetaComboData.ID;
        }
    }
}