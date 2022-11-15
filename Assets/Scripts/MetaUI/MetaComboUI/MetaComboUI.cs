using System;
using CardMaga.MetaData.AccoutData;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using CardMaga.UI.Combos;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaComboUI : BaseUIElement, IPoolableMB<MetaComboUI>,IVisualAssign<MetaComboData>
    {
        public event Action<MetaComboUI> OnDisposed;


        [SerializeField] private ComboVisualHandler _comboVisual;



        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }

    

        public void AssignVisual(MetaComboData data)
        {
            _comboVisual.Init(data.BattleComboData);
        }
    }
}