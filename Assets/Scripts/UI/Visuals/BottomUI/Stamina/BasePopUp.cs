using CardMaga.Battle.Players;
using CardMaga.Tools.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.UI.PopUp
{
    public abstract class BasePopUp : BaseUIElement, IPoolableMB<BasePopUp>, ITaggable
    {
        public event Action<BasePopUp> OnDisposed;

        [SerializeField] private PopUpSO[] _popUpSO;
        [SerializeField] private bool _toRememberPreviousScreen = false;
        
        protected PopUpTransitionHandler _popUpTransitionHandler;

        public PopUpTransitionHandler PopUpTransitionHandler => _popUpTransitionHandler;
        public PopUpSO[] PopUpSO { get => _popUpSO; }
        public IReadOnlyList<TagSO> Tags => PopUpSO;

        public virtual void Awake()
        {
            _popUpTransitionHandler = new PopUpTransitionHandler(RectTransform);
            _popUpTransitionHandler.OnExitTransitionEnding += Close;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        protected virtual void ResetParams()
        {
            _popUpTransitionHandler.StopTransition();
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
        
        public void Enter()
        {
            ResetParams();
            UIHistoryManager.Show(this, _toRememberPreviousScreen);
            StartEnterTransition();
        }

        public virtual void StartEnterTransition()
        {
            _popUpTransitionHandler.ResetAndStartTransitionFlow(true);
        }
        
        public virtual void StartExitTransition()
        {
            _popUpTransitionHandler.ResetAndStartTransitionFlow(false);
        }

        public void Close()
        {
            _popUpTransitionHandler.ClearTransitionDatas();
            Dispose();
            UIHistoryManager.ReturnBack();
        }

        public void Dispose()
        => OnDisposed?.Invoke(this);
    }
}