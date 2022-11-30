using CardMaga.Trackers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.Battle.UI;
using System.Collections;

namespace CardMaga.UI.Collections
{
    public class TutorialComboCollectionHandler : MonoBehaviour
    {
        [SerializeField] TrackerID collectionEnterButtonTrackerID;
        [SerializeField] TrackerID collectionPanelTrackerID;
        [SerializeField] TrackerID collectionExitButtonTrackerID;
        [SerializeField] private UnityEvent BeforeCollectionEnterButtonPress;
        [SerializeField] private UnityEvent AfterCollectionEnterButtonPress;
        [SerializeField] private UnityEvent BeforeCollectionExitButtonPress;
        [SerializeField] private UnityEvent AfterCollectionExitButtonPress;

        private IDisposable _token;
        [HideInInspector] public RectTransform _enterButtonTransform;
        private RectTransform _collectionPanelTransform;
        [HideInInspector] public RectTransform _collectionExitButtonTransform;

        public event Action OnCollectionEnterButton;
        public event Action OnCollectionExitButton;

        public void LoadEnterButtonOnPanel(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            if (BeforeCollectionEnterButtonPress != null)
                BeforeCollectionEnterButtonPress.Invoke();
            _enterButtonTransform = TrackerHandler.GetTracker(collectionEnterButtonTrackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _enterButtonTransform);
            ComboCollectorEnterButtonTutorialTapDetector.OnButtonPress += StartAfterDelay;
        }

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

        public void LoadExitComboCollectionButton(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            if (BeforeCollectionExitButtonPress != null)
                BeforeCollectionExitButtonPress.Invoke();
            _collectionExitButtonTransform = TrackerHandler.GetTracker(collectionExitButtonTrackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _collectionExitButtonTransform);
            ComboCollectorBackButtonTutorialTapDetector.OnButtonPress += AfterExitButtonPress;
        }

        private void AfterExitButtonPress()
        {
            ComboCollectorBackButtonTutorialTapDetector.OnButtonPress -= AfterExitButtonPress;
            if (AfterCollectionExitButtonPress != null)
                AfterCollectionExitButtonPress.Invoke();

            if (OnCollectionExitButton != null)
                OnCollectionExitButton.Invoke();
            TutorialClickHelper.Instance.ReturnObjects();
            ReleaseToken();
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

