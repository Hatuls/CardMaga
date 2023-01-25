using Account;
using CardMaga.ValidatorSystem;
using System;
using TMPro;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    public class ValidationPopUpHandler : MonoBehaviour
    {
        private const string Title = "Error: ";


        [SerializeField]
        private TextMeshProUGUI _errorTitle;
        [SerializeField]
        private TextMeshProUGUI _errorContext;
        [SerializeField]
        private TextMeshProUGUI _playfabContext;

        private Action OnClose;
        public void AssignVisuals(IValidFailedInfo valid, Action onClose)
        {
            _errorTitle.text = string.Concat(Title, valid.ID);
            _playfabContext.text = string.Concat("User ID: " , AccountManager.Instance.PlayfabID);
            _errorContext.text = string.Concat(valid.Message);
            OnClose = onClose;
        }

        public void Close() => OnClose?.Invoke();
    }
}