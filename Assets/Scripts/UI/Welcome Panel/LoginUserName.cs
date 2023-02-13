using Account;
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
        private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField]
        private Button _enterNameBtn;
        private IDisposable _loginToken;
        private SetNameRequest _setNameRequest;
#region Monobehaviour
        private void Awake()
        {
             _setNameRequest = new SetNameRequest();
            _setNameRequest.OnFailedName += ShowError;

            _inputField.onValueChanged.AddListener(TextFieldValueChange);
            _enterNameBtn.onClick.AddListener(TrySetName);
        }

        private void OnDestroy()
        {
            _inputField.onValueChanged.RemoveListener(TextFieldValueChange);
            _enterNameBtn.onClick.RemoveListener(TrySetName);
            _setNameRequest.OnFailedName -= ShowError;
        }
#endregion

        public void RegisterToken(ITokenReceiver tokenReciever)
        {
            _loginToken = tokenReciever.GetToken();
            var account = AccountManager.Instance;
            if (account.LoginResult.NewlyCreated)
                Show();
            else
            {
                var tokenMAchine = new TokenMachine(ProceedNext);
                GetUserName(tokenMAchine);
            }

        }

        #region Private 

        private void TextFieldValueChange(string text)
        {
            bool isLengthOkay = text.Length > MIN_NAME_REQUIREMENTS;
                _enterNameBtn.gameObject.SetActive(isLengthOkay);
        }

        private void TrySetName()
        {
                SetName();
        }
        private void ShowError()
        {
            _textMeshProUGUI.gameObject.SetActive(true);
            _textMeshProUGUI.text = _setNameRequest.FeedBack;
        }
        private void SetName()
        {
            string userName = _inputField.text;

            TokenMachine tokenMachine = new TokenMachine(ProceedNext);
            _setNameRequest.SetName(userName.Trim());
            _setNameRequest.SendRequest(tokenMachine);
        }


        private void ProceedNext()
        {
            Hide();
            if (_loginToken != null)
                _loginToken.Dispose();
        }
        IDisposable _tokenName;
        private void GetUserName(ITokenReceiver tokenMachine)
        {
            _tokenName = tokenMachine.GetToken();
            var request = new GetPlayerProfileRequest
            {
                AuthenticationContext = AccountManager.Instance.LoginResult.AuthenticationContext,
                PlayFabId = AccountManager.Instance.PlayfabID,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true,
                }
            };
            PlayFab.PlayFabClientAPI.GetPlayerProfile(request, OnSuccessReceivingName, OnFailedToReceiveName);
        }

        private void OnFailedToReceiveName(PlayFabError obj)
        {
            AccountManager.Instance.DisplayName = "Error";
            throw new Exception("Error while trying to retrieve name\nError: " + obj.ErrorMessage);
           // _tokenName.Dispose();
        }

        private void OnSuccessReceivingName(GetPlayerProfileResult obj)
        {

            AccountManager.Instance.DisplayName = obj.PlayerProfile.DisplayName;
            _tokenName.Dispose();
            
        }
        #endregion
    }

    public class SetNameRequest : BaseServerRequest
    {
        private string _currentName;
        public string FeedBack;
        public bool IsSuccess;

        public event Action OnFailedName;
        public void SetName(string name)
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
            FeedBack = obj.ErrorMessage;
            IsSuccess = false;
            OnFailedName?.Invoke();
        }

        private void OnSuccess(UpdateUserTitleDisplayNameResult obj)
        {
            IsSuccess = true;
            GetUserName();
        }

  
        private void GetUserName()
        {
            var request = new GetPlayerProfileRequest {
                AuthenticationContext = AccountManager.Instance.LoginResult.AuthenticationContext,
                PlayFabId = AccountManager.Instance.PlayfabID,
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true,
                }
            };
            PlayFab.PlayFabClientAPI.GetPlayerProfile(request, OnSuccessReceivingName, OnFailedToReceiveName);
        }

        private void OnFailedToReceiveName(PlayFabError obj)
        {
            AccountManager.Instance.DisplayName = "Error";
            throw new Exception("Error while trying to retrieve name\nError: " + obj.ErrorMessage);
        }

        private void OnSuccessReceivingName(GetPlayerProfileResult obj)
        {
            
            AccountManager.Instance.DisplayName = obj.PlayerProfile.DisplayName;
            ReceiveResult();
        }
    }

}