using System;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{
    public class SwipeTerminal : MonoBehaviour
    {
        public event Action<SwipeData> OnSwipe;
        public event Action<SwipeData> OnSwipingLeft;
        public event Action<SwipeData> OnSwipingRight;
        public event Action<SwipeData> OnSwipingUp;
        public event Action<SwipeData> OnSwipingDown;

        public event Action<Vector2> OnSwipeEnded;

        private void OnEnable()
        {
            InputReciever.Instance.OnSwipeDetected += SwipeDetected;
            InputReciever.Instance.OnTouchEnded += ReleaseSwipe;
        }
        private void OnDisable()
        {
            InputReciever.Instance.OnSwipeDetected -= SwipeDetected;
            InputReciever.Instance.OnTouchEnded -= ReleaseSwipe;
        }


        private void ReleaseSwipe(Vector2 obj)
        {
            OnSwipeEnded?.Invoke(obj);
        }

        private void SwipeDetected(SwipeData obj)
        {
            switch (obj.SwipeDirection)
            {
                case InputReciever.SwipeDirection.Left:
                    OnSwipingLeft?.Invoke(obj);
                    break;
                case InputReciever.SwipeDirection.Right:
                    OnSwipingRight?.Invoke(obj);
                    break;
                case InputReciever.SwipeDirection.Up:
                    OnSwipingUp?.Invoke(obj);
                    break;
                case InputReciever.SwipeDirection.Down:
                    OnSwipingDown?.Invoke(obj);
                    break;
            }
            OnSwipe?.Invoke(obj);
        }
    }
}