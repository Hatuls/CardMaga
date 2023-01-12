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
    public class MetaComboUI : BaseUIElement, IPoolableMB<MetaComboUI>,IVisualAssign<ComboCore> ,IEquatable<MetaComboUI>
    {
        public event Action<MetaComboUI> OnDisposed;
        
        [SerializeField] private ComboVisualHandler _comboVisual;
        [SerializeField] private RectTransform _emptyCombo;

        private ComboCore _comboData;

        public bool IsEmpty => _emptyCombo.gameObject.activeSelf && !_comboVisual.gameObject.activeSelf;
        
        public ComboCore ComboData => _comboData;
        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }

        public override void Init()
        {
            base.Init();
            Hide();
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


        public void AssignVisual(ComboCore data)
        {
            _comboData = data;
            _comboVisual.Init(data);
        }

        public bool Equals(MetaComboUI other)
        {
            if (ReferenceEquals(other, null)) return false;
            return ComboData.CoreID == other.ComboData.CoreID;
        }
    }
}