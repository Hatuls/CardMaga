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
      
            KillSequence();
            TransitionIn?.StartTransition(_popUp);
        }
        public void ExitTransition()
        {
            KillSequence();
            TransitionOut?.StartTransition(_popUp);
         
        }
        public void KillSequence()
        {
            if (CurrentSequence != null && CurrentSequence.IsActive())
                DOTween.Kill(CurrentSequence);
        }


    }




}
