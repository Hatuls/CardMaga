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
        [SerializeField] private PlayableDirector _playableDirector;
        [SerializeField] protected RectTransform _directorRect;
        
        [SerializeField] private UnityEvent OnDirectorStart;
        [SerializeField] private UnityEvent OnDirectorEnd;

        private IDisposable _token;
        private TutorialClickHelper _tutorialClickHelper;

        
        public void StartAnimation(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            _tutorialClickHelper = TutorialClickHelper.Instance;
            gameObject.SetActive(true);
            SubscribeEvent();
            _tutorialClickHelper.LoadObject(true, false, null, _directorRect);
            MoveDirectorPosition();
            _playableDirector.Play();
            if (OnDirectorStart != null)
                OnDirectorStart.Invoke();
        }
        public void StopDirector()
        {
            UnsubscribeEvent();
            //ReturnCanvasObjects();
            _tutorialClickHelper.Close();
            ReleaseToken();
            gameObject.SetActive(false);
        }

        protected void ReturnCanvasObjects()
        {
           _tutorialClickHelper.ReturnObjects();
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


