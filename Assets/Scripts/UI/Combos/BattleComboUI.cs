using CardMaga.Tools.Pools;
using System;
using Account.GeneralData;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class BattleComboUI : BaseUIElement, IPoolableMB<BattleComboUI>, IVisualAssign<ComboCore>
    {
        public event Action<BattleComboUI> OnDisposed;
        public static event Action<ComboCore> OnComboTypePopUpSelected;
        public static event Action OnComboTypeRelease;


        [SerializeField] private ComboVisualHandler _comboVisual;
        private ComboCore _data;
        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(ComboCore data)
        {
            _comboVisual.Init(data);
            _data = data;   
        }


        public void ShowComboInfo() => OnComboTypePopUpSelected?.Invoke(_data);
        public void HideComboInfo() => OnComboTypeRelease?.Invoke();
    }
}