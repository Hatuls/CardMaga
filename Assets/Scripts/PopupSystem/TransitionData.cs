using CardMaga.Battle.Players;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class TransitionData
    {
        public Func<Vector2> GetPosition;
        public TransitionPackSO TransitionPackSO;
        public TransitionData(TransitionPackSO transitionPackSO, Func<Vector2> getPosition)
        {
            TransitionPackSO = transitionPackSO;
            GetPosition = getPosition;
        }
        ~TransitionData()
        {
            GetPosition = null;
        }
    }
    [Serializable]
    public class TransitionBuilder
    {
        public TransitionPackSO TransitionPackSO;
        public TagSO TagSO;

        public float Alpha;
        public float Duration;
        public AnimationCurve AlphaCurve;

    }
    public interface IPopUpTransition<T>
    {
        event Action OnTransitionComplete;
        event Action OnTransitionReset;

        IReadOnlyList<T> Transitions { get; }
        void StartTransition(PopUp basePopUp);
        void StopTransition(PopUp basePopUp);
        void ResetTransition(PopUp basePopUp);
    }
    public class BasicTransition : IPopUpTransition<TransitionData>
    {
        private readonly TransitionData[] _transitionDatas;
        public event Action OnTransitionComplete;
        public event Action OnTransitionReset;

        public IReadOnlyList<TransitionData> Transitions => _transitionDatas;

        public BasicTransition(params TransitionData[] transitions)
        {
            _transitionDatas = transitions;
        }
        public void ResetTransition(PopUp basePopUp)
        {
            StopTransition(basePopUp);
            StartTransition(basePopUp);
        }

        public void StartTransition(PopUp basePopUp)
        {
            Transition(basePopUp);
        }
        private void Transition(PopUp basePopUp)
        {
                PopUpTransitionHandler popUpTransitionHandler = basePopUp.PopUpTransitionHandler;
            int count = Transitions.Count;
            if (count > 0)
            {
                popUpTransitionHandler.KillSequence();
                popUpTransitionHandler.CurrentSequence = DOTween.Sequence();

                popUpTransitionHandler.CurrentSequence.Append(basePopUp.RectTransform.Transition(Transitions[0].GetPosition?.Invoke() ?? basePopUp.RectTransform.position, Transitions[0].TransitionPackSO));

     
                if (count > 1)
                {
                    for (int i = 1; i < count; i++)
                        popUpTransitionHandler.CurrentSequence.AppendCallback(() =>
                        {
                            RectTransform rectTransform = basePopUp.RectTransform;
                            rectTransform.Transition(Transitions[i].GetPosition?.Invoke() ?? rectTransform.position, Transitions[i].TransitionPackSO);
                        });
                }
            }

            if (OnTransitionComplete != null)
                popUpTransitionHandler.CurrentSequence.AppendCallback(OnTransitionComplete.Invoke);
        }
        public void StopTransition(PopUp basePopUp)
        {
            PopUpTransitionHandler popUpTransitionHandler = basePopUp.PopUpTransitionHandler;
            popUpTransitionHandler.KillSequence();
        }
    }

}

