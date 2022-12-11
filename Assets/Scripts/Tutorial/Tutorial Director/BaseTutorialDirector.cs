using ReiTools.TokenMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace TutorialDirector
{
    public class BaseTutorialDirector : MonoBehaviour
    {
        [SerializeField] protected PlayableDirector _playableDirector;
        [SerializeField] protected RectTransform _directorRect;
        
        [SerializeField] private UnityEvent OnDirectorStart;
        [SerializeField] private UnityEvent OnDirectorEnd;
        [SerializeField] private bool _loadOnTutorialClickHelper;

        private IDisposable _token;
        
        public void StartAnimation(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            gameObject.SetActive(true);
            SubscribeEvent();
            if(_loadOnTutorialClickHelper)
            TutorialClickHelper.Instance.LoadObject(true, false, null, _directorRect);
            MoveDirectorPosition();
            if (OnDirectorStart != null)
                OnDirectorStart.Invoke();
            _playableDirector.Play();
        }

        public void StopDirector()
        {
            UnsubscribeEvent();
            if (OnDirectorEnd != null)
                OnDirectorEnd.Invoke();
            if (_loadOnTutorialClickHelper)
                TutorialClickHelper.Instance.Close();

            ReleaseToken();
            gameObject.SetActive(false);
        }

        protected void ReturnCanvasObjects()
        {
            TutorialClickHelper.Instance.ReturnObjects();
        }

        protected virtual void UnsubscribeEvent()
        {

        }

        protected virtual void SubscribeEvent()
        {

        }

        protected virtual void MoveDirectorPosition()
        {
            
        }

        private void ReleaseToken()
        {
            if (_token != null)
                _token.Dispose();

            else
                Debug.LogError("No token to release");
        }
    }
}


