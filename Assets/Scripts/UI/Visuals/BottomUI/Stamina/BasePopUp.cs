using CardMaga.Tools.Pools;
using DG.Tweening;
using System;
using UnityEngine;


namespace CardMaga.UI.PopUp
{
    public abstract class BasePopUp : BaseUIElement, IPoolableMB<BasePopUp>
    {
        public event Action<BasePopUp> OnDisposed;

        [SerializeField]
        private bool _toRememberPreviousScreen = false;

        protected Tween _sequence;


        protected virtual void ResetParams()
        {
            if (_sequence != null)
                _sequence.Kill(false);
            //    _rectTransform.SetScale(0);
        }
        public virtual void Enter()
        {
            ResetParams();
            UIHistoryManager.Show(this, _toRememberPreviousScreen);
        }

        public virtual void Close()
        {
            UIHistoryManager.ReturnBack();
        }

        public void Dispose()
        => OnDisposed?.Invoke(this);
    }
}