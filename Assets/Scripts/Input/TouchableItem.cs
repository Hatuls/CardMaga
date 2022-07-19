using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CardMaga.Input
{

    public class TouchableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public enum State
        {
            Lock,
            UnLock
        }

        [SerializeField] private float _holdDelaySce = .5f;

        [SerializeField] [ReadOnly] private State _currentState;
        private bool _isHold;

        private bool _isTouchable;

        public State CurrentState => _currentState;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isTouchable)
            {
                StartCoroutine(HoldDelay(eventData));
                OnPointDown?.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isTouchable)
            {
                if (_isHold)
                {
                    EndHold(eventData);
                    return;
                }

                ProcessTouch(eventData);
            }
        }

        protected event Action OnClick;
        protected event Action OnBeginHold;
        protected event Action OnEndHold;
        protected event Action OnHold;
        protected event Action OnPointDown;
        protected event Action OnPointUp;


        private IEnumerator HoldDelay(PointerEventData eventData)
        {
            yield return new WaitForSeconds(_holdDelaySce);
            _isHold = true;
            StartCoroutine(ProcessHoldTouchCoroutine(eventData));
        }

        private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
        {
            yield return null;
            OnBeginHold?.Invoke();

            while (_isHold)
            {
                yield return null;
                OnHold?.Invoke();
            }
        }

        private void EndHold(PointerEventData eventData)
        {
            _isHold = false;
            StopAllCoroutines();
            OnEndHold?.Invoke();
            OnPointUp?.Invoke();
        }

        private void ProcessTouch(PointerEventData eventData)
        {
            StopAllCoroutines();
            OnClick?.Invoke();
            OnPointUp?.Invoke();
        }

        private void ChangeState(State state)
        {
            _currentState = state;

            switch (_currentState)
            {
                case State.Lock:
                    _isTouchable = false;
                    break;
                case State.UnLock:
                    _isTouchable = true;
                    break;
                default:
                    Debug.LogError(name + " State Not Set");
                    break;
            }

            Debug.Log(name + " is touchable set to " + _isTouchable);
        }

        [ContextMenu("ToggleState")]
        public void ToggleState()
        {
            if (_currentState == State.Lock)
                ChangeState(State.UnLock);

            else if (_currentState == State.UnLock) ChangeState(State.Lock);
        }

        public void ForceChangeState(bool isTouchable)
        {
            if (isTouchable)
                ChangeState(State.UnLock);
            else
                ChangeState(State.Lock);
        }
    }


    public class TouchableItem<T> : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
        where T : MonoBehaviour
    {
        public enum State
        {
            Lock,
            UnLock
        }

        [SerializeField] private float _holdDelaySce = .5f;
        [SerializeField] T _touchableItem;
        [SerializeField] [ReadOnly] private State _currentState;
        private bool _isHold;

        private bool _isTouchable;

        public State CurrentState => _currentState;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isTouchable)
            {
                StartCoroutine(HoldDelay(eventData));
                OnPointDown?.Invoke(_touchableItem);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isTouchable)
            {
                if (_isHold)
                {
                    EndHold(eventData);
                    return;
                }

                ProcessTouch(eventData);
            }
        }

        public virtual event Action<T> OnClick;
        public virtual event Action<T> OnBeginHold;
        public virtual event Action<T> OnEndHold;
        public virtual event Action<T> OnHold;
        public virtual event Action<T> OnPointDown;
        public virtual event Action<T> OnPointUp;


        private IEnumerator HoldDelay(PointerEventData eventData)
        {
            yield return new WaitForSeconds(_holdDelaySce);
            _isHold = true;
            StartCoroutine(ProcessHoldTouchCoroutine(eventData));
        }

        private IEnumerator ProcessHoldTouchCoroutine(PointerEventData eventData)
        {
            yield return null;
            OnBeginHold?.Invoke(_touchableItem);

            while (_isHold)
            {
                yield return null;
                OnHold?.Invoke(_touchableItem);
            }
        }

        private void EndHold(PointerEventData eventData)
        {
            _isHold = false;
            StopAllCoroutines();
            OnEndHold?.Invoke(_touchableItem);
            OnPointUp?.Invoke(_touchableItem);
        }

        private void ProcessTouch(PointerEventData eventData)
        {
            StopAllCoroutines();
            OnClick?.Invoke(_touchableItem);
            OnPointUp?.Invoke(_touchableItem);
        }

        private void ChangeState(State state)
        {
            _currentState = state;

            switch (_currentState)
            {
                case State.Lock:
                    _isTouchable = false;
                    break;
                case State.UnLock:
                    _isTouchable = true;
                    break;
                default:
                    Debug.LogError(name + " State Not Set");
                    break;
            }

            Debug.Log(name + " is touchable set to " + _isTouchable);
        }

        [ContextMenu("ToggleState")]
        public void ToggleState()
        {
            if (_currentState == State.Lock)
                ChangeState(State.UnLock);

            else if (_currentState == State.UnLock) ChangeState(State.Lock);
        }

        public void ForceChangeState(bool isTouchable)
        {
            if (isTouchable)
                ChangeState(State.UnLock);
            else
                ChangeState(State.Lock);
        }
    }
}