using Battle.Combo;
using CardMaga.Tools.Pools;
using System;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class BattleComboUI : BaseUIElement, IPoolableMB<BattleComboUI>, IVisualAssign<BattleComboData>
    {
        public event Action<BattleComboUI> OnDisposed;


        [SerializeField] private ComboVisualHandler _comboVisual;

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(BattleComboData data)
        {
            _comboVisual.Init(data);
        }
    }
}