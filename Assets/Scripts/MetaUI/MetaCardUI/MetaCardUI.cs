using System;
using Account.GeneralData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCardUI : BaseUIElement, IPoolableMB<MetaCardUI>, IVisualAssign<CardInstance> ,IEquatable<MetaCardUI>

    {
        public event Action<MetaCardUI> OnDisposed;

        private CardInstance _cardInstance;

        [SerializeField] private BattleCardUI _cardUI;
        public CardInstance CardInstance => _cardInstance;


        public BattleCardUI CardUI  => _cardUI; 

        public override void Init()
        {
            base.Init();
            Hide();
        }
        
        public void Dispose()
        {
            OnDisposed?.Invoke(this);
        }

        public override void Hide()
        {
            base.Hide();
            Dispose();
        }

        public void AssignVisual(CardInstance data)
        {
            CardUI.AssignVisualAndData(Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(data));
    
            _cardInstance = data;
            //_cardVisuals.CardZoomHandler.ForceReset();//plaster
        }

        public bool Equals(MetaCardUI other)
        {
            if (ReferenceEquals(other, null)) return false;
            return CardInstance.CoreID == other.CardInstance.CoreID;
        }
    }
}