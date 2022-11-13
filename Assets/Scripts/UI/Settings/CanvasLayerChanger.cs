using UnityEngine;

namespace CardMaga.UI.Settings
{
    [AddComponentMenu("UI/Canvas Layer Changer", 11)]
    public class CanvasLayerChanger : MonoBehaviour
    {
        /// <summary>
        /// Apperantly canvas.sortingOrder cannot work with int.maxvalue (it will set it to -1)
        /// it's max value is short.MaxValue => 32767
        /// </summary>
        private const short LARGE_VALUE = short.MaxValue;
        [SerializeField]
        private Canvas _canvas;

        private int _previousValue = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toRemember">Remembering the previous value from the canvas</param>
        public void PrioritizeLayer(bool toRemember)
        {
            if(toRemember)
            _previousValue = _canvas.sortingOrder;
            SetLayer(LARGE_VALUE);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toRemember">Remembering the previous value from the canvas</param>
        public void UnPrioritizeLayer(bool toRemember)
        {
            if(toRemember)
            _previousValue = _canvas.sortingOrder;
            SetLayer(-LARGE_VALUE);
        }
        /// <summary>
        /// Returning the canvas sorting order back to the previous value
        /// NOTE: it must first remember the previous value 
        /// to do so call either the function of PrioritizeLayer/UnPrioritizeLayer with true value 
        /// </summary>

        public void Reset()
        {
            SetLayer(_previousValue);
        }

        private void SetLayer(int value)
        {
           // _canvas.overrideSorting = true;
            _canvas.sortingOrder = value;
            Canvas.ForceUpdateCanvases();
          //  _canvas.overrideSorting = false;
        }
    }
}

