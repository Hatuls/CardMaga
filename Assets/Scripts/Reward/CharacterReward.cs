
using Account.GeneralData;
using Battle;
using Factory;
using PlayFab;
using PlayFab.ClientModels;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
namespace CardMaga.Rewards
{
    [Serializable]
    public class CharacterReward : IRewardable
    {
        public event Action OnServerSuccessfullyAdded;
        public event Action OnServerFailedToAdded;

        [SerializeField] private string _name;
        [SerializeField] private int _characterID;
      
        public string Name => _name;

        private IDisposable _token;

        public void TryRecieveReward(ITokenReciever tokenMachine)
        {
            _token = tokenMachine.GetToken();
            AddToDevicesData();
            UpdateOnServer();
        }

        private void UpdateOnServer()
        {
            var request = new ExecuteCloudScriptRequest()
            {
                FunctionName = "AddCharacter",
                FunctionParameter = new
                {
                    CharactersData = JsonUtility.ToJson(Account.AccountManager.Instance.Data.CharactersData)
                }
            };
            Account.AccountManager.Instance.UpdateDataOnServer();

            PlayFabClientAPI.ExecuteCloudScript(request, OnRewardReceived, OnFailedToReceived);
        }

        private void OnFailedToReceived(PlayFabError obj)
        {
            OnServerFailedToAdded?.Invoke();
            _token.Dispose();
        }

        private void OnRewardReceived(ExecuteCloudScriptResult obj)
        {
            OnServerSuccessfullyAdded?.Invoke();
            Debug.Log("Character Added Successfully");
            _token.Dispose();
        }
        public void AddToDevicesData()
        {
            CharacterSO characterSo = GameFactory.Instance.CharacterFactoryHandler.GetCharacterSO(_characterID);

            Account.AccountManager.Instance.Data.CharactersData.AddCharacter(new Character(characterSo));

        }

#if UNITY_EDITOR
        public void Init(string name, int characterID)
        {
            _name = name;
            _characterID = characterID;
        }

   

#endif
    }
}