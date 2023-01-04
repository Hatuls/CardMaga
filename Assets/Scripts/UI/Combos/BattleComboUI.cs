using CardMaga.Tools.Pools;
using System;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class BattleComboUI : BaseUIElement, IPoolableMB<BattleComboUI>, IVisualAssign<ComboCore>
    {
        public event Action<BattleComboUI> OnDisposed;
        
        [SerializeField] private ComboVisualHandler _comboVisual;

        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(ComboCore data)
        {
            _comboVisual.Init(data);
        }
    }
}