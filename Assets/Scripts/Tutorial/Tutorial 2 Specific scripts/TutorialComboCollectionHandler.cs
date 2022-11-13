using CardMaga.Trackers;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;
using CardMaga.Battle.UI;

namespace CardMaga.UI.Collections
{
    public class TutorialComboCollectionHandler : MonoBehaviour
    {
        [SerializeField] TrackerID trackerID;
        [SerializeField] private DialoguesFlow _dialoguesFlow5;
        [SerializeField] private UnityEvent OnCollectionButtonLoadOnPanel;
        [SerializeField] private UnityEvent OnCollectionButtonExitPanel;

        private IDisposable _token;
        private RectTransform _enterButtonTransform;


        public void LoadButtonOnPanel(ITokenReciever tokenReciever)
        {
            _token = tokenReciever.GetToken();
            _dialoguesFlow5.gameObject.SetActive(true);
            if (OnCollectionButtonLoadOnPanel != null)
                OnCollectionButtonLoadOnPanel.Invoke();
            _enterButtonTransform = TrackerHandler.GetTracker(trackerID).RectTransform;
            TutorialClickHelper.Instance.LoadObject(true, true, null, _enterButtonTransform);
            ComboCollectorTutorialTapDetector.OnButtonPress += AfterEnterButtonPress;
        }

        private void AfterEnterButtonPress()
        {
            if (OnCollectionButtonExitPanel != null)
                OnCollectionButtonExitPanel.Invoke();
            _dialoguesFlow5.gameObject.SetActive(false);
            ComboCollectorTutorialTapDetector.OnButtonPress -= AfterEnterButtonPress;
            ReleaseToken();
        }

        public void AfterCollectioOpen()
        {
            //BattleUiManager.Instance.ComboAndDeckCollectionBattleHandler;
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

