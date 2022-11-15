using System;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI
{

    public class MetaCardUI : BaseUIElement, IPoolableMB<MetaCardUI>,  IVisualAssign<MetaCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;


        private CardInstance _cardInstance;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;


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
    }
}