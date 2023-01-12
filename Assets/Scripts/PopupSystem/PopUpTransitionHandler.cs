using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class PopUpTransitionHandler
    {
        public event Action OnEnterTransitionEnding;
        public event Action OnExitTransitionEnding;
        private RectTransform _popUpObject;
        private List<TransitionData> _enterTransitionDatas = new List<TransitionData>();
        private List<TransitionData> _exitTransitionDatas = new List<TransitionData>();
        private int _currentTransitionIndex;
        private Sequence _sequence;

        public PopUpTransitionHandler(RectTransform rectTransform)
        {
            _popUpObject = rectTransform;
        }

        public void AddTransitionData(bool isEnter, TransitionData popUpTransitionData)
        {
            if (isEnter)
                _enterTransitionDatas.Add(popUpTransitionData);

            else
                _exitTransitionDatas.Add(popUpTransitionData);
        }

        public void StartOnlySpecificTransition(bool isEnter, int index)
        {
            if (index <= _enterTransitionDatas.Count - 1)
            {
                if (isEnter)
                    _sequence = _popUpObject.Transition(_enterTransitionDatas[index].Destination, _enterTransitionDatas[index].TransitionPackSO);

                else
                    _sequence = _popUpObject.Transition(_exitTransitionDatas[index].Destination, _exitTransitionDatas[index].TransitionPackSO);
            }

            else
                Debug.LogError("PopUpTransitionHandler: The given index is higher that the list capacity");
        }

        public void ResetAndStartTransitionFlow(bool isEnter)
        {
            if (_enterTransitionDatas.Count == 0 && isEnter)
            {
                Debug.LogError("PopUpTransitionHandler: There is no enter transition data!");
                return;
            }

            else if (_enterTransitionDatas.Count == 0 && !isEnter)
            {
                Debug.LogError("PopUpTransitionHandler: There is no exit transition data!");
                return;
            }

            _currentTransitionIndex = 0;
            StartTransitionFromCurrentIndex(isEnter);
        }

        public void StartTransitionFromCurrentIndex(bool isEnter)
        {

            if (isEnter)
                EnterTransition();
            else
                ExitTransition();

            void EnterTransition()
            {
                if (_enterTransitionDatas.Count == 0)
                {
                    Debug.LogError("PopUpTransitionHandler: There is no enter transition data!");
                    
                    return;
                }
                _sequence = _popUpObject.Transition(_enterTransitionDatas[_currentTransitionIndex].Destination, _enterTransitionDatas[_currentTransitionIndex].TransitionPackSO, CheckForNextEnterTransition);
            }

            void ExitTransition()
            {
                if (_exitTransitionDatas.Count == 0)
                {
                    Debug.LogError("PopUpTransitionHandler: There is no exit transition data!");
                    
                    return;
                }
                _sequence = _popUpObject.Transition(_exitTransitionDatas[_currentTransitionIndex].Destination, _exitTransitionDatas[_currentTransitionIndex].TransitionPackSO, CheckForNextExitTransition);

            }

            void CheckForNextEnterTransition()
            {
                if (_currentTransitionIndex == _enterTransitionDatas.Count - 1)
                {
                    if (OnEnterTransitionEnding != null)
                        OnEnterTransitionEnding.Invoke();
                }
                else
                {
                    _currentTransitionIndex++;
                    EnterTransition();
                }
            }

            void CheckForNextExitTransition()
            {

                if (_currentTransitionIndex == _exitTransitionDatas.Count - 1)
                {
                    if (OnExitTransitionEnding != null)
                        OnExitTransitionEnding.Invoke();
                }

                else
                {
                    _currentTransitionIndex++;
                    ExitTransition();
                }
            }
        }

        public void StopTransition()
        {
            if (_sequence != null)
                _sequence.Kill();
        }

        public void ClearTransitionDatas()
        {
            _enterTransitionDatas.Clear();
            _exitTransitionDatas.Clear();
        }
    }
}
