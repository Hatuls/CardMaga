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
        public event Action OnTryAddCardToDeck; 
        public event Action OnTryRemoveCardFromDeck;
        

        [SerializeField] private ComboVisualHandler _comboVisual;

        private MetaCollectionDataCombo _metaDataCombo;

        public override void Init()
        {
            base.Init();
            Show();
        }

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(MetaCollectionDataCombo data)
        {
            _metaDataCombo = data;
            
            _metaDataCombo = data;
            _comboVisual.Init(data.ItemReference.BattleComboData);
            
            OnTryAddToDeck += data.TryAddItemToCollection;
            OnTryRemoveFromDeck += data.TryRemoveItemFromCollection;
            data.OnSuccessfulAddItemToCollection += SuccessAddToCollection;
            data.OnSuccessfulRemoveItemFromCollection += SuccessRemoveFromCollection;
        }

        public override void SuccessAddToCollection(MetaComboData itemData)
        {
            Debug.Log("Add combo to Deck");
        }

        public override void SuccessRemoveFromCollection(MetaComboData itemData)
        {
            Debug.Log("Remove Combo From Deck");
        }
    }
}