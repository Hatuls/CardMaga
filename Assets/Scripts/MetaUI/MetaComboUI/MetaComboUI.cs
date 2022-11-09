using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.UI.MetaUI
{
    public class MetaComboUI : MonoBehaviour, IPoolableMB<MetaComboUI>,IUIElement,IVisualAssign<MetaComboData>
    {
        public event Action<MetaComboUI> OnDisposed;
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        [SerializeField] private ComboUI _comboUI;

        public void Init()
        {
            OnInitializable?.Invoke();
            _comboUI.Init();
        }

        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }
     
        public void AssingVisual(MetaComboData data)
        {
            _comboUI.AssingVisual(data.ComboData);
        }

        public void Hide()
        {
            OnHide?.Invoke();
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
        public void Show()
        {
            OnShow?.Invoke();
            gameObject.SetActive(true);
        }

    }
}