using Battle.Combo;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using System;
using UnityEngine;

namespace CardMaga.UI.Combos
{
    public class BattleComboUI : BaseUIElement, IPoolableMB<BattleComboUI>, IVisualAssign<BattleComboData>,IZoomableObject
    {
        public event Action<BattleComboUI> OnDisposed;

        [SerializeField] private BattleCardUI _battleCardUI;
        [SerializeField] private ComboVisualHandler _comboVisual;
        public ComboVisualHandler ComboVisualHandler => _comboVisual;
        public CardVisualHandler CardVisualHandler => ComboVisualHandler.ComboCard;


        public BattleCardUI BattleCardUI => _battleCardUI;
        public IZoomable ZoomHandler => CardVisualHandler.ZoomHandler;
        public void Dispose()
        {
            OnDisposed?.Invoke(this);
            Hide();
        }

        public void AssignVisual(BattleComboData data)
        {
            ComboVisualHandler.Init(data);
        }
    }
}