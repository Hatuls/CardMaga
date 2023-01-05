using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    public class PopUpTransitionHandler
    {
        RectTransform _popUpObject;
        private List<TransitionData> _enterTransitionDatas = new List<TransitionData>();
        private List<TransitionData> _exitTransitionDatas = new List<TransitionData>();
        private int _currentTransitionIndex;
        private Sequence _sequence;

        public PopUpTransitionHandler(RectTransform rectTransform)
        {
            _popUpObject = rectTransform;
        }

        public void AddTransitionData(bool isEnter,TransitionData popUpTransitionData)
        {
            if (isEnter)
                _enterTransitionDatas.Add(popUpTransitionData);

            else
                _exitTransitionDatas.Add(popUpTransitionData);
        }

        public void StartOnlySpecificTransition(int index)
        {
            if (index <= _enterTransitionDatas.Count - 1)
                _sequence=_popUpObject.Transition(_enterTransitionDatas[index].Destination, _enterTransitionDatas[index].TransitionPackSO);

            else
                Debug.LogError("PopUpTransitionHandler: The given index is higher that the list capacity");
        }

        public void StartTransitionFlowFromBeginning()
        {
            if (_enterTransitionDatas.Count==0)
            {
                Debug.LogError("PopUpTransitionHandler: There is no transition data!");
                return;
            }

            _currentTransitionIndex = 0;
            StartTransitionFlowFromCurrentIndex();
        }

        public void StartTransitionFlowFromCurrentIndex()
        {

            if (_currentTransitionIndex == _enterTransitionDatas.Count - 1)
            {
                Debug.LogError("There is no transitions left to make");
                return;
            }

            _sequence=_popUpObject.Transition(_enterTransitionDatas[_currentTransitionIndex].Destination, _enterTransitionDatas[_currentTransitionIndex].TransitionPackSO, StartTransitionFlowFromCurrentIndex);
            if (_currentTransitionIndex == _enterTransitionDatas.Count - 1)
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
            _enterTransitionDatas.Clear();
            _exitTransitionDatas.Clear();
        }
    }
}
