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
        [SerializeField] private DialoguesFlow _dialoguesFlow5;
        [SerializeField] private DialoguesFlow _dialoguesFlow7;
        [SerializeField] private UnityEvent BeforeCollectionEnterButtonPress;
        [SerializeField] private UnityEvent AfterCollectionEnterButtonPress;
        [SerializeField] private UnityEvent BeforeCollectionExitButtonPress;
        [SerializeField] private UnityEvent AfterCollectionExitButtonPress;

        private IDisposable _token;
        private RectTransform _enterButtonTransform;
        private RectTransform _collectionPanelTransform;
        private RectTransform _collectionExitButtonTransform;


        public void LoadEnterButtonOnPanel(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            _dialoguesFlow5.gameObject.SetActive(true);
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
            TutorialClickHelper.Instance.ReturnObjects();
            _dialoguesFlow5.gameObject.SetActive(false);
            _collectionPanelTransform = TrackerHandler.GetTracker(collectionPanelTrackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _collectionPanelTransform);
            ReleaseToken();
        }

        public void LoadExitComboCollectionButton(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            _dialoguesFlow7.gameObject.SetActive(true);
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
            TutorialClickHelper.Instance.ReturnObjects();
            _dialoguesFlow7.gameObject.SetActive(false);
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

