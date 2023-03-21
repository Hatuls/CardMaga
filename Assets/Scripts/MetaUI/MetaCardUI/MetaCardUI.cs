using System;
using Account.GeneralData;
using CardMaga.MetaData.Collection;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Card;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCardUI : BaseUIElement, IPoolableMB<MetaCardUI>, IVisualAssign<MetaCardInstanceInfo> ,IEquatable<MetaCardUI>

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

        public void AssignVisual(MetaCardInstanceInfo data)
        {
            CardUI.AssignVisualAndData(Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(data.CardInstance));
    
            _cardInstance = data.CardInstance;
            //_cardVisuals.CardZoomHandler.ForceReset();//plaster
        }

        public bool Equals(MetaCardUI other)
        {
            if (other == null) return false;
            return CardInstance.CoreID == other.CardInstance.CoreID;
        }

#if UNITY_EDITOR
        [Header("Editor:")]
        [SerializeField]
        private CardCore core;
        [ContextMenu("Assign Card Instance")]
        private void SetInstance() => AssignVisual( new MetaCardInstanceInfo(new CardInstance(core)));
#endif
    }
}