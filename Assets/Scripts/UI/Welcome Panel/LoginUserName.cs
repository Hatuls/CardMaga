﻿using Account;
using CardMaga.Playfab;
using CardMaga.Server.Request;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI
{


    public class LoginUserName : BaseUIElement
    {
#if UNITY_EDITOR
        private const int MIN_NAME_REQUIREMENTS = 0;
#elif !UNITY_EDITOR
        private const int MIN_NAME_REQUIREMENTS = 2;
#endif
        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField]
        private Button _enterNameBtn;
        private IDisposable _loginToken;
        private bool _isNewlyCreated;
#region Monobehaviour
        private void Awake()
        {
            _inputField.onValueChanged.AddListener(TextFieldValueChange);
            _enterNameBtn.onClick.AddListener(TrySetName);
            PlayfabLogin.OnSuccessfullLogin += OpenLoginText;
        }

        private void OnDestroy()
        {
            _inputField.onValueChanged.RemoveListener(TextFieldValueChange);
            _enterNameBtn.onClick.RemoveListener(TrySetName);
            PlayfabLogin.OnSuccessfullLogin -= OpenLoginText;

        }
#endregion

        public void RegisterToken(ITokenReciever tokenReciever)
        {
            _loginToken = tokenReciever.GetToken();

            if (_isNewlyCreated)
                Show();
            else
                ProceedNext();
        }

        #region Private 
        private void OpenLoginText(LoginResult loginResult)
        {
            _isNewlyCreated = loginResult.NewlyCreated;
        }
        private void TextFieldValueChange(string text)
        {
            bool isLengthOkay = text.Length > MIN_NAME_REQUIREMENTS;
                _enterNameBtn.gameObject.SetActive(isLengthOkay);
        }

        private void TrySetName()
        {
            if (true)
                SetName();
        }
        private void SetName()
        {
            string userName = _inputField.text;

            TokenMachine tokenMachine = new TokenMachine(ProceedNext);
            new SetNameRequest(userName).SendRequest(tokenMachine);
        }


        private void ProceedNext()
        {
            Hide();
            if (_loginToken != null)
                _loginToken.Dispose();
        }
#endregion
    }

    public class SetNameRequest : BaseServerRequest
    {
        private readonly string _currentName;
        public SetNameRequest(string name)
        {
            _currentName = name;
        }
        protected override void ServerLogic()
        {
            var request = new UpdateUserTitleDisplayNameRequest
            {
                AuthenticationContext = AccountManager.Instance.LoginResult.AuthenticationContext,
                DisplayName = _currentName
            };
            PlayFab.PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnSuccess, OnFail);

        }

        private void OnFail(PlayFabError obj)
        {
            string failedText = "Failed To Set Name";
            Debug.LogError(failedText + "\nError: " + obj.ErrorMessage);
            SetLocalName(failedText);
        }

        private void OnSuccess(UpdateUserTitleDisplayNameResult obj)
        {
            SetLocalName(_currentName);
        }

        private void SetLocalName(string name)
        {
            AccountManager.Instance.Data.DisplayName = name;
            ReceiveResult();
        }
    }

}