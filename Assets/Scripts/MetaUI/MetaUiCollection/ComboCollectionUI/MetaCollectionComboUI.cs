using System;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionComboUI : BaseCollectionUIItem, IPoolableMB<MetaCollectionComboUI>,IVisualAssign<MetaCollectionComboData>
    {
        public event Action<MetaCollectionComboUI> OnDisposed;
        
        [SerializeField] private ComboVisualHandler _comboVisual;

        private MetaCollectionComboData _metaComboData;

        public override void Init()
        {
            base.Init();
            Show();
        }
        
        public void AssignVisual(MetaCollectionComboData comboData)
        {
            _metaComboData = comboData;
            
            _comboVisual.Init(comboData.ComboData);
            
            _metaComboData.OnSuccessAddOrRemoveFromCollection += SuccessAddOrRemoveCollection;
            
            UpdateVisual();
        }

        public  void TryAddToCollection()
        {
            _metaComboData.AddComboToCollection();
        }

        public  void TryRemoveFromCollection()
        {
            _metaComboData.RemoveComboFromCollection();
        }

        public void Dispose()
        {
            _metaComboData.OnSuccessAddOrRemoveFromCollection -= SuccessAddOrRemoveCollection;
            
            OnDisposed?.Invoke(this);
            Hide();
        }


        private void UpdateVisual()
        {
            Enable();

            if (_metaComboData.NotMoreInstants)
            {
                DisablePlus();
                return;
            }

            if (_metaComboData.MaxInstants)
            {
                DisableMinus();
                return;
            }
        }

        protected override void SuccessAddOrRemoveCollection()
        {
            UpdateVisual();
        }
    }
}