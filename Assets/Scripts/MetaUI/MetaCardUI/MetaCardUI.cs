using System;
using Account.GeneralData;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCardUI : BaseSlot<MetaCardUI>, IPoolableMB<MetaCardUI>, IVisualAssign<MetaCardData> ,IEquatable<MetaCardUI>
    {
        public event Action<MetaCardUI> OnDisposed;
        
        private CardInstance _cardInstance;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;

        public CardInstance CardInstance => _cardInstance;

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
        
        public void AssignVisual(MetaCardData data)
        {
            _cardVisuals.Init(data.BattleCardData);
            _cardInstance = data.CardInstance;
        }

        public bool Equals(MetaCardUI other)
        {
            if (ReferenceEquals(other, null)) return false;
            return CardInstance.ID == other.CardInstance.ID;
        }
    }
}