using Battle.Combo;
using CardMaga.Tools.Pools;
using System;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class BattleComboUI : MonoBehaviour, IUIElement, IPoolableMB<BattleComboUI>, IVisualAssign<BattleComboData>
    {
        public event Action<BattleComboUI> OnDisposed;
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

        public void Hide()
        {
            OnHide?.Invoke();
            if (gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        public void AssignVisual(BattleComboData data)
        {
            _comboVisual.Init(data);
        }
    }
}