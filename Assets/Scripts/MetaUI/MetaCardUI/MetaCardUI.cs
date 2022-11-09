using System;
using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

namespace MetaUI.MetaCardUI
{
    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>,IShowableUI,IVisualAssign<MetaCardData>
    {
        [SerializeField] private BattleCardUI _cardUI;
        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public void Show()
        {
            throw new NotImplementedException();
        }

        public event Action<MetaCardUI> OnDisposed;
        public void AssignVisual(MetaCardData data)
        {
            _cardUI.AssignVisual(data.BattleCardData);
        }
    }
}