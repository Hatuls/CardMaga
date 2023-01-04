using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class PopUpTransitionHandler
    {
        RectTransform _popUpObject;
        private List<PopUpTransitionData> _transitionDatas = new List<PopUpTransitionData>();
        private int _currentTransitionIndex;
        private Sequence _sequence;

        public PopUpTransitionHandler(RectTransform rectTransform)
        {
            _popUpObject = rectTransform;
        }

        public void AddTransitionData(PopUpTransitionData popUpTransitionData)
        {
            _transitionDatas.Add(popUpTransitionData);
        }

        public void StartOnlySpecificTransition(int index)
        {
            if (index <= _transitionDatas.Count - 1)
                _sequence=_popUpObject.Transition(_transitionDatas[index].Destination, _transitionDatas[index].TransitionPackSO);

            else
                Debug.LogError("PopUpTransitionHandler: The given index is higher that the list capacity");
        }

        public void StartTransitionFlowFromBeginning()
        {
            if (_transitionDatas.Count==0)
            {
                Debug.LogError("PopUpTransitionHandler: There is no transition data!");
                return;
            }

            _currentTransitionIndex = 0;
            StartTransitionFlowFromCurrentIndex();
        }

        public void StartTransitionFlowFromCurrentIndex()
        {

            if (_currentTransitionIndex == _transitionDatas.Count - 1)
            {
                Debug.LogError("There is no transitions left to make");
                return;
            }

            _sequence=_popUpObject.Transition(_transitionDatas[_currentTransitionIndex].Destination, _transitionDatas[_currentTransitionIndex].TransitionPackSO, StartTransitionFlowFromCurrentIndex);
            if (_currentTransitionIndex == _transitionDatas.Count - 1)
                return;
            _currentTransitionIndex++;
        }

        /// <summary> 
        /// 
        /// </summary>
        public void StopTransition()
        {
            if(_sequence!=null)
            _sequence.Kill();
        }

        public void ClearTransitionDatas()
        {
            _transitionDatas.Clear();
        }
    }
}
