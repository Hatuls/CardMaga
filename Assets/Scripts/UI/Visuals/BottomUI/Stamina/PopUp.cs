using CardMaga.Battle.Players;
using CardMaga.Tools.Pools;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace CardMaga.UI.PopUp
{
    public class PopUp : BaseUIElement, IPoolableMB<PopUp>, ITaggable
    {
        public event Action<PopUp> OnDisposed;

        [SerializeField]
        private CanvasGroup _canvasGroup;
        [SerializeField]
        private PopUpSO _popUpTag;


        private PopUpTransitionHandler _popUpTransitionHandler;
        private PopUpAlphaHandler _popUpAlphaHandler;
        public IEnumerable<TagSO> Tags { get { yield return _popUpTag; } }

        public PopUpTransitionHandler PopUpTransitionHandler { get => _popUpTransitionHandler; }
        public PopUpAlphaHandler PopUpAlphaHandler { get => _popUpAlphaHandler; }

        private void Awake()
        {
            _popUpAlphaHandler = new PopUpAlphaHandler(_canvasGroup, this);
            _popUpTransitionHandler = new PopUpTransitionHandler(this);
        }
        public virtual void Enter()
        {
            _popUpAlphaHandler.ResetToStartingAlpha();
            _popUpTransitionHandler.SetAtStartLocation();
            Show();
            _popUpAlphaHandler.Enter();
            _popUpTransitionHandler.EnterTransition();
            _popUpTransitionHandler.TransitionOut.OnTransitionComplete += Dispose;
        }

        public virtual void Close()
        {
            _popUpAlphaHandler.Exit();
            _popUpTransitionHandler.ExitTransition();
        }

        public void Dispose()
        {
            if (_popUpTransitionHandler.TransitionOut != null)
                _popUpTransitionHandler.TransitionOut.OnTransitionComplete -= Dispose;

            _popUpTransitionHandler.KillSequence();
            _popUpAlphaHandler.KillSequence();

            OnDisposed?.Invoke(this);
            Hide();
        }

    }


    public class PopUpAlphaHandler
    {
        public event Action OnTransitionComplete;
        public event Action OnTransitionReset;

        private readonly CanvasGroup _canvasGroup;
        private readonly PopUp _popUp;

        private IPopUpTransition<AlphaData> _enterAlphaTransitions;
        private IPopUpTransition<AlphaData> _exitAlphaTransitions;
        private Sequence _sequence;
        private float _startingAlpha;

        public Sequence Sequence { get => _sequence; set => _sequence = value; }
        public IPopUpTransition<AlphaData> ExitAlphaTransitions { get => _exitAlphaTransitions; set => _exitAlphaTransitions = value; }
        public IPopUpTransition<AlphaData> EnterAlphaTransitions { get => _enterAlphaTransitions; set => _enterAlphaTransitions = value; }

        public PopUpAlphaHandler(CanvasGroup canvasGroup, PopUp popUp)
        {
            _canvasGroup = canvasGroup;
            _popUp = popUp;
        }
        public void Enter()
        {
            EnterAlphaTransitions.StartTransition(_popUp);
        }
        public void Exit()
        {
            ExitAlphaTransitions.StartTransition(_popUp);
        }
        public void KillSequence()
        {
            if (_sequence != null && !_sequence.IsActive())
            {
                _sequence.Kill();
            }
        }
        internal Tween Fade(AlphaData alpha) => _canvasGroup.DOFade(alpha.Alpha, alpha.Duration).SetEase(alpha.Curve);
        public void ResetToStartingAlpha() => _canvasGroup.alpha = _startingAlpha;
        internal void SetStartingAlpha(float v) => _startingAlpha = v;


    }


    public class AlphaTransition : IPopUpTransition<AlphaData>
    {
        public event Action OnTransitionComplete;
        public event Action OnTransitionReset;
        private readonly AlphaData[] _alphaTransition;
        public AlphaTransition(params AlphaData[] alphaDatas)
        {
            _alphaTransition = alphaDatas;
        }
        public IReadOnlyList<AlphaData> Transitions => _alphaTransition;



        public void ResetTransition(PopUp basePopUp)
        {
            StopTransition(basePopUp);
            StartTransition(basePopUp);
        }

        public void StartTransition(PopUp basePopUp)
        {
            var alpha = basePopUp.PopUpAlphaHandler;
            alpha.Sequence = DOTween.Sequence();
            for (int i = 0; i < Transitions.Count; i++)
                alpha.Sequence.Append( alpha.Fade(Transitions[i]));

        }

        public void StopTransition(PopUp basePopUp)
        {
            basePopUp.PopUpAlphaHandler.KillSequence();
        }
    }
    public class AlphaData
    {
        public readonly AnimationCurve Curve;
        public readonly float Alpha;
        public readonly float Duration;
        public AlphaData(float alpha, float duration, AnimationCurve curve)
        {
            Alpha = alpha;
            Duration = duration;
            Curve = curve;
        }
    }
}