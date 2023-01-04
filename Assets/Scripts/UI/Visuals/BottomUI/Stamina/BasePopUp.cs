using CardMaga.Tools.Pools;
using System;
using UnityEngine;


namespace CardMaga.UI.PopUp
{
    public abstract class BasePopUp : BaseUIElement, IPoolableMB<BasePopUp>
    {
        public event Action<BasePopUp> OnDisposed;

        [SerializeField]
        private bool _toRememberPreviousScreen = false;
        protected PopUpTransitionHandler _popUpTransitionHandler;

        private void Awake()
        {
            _popUpTransitionHandler = new PopUpTransitionHandler(RectTransform);
        }

        protected virtual void ResetParams()
        {
            _popUpTransitionHandler.StopTransition();
        }
        public virtual void Enter()
        {
            ResetParams();
            UIHistoryManager.Show(this, _toRememberPreviousScreen);
        }

        protected virtual void EnterMusic()
        {
        }
        protected virtual void ExitMusic()
        {
        }
        protected virtual void EnterColor()
        {
        }
        protected virtual void ExitColor()
        {
        }

        public virtual void Close()
        {
            UIHistoryManager.ReturnBack();
        }

        public void Dispose()
        => OnDisposed?.Invoke(this);
    }
}