using System;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCardUI : BaseUIElement, IPoolableMB<MetaCardUI>, IVisualAssign<CardInstance> ,IEquatable<MetaCardUI>
    {
        public event Action<MetaCardUI> OnDisposed;
        
        private CardInstance _cardInstance;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private RectTransform _emptyCard;

        public CardInstance CardInstance => _cardInstance;

        public bool IsEmpty => _emptyCard.gameObject.activeSelf && !_cardVisuals.gameObject.activeSelf;

        public override void Init()
        {
            base.Init();
            Hide();
        }


        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            Hide();
        }

        public override void Hide()
        { 
            _cardVisuals.gameObject.SetActive(false);
            _emptyCard.gameObject.SetActive(true);
        }

        public override void Show()
        {
            _cardVisuals.gameObject.SetActive(true);
            _emptyCard.gameObject.SetActive(false);
        }

        public void AssignVisual(CardInstance data)
        {
            _cardVisuals.Init(data.GetCardCore());
            _cardInstance = data.CardInstance;
            //_cardVisuals.CardZoomHandler.ForceReset();//plaster
        }

        public bool Equals(MetaCardUI other)
        {
            if (ReferenceEquals(other, null)) return false;
            return CardInstance.CoreID == other.CardInstance.CoreID;
        }
    }
}