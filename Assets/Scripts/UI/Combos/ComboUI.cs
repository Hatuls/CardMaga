using Battle.Combo;
using CardMaga.Tools.Pools;
using System;
using UnityEngine;
namespace CardMaga.UI.Combos
{
    public class ComboUI : MonoBehaviour, IUIElement, IPoolableMB<ComboUI>, IVisualAssign<ComboData>
    {
        public event Action<ComboUI> OnDisposed;
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        [SerializeField] private ComboVisualHandler _comboVisual;

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

        public void Init()
        {
            OnInitializable?.Invoke();
        }

        public void AssingVisual(ComboData data)
        {
            _comboVisual.Init(data);
        }

        public void Hide()
        {
            OnHide?.Invoke();
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }
    }

}