using System;
using Account.GeneralData;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using UnityEngine;

namespace CardMaga.MetaUI
{

    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>, IUIElement, IVisualAssign<MetaCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        private CardInstance _cardInstance;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;


        public void Init()
        {
            OnInitializable?.Invoke();
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

        public void Show()
        {
            OnShow?.Invoke();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            OnHide?.Invoke();
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

        }
    }
}