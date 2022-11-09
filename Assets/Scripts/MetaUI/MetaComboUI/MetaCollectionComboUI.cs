using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI.MetaComboUI
{
    public class MetaCollectionComboUI : MonoBehaviour, IPoolableMB<MetaCollectionComboUI>,IUIElement,IVisualAssign<MetaComboData>
    {
        public event Action OnInitializable;
        public event Action<MetaCollectionComboUI> OnDisposed;
        public event Action OnShow;
        public event Action OnHide;
        
        [SerializeField] private ComboVisualHandler _comboVisual;

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

        public void Show()
        {
            OnShow?.Invoke();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            OnHide?.Invoke();
            gameObject.SetActive(false);
        }

        public void AssignVisual(MetaComboData data)
        {
            _comboVisual.Init(data.BattleComboData);
        }
    }
}