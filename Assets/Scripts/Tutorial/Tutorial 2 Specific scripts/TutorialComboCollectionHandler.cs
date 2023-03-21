using CardMaga.Trackers;
using ReiTools.TokenMachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.UI.Collections
{
    public class TutorialComboCollectionHandler : MonoBehaviour
    {
        #region SerializeField Of TrackID
        [SerializeField] TrackerID collectionEnterButtonTrackerID;
        [SerializeField] TrackerID collectionPanelTrackerID;
        [SerializeField] TrackerID collectionExitButtonTrackerID;
        #endregion

        #region UnityEvents
        [SerializeField] private UnityEvent BeforeCollectionEnterButtonPress;
        [SerializeField] private UnityEvent AfterCollectionEnterButtonPress;
        [SerializeField] private UnityEvent BeforeCollectionExitButtonPress;
        [SerializeField] private UnityEvent AfterCollectionExitButtonPress;
        #endregion

        #region Public Events
        public event Action OnCollectionEnterButton;
        public event Action OnCollectionExitButton;
        #endregion

        #region Fields
        private IDisposable _token;
        [HideInInspector] public RectTransform _enterButtonTransform;
        private RectTransform _collectionPanelTransform;
        [HideInInspector] public RectTransform _collectionExitButtonTransform;
        #endregion

        #region Public Methods
        public void LoadEnterButtonOnPanel(ITokenReceiver tokenReciever)
        {
            _token = tokenReciever.GetToken();
            if (BeforeCollectionEnterButtonPress != null)
                BeforeCollectionEnterButtonPress.Invoke();
            _enterButtonTransform = TrackerHandler.GetTracker(collectionEnterButtonTrackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _enterButtonTransform);
            ComboCollectorEnterButtonTutorialTapDetector.OnButtonPress += StartAfterDelay;
        }
        public void LoadExitComboCollectionButton(ITokenReceiver tokenReciever)
        {
            _token = tokenReciever.GetToken();
            if (BeforeCollectionExitButtonPress != null)
                BeforeCollectionExitButtonPress.Invoke();
            _collectionExitButtonTransform = TrackerHandler.GetTracker(collectionExitButtonTrackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _collectionExitButtonTransform);
            ComboCollectionBackButtonTutorialTapDetector.OnButtonPress += AfterExitButtonPress;
        }

        public void WaitForPlayerEnterCollection(ITokenReceiver tokenReciever)
        {
            _token = tokenReciever.GetToken();
            ComboCollectorEnterButtonTutorialTapDetector.OnButtonPress += InvokeAfterEnterButtonPress;
        }

        private void InvokeAfterEnterButtonPress()
        {
            if (AfterCollectionEnterButtonPress != null)
                AfterCollectionEnterButtonPress.Invoke();
            ComboCollectionBackButtonTutorialTapDetector.OnButtonPress += InvokeAfterExitButtonPress;
        }

        #endregion

        #region Private Methods
        private void StartAfterDelay()
        {
            StartCoroutine(AfterEnterButtonPressed());
        }

        private IEnumerator AfterEnterButtonPressed()
        {
            yield return null;
            ComboCollectorEnterButtonTutorialTapDetector.OnButtonPress -= StartAfterDelay;
            if (AfterCollectionEnterButtonPress != null)
                AfterCollectionEnterButtonPress.Invoke();

            if (OnCollectionEnterButton != null)
                OnCollectionEnterButton.Invoke();
            TutorialClickHelper.Instance.ReturnObjects();
            _collectionPanelTransform = TrackerHandler.GetTracker(collectionPanelTrackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _collectionPanelTransform);
            ReleaseToken();
        }

        private void AfterExitButtonPress()
        {
            ComboCollectionBackButtonTutorialTapDetector.OnButtonPress -= AfterExitButtonPress;
            if (AfterCollectionExitButtonPress != null)
                AfterCollectionExitButtonPress.Invoke();

            if (OnCollectionExitButton != null)
                OnCollectionExitButton.Invoke();
            TutorialClickHelper.Instance.ReturnObjects();
            ReleaseToken();
        }

        private void InvokeAfterExitButtonPress()
        {
            if (AfterCollectionExitButtonPress != null)
                AfterCollectionExitButtonPress.Invoke();
            ReleaseToken();
        }

        private void ReleaseToken()
        {
            if (_token != null)
                _token.Dispose();

            else
                Debug.LogError("No token to release");
        }
        #endregion
    }
}

