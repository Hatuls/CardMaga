using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionComboUI : BaseUIElement, IPoolableMB<MetaCollectionComboUI>,IVisualAssign<MetaComboData>
    {
        public event Action<MetaCollectionComboUI> OnDisposed;

        [SerializeField] private ComboVisualHandler _comboVisual;

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

        public void AssignVisual(MetaComboData data)
        {
            _comboVisual.Init(data.BattleComboData);
        }

        public void AddToDeck()
        {
            
        }

        public void RemoveFromDeck()
        {
            
        }
    }
}