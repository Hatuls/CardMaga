using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionComboUI : BaseCollectionItemUI<MetaComboData>, IPoolableMB<MetaCollectionComboUI>,IVisualAssign<MetaCollectionComboData>
    {
        public event Action<MetaCollectionComboUI> OnDisposed;
        public event Action OnTryAddCardToDeck; 
        public event Action OnTryRemoveCardFromDeck;
        

        [SerializeField] private ComboVisualHandler _comboVisual;

        private MetaCollectionComboData _metaComboData;

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

        public void AssignVisual(MetaCollectionComboData data)
        {
            _metaComboData = data;
            
            _metaComboData = data;
            _comboVisual.Init(data.ItemReference.BattleComboData);
            
            OnTryAddToDeck += data.TryRemoveItemReference;
            OnTryRemoveFromDeck += data.TryAddItemReference;
            data.OnSuccessfullAddItem += SuccessAddToDeck;
            data.OnSuccessfullRemoveItem += SuccessRemoveFromDeck;
        }

        public override void SuccessAddToDeck(MetaComboData metaCardData)
        {
            Debug.Log("Add combo to Deck");
        }

        public override void SuccessRemoveFromDeck(MetaComboData metaCardData)
        {
            Debug.Log("Remove Combo From Deck");
        }
    }
}