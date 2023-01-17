using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class PopUpTransitionHandler
    {
        private readonly PopUp _popUp;

        private Sequence _currentSequence;
        private IPopUpTransition<TransitionData> transitionIn;
        private IPopUpTransition<TransitionData> transitionOut;
        private Vector3 _startLocation;

        public IPopUpTransition<TransitionData> TransitionIn { get => transitionIn; set => transitionIn = value; }
        public IPopUpTransition<TransitionData> TransitionOut { get => transitionOut; set => transitionOut = value; }
        public Sequence CurrentSequence { get => _currentSequence; set => _currentSequence = value; }

        public PopUpTransitionHandler(PopUp popup)
        {
            _popUp = popup;
        }
        public void AssignStartLocation(Vector3 position)
        {
            _startLocation = position;
        }
        public void SetAtStartLocation()
            => _popUp.RectTransform.position = _startLocation;
        public void EnterTransition()
        {
            //IReadOnlyList<TransitionData> transitions = TransitionIn.Transitions;
            //StartTransition(transitions, onComplete);
            KillSequence();
            TransitionIn.StartTransition(_popUp);
        }
        public void ExitTransition()
        {
            KillSequence();
            TransitionOut.StartTransition(_popUp);
            //IReadOnlyList<TransitionData> transitions = TransitionOut.Transitions;
            //StartTransition(transitions, onComplete);
        }
        public void KillSequence()
        {
            if (CurrentSequence != null && CurrentSequence.IsActive())
                DOTween.Kill(CurrentSequence);
        }
        //private void StartTransition(IReadOnlyList<TransitionData> transitions, Action onComplete = null)
        //{
        //    //KillTween();

        //    //CurrentSequence = DOTween.Sequence();
        //    //for (int i = 0; i < transitions.Count; i++)
        //    //    CurrentSequence.AppendCallback(() => _rectTransform.Transition(transitions[i].GetPosition?.Invoke() ?? _rectTransform.position, transitions[i].TransitionPackSO));

        //    //if(onComplete!=null)
        //    //CurrentSequence.AppendCallback(onComplete.Invoke);
        //}



    }




}
