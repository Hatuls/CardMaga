using System.Collections;
using UnityEngine;

namespace MainMenu.ModeRotation
{
    public class ModelRotationHandler : MonoBehaviour
    {
        [SerializeField] private ModelRotation _model;
        [SerializeField, Range(1, 100)] private float _dragModifire;
        [SerializeField, Range(1, 100)] private float _restModifire;
        [SerializeField, Range(1, 50)] private float _resetTimer;

        private Coroutine _MoveCoroutine;
        private Coroutine _timeCoroutine;

        private void OnEnable()
        {
            InputReciever.Instance.OnSwipeDetected += RotateModel;
            InputReciever.Instance.OnTouchEnded += StopRotate;
        }

        private void OnDisable()
        {
            InputReciever.Instance.OnSwipeDetected -= RotateModel;
            InputReciever.Instance.OnTouchEnded -= StopRotate;
        }

        private void RotateModel(SwipeData swipeData)
        {
            _MoveCoroutine = StartCoroutine(RotateModelCoroutin(swipeData));
        }
        
        private IEnumerator RotateModelCoroutin(SwipeData swipeData)
        {
            if (!ReferenceEquals(_timeCoroutine,null))
                StopCoroutine(_timeCoroutine);
            
            float startY = _model.transform.localRotation.eulerAngles.y;
            
            while (true)
            {
                float force = swipeData.SwipeStartPosition.x - InputReciever.Instance.TouchScreenPosition.x;
                
                float yD = startY + force / _dragModifire;
                
                Quaternion quaternion =   Quaternion.Euler(0,yD,0);
                _model.transform.rotation = quaternion;
                yield return null;
            }
        }

        private void StopRotate(Vector2 vector2)
        {
            if (!ReferenceEquals(_MoveCoroutine,null))
            {
                StopCoroutine(_MoveCoroutine);
                _timeCoroutine = StartCoroutine(Timer());
            }
        }

        private IEnumerator Timer()
        {
            float timer = _resetTimer;
            
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            StartCoroutine(RestModel());
        }

        private IEnumerator RestModel()
        {
            float dir;
            float startY = _model.transform.rotation.eulerAngles.y;
            
            while (_model.transform.rotation.eulerAngles.y > 10 || _model.transform.rotation.eulerAngles.y < -10)
            {
                if (_model.transform.rotation.eulerAngles.y < 180)
                    dir = -1;
                else
                    dir = 1;
                
                Quaternion quaternion = Quaternion.Euler(0,startY += dir * _restModifire,0);
                _model.transform.rotation = quaternion;

                yield return null;
            }
        }

    }
}