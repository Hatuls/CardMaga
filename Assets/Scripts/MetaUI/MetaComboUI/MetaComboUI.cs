using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.Combos;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace CardMaga.UI.MetaUI
{
    public class MetaComboUI : MonoBehaviour, IPoolableMB<MetaComboUI>,IShowableUI,IVisualAssign<MetaComboData>
    {
        public event Action<MetaComboUI> OnDisposed;

        [SerializeField] private ComboUI _comboUI;

        public void Init()
        {
            gameObject.SetActive(true);
            _comboUI.Init();
        }

        public void Dispose()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            Init();
        }

        public void AssignVisual(MetaComboData data)
        {
            _comboUI.AssignVisual(data.ComboData);
        }
    }
}