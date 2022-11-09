
using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using CardMaga.UI.ScrollPanel;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MetaUI.MetaCardUI
{

    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>, IUIElement, IVisualAssign<MetaCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;
        public event Action<int> OnAddCard;
        public event Action<int> OnRemoveCard;
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;


        public void Init()
        {
            OnInitializable?.Invoke();
            Show();
        }

        public void Dispose()
        {
            
        }


        public event Action<MetaCardUI> OnDisposed;
        public void AssignVisual(MetaCardData data)
        {
            _cardUI.AssignVisual(data.BattleCardData);
         
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