using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionUICombo : BaseCollectionUIItem<MetaComboData>, IPoolableMB<MetaCollectionUICombo>,IVisualAssign<MetaCollectionComboData>
    {
        public event Action<MetaCollectionUICombo> OnDisposed;


        [SerializeField] private ComboVisualHandler _comboVisual;

        private MetaCollectionComboData metaComboData;

        public override void Init()
        {
            base.Init();
            Show();
        }

        public void Dispose()
        {
            OnTryAddToDeck -= metaComboData.AddItemToCollection;
            OnTryRemoveFromDeck -= metaComboData.RemoveItemFromCollection;
            metaComboData.OnSuccessfulAddItemToCollection -= SuccessAddToCollection;
            metaComboData.OnSuccessfulRemoveItemFromCollection -= SuccessRemoveFromCollection;
            
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(MetaCollectionComboData comboData)
        {
            metaComboData = comboData;
            
            metaComboData = comboData;
            _comboVisual.Init(comboData.ItemReference.BattleComboData);
            
            OnTryAddToDeck += metaComboData.AddItemToCollection;
            OnTryRemoveFromDeck += metaComboData.RemoveItemFromCollection;
            metaComboData.OnSuccessfulAddItemToCollection += SuccessAddToCollection;
            metaComboData.OnSuccessfulRemoveItemFromCollection += SuccessRemoveFromCollection;
            
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            Enable();

            if (metaComboData.IsNotMoreInstants)
            {
                DisablePlus();
                return;
            }

            if (metaComboData.IsMaxInstants)
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