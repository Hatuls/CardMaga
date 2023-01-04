using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
namespace CardMaga.Meta.Upgrade
{
    public class UpgradeCardMover : MonoBehaviour
    {
        public event Action OnSwipeLeftExecuted;
        public event Action OnSwipeRightIsAtMaxValue;
        public event Action<float> OnSwipingLeft;
        public event Action<float> OnSwipingRight;
        public event Action OnSwipeExecuted;

        [SerializeField]
        private SwipeTerminal _swipeTerminal;
        [SerializeField]
        private RectTransform _cardsContainer;
        [SerializeField]
        private RectTransform _middlePosition;

        private Vector3 _startPosition;
        private Vector3 _leftPosition;
        private Vector3 _rightPosition;
    
        [SerializeField, Tooltip("The precentage to call swipe success"), Range(0f, 1f)]
        private float _swipeDistanceSuccess;
        [SerializeField, ReadOnly]
        private bool _isSwipeRight;
        [SerializeField, ReadOnly]
        private bool _isSwipeSucceded;

        [SerializeField, ReadOnly]
        private float _swipeDistanceToChangeLocations;
        [SerializeField, Range(0, 1f)]
        private float _resetDelayTime = .3f;
        private float HalfScreenSize => Screen.width / 2;
        private void Awake()
        {
            _startPosition = _middlePosition.position;
            Vector3 middlePosition = _startPosition;

            middlePosition.x -= HalfScreenSize; // middle minus half of the screen size for the left pos
            _leftPosition = middlePosition;

            middlePosition.x += Screen.width; //  left position plus screen size will get us the right pos
            _rightPosition = middlePosition;

            float screenWidthDistance = Vector2.Distance(_rightPosition, _leftPosition);

            _swipeDistanceToChangeLocations = screenWidthDistance * _swipeDistanceSuccess;
        }
        private void OnEnable()
        {
            _swipeTerminal.OnSwipingLeft += MoveRight;
            _swipeTerminal.OnSwipingRight += MoveLeft;
            _swipeTerminal.OnSwipeEnded += ResetCardPosition;
        }
        private void OnDisable()
        {
            _swipeTerminal.OnSwipeEnded -= ResetCardPosition;
            _swipeTerminal.OnSwipingLeft -= MoveRight;
            _swipeTerminal.OnSwipingRight -= MoveLeft;
        }

        private void MoveRight(SwipeData obj)
        {
            _isSwipeRight = true;
            float swipeDistance = obj.SwipeDistance;
            _isSwipeSucceded = swipeDistance > _swipeDistanceToChangeLocations;


            float distance = _rightPosition.x - _middlePosition.position.x;
            // Debug.Log("! " + obj.SwipeDistance + _startPosition.x);
            if (swipeDistance < HalfScreenSize)
                OnSwipingRight?.Invoke(-swipeDistance); // Need to check this****

                _middlePosition.position = _startPosition + swipeDistance * Vector3.right;
            //_cardsContainer.DOMoveX(_startPosition.x + obj.SwipeDistance, Time.deltaTime);
        }
        private void MoveLeft(SwipeData obj)
        {
            _isSwipeRight = false;
            _isSwipeSucceded = obj.SwipeDistance > _swipeDistanceToChangeLocations;

            if(obj.SwipeDistance < HalfScreenSize)
                OnSwipingLeft?.Invoke(obj.SwipeDistance); //need to check this **
            
            _middlePosition.position = _startPosition - obj.SwipeDistance*Vector3.right;
              //  _cardsContainer.DOMoveX(_startPosition.x - obj.SwipeDistance, Time.deltaTime);
        }


        private void ResetCardPosition(Vector2 touchPos)
        {
            _middlePosition.position = _startPosition;
            if (_isSwipeSucceded)
            {
                if (_isSwipeRight)
                    OnSwipeRightIsAtMaxValue?.Invoke();
                else
                    OnSwipeLeftExecuted?.Invoke();
            }
            OnSwipeExecuted?.Invoke();
       //     ResetCardPosition();
        }
   //     private void ResetCardPosition() => _cardsContainer.DOMoveX(_startPosition.x, _resetDelayTime, true);

        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.blue;

        //    Gizmos.DrawLine(_rightPosition, _middleCardUI.RectTransform.position);

        //}
    }
}