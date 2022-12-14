using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionUICombo : BaseCollectionUIItem<MetaComboData>, IPoolableMB<MetaCollectionUICombo>,IVisualAssign<MetaCollectionDataCombo>
    {
        public event Action<MetaCollectionUICombo> OnDisposed;


        [SerializeField] private ComboVisualHandler _comboVisual;

        private MetaCollectionDataCombo _metaDataCombo;

        public override void Init()
        {
            base.Init();
            Show();
        }

        public void Dispose()
        {
            OnTryAddToDeck -= _metaDataCombo.TryAddItemToCollection;
            OnTryRemoveFromDeck -= _metaDataCombo.TryRemoveItemFromCollection;
            _metaDataCombo.OnSuccessfulAddItemToCollection -= SuccessAddToCollection;
            _metaDataCombo.OnSuccessfulRemoveItemFromCollection -= SuccessRemoveFromCollection;
            
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(MetaCollectionDataCombo data)
        {
            _metaDataCombo = data;
            
            _metaDataCombo = data;
            _comboVisual.Init(data.ItemReference.BattleComboData);
            
            OnTryAddToDeck += _metaDataCombo.TryAddItemToCollection;
            OnTryRemoveFromDeck += _metaDataCombo.TryRemoveItemFromCollection;
            _metaDataCombo.OnSuccessfulAddItemToCollection += SuccessAddToCollection;
            _metaDataCombo.OnSuccessfulRemoveItemFromCollection += SuccessRemoveFromCollection;
            
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            Enable();

            if (_metaDataCombo.IsNotMoreInstants)
            {
                DisablePlus();
                return;
            }

            if (_metaDataCombo.IsMaxInstants)
            {
                DisableMins();
                return;
            }
        }

        public override void SuccessAddToCollection(MetaComboData itemData)
        {
            UpdateVisual();
        }

        public override void SuccessRemoveFromCollection(MetaComboData itemData)
        {
            UpdateVisual();
        }
    }
}