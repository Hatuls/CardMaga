using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace MetaUI.MetaComboUI
{
    public class MetaComboUI : MonoBehaviour, IPoolableMB<MetaComboUI>,IShowableUI,IVisualAssign<MetaComboData>
    {
        public event Action<MetaComboUI> OnDisposed;
        
        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public void AssingVisual(MetaComboData data)
        {
            throw new NotImplementedException();
        }
    }
}