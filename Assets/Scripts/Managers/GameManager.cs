using ReiTools.TokenMachine;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace CardMaga.Managers.GameManager
{
    public class GameManager : MonoBehaviour
    {
        TokenMachine _tokenMachine;

        public static event Action<ITokenReciever> OnEnteringTheGame;
        [SerializeField] private UnityEvent OnApplicationStart;
        
        private void Start()
        {
       
            //   OnApplicationStart?.Invoke();
            _tokenMachine = new TokenMachine( OnFirstEnterTheGame);
           
           using (_tokenMachine.GetToken())
           {
               if (AudioManager.Instance == null)
                   Debug.Log("Creating Audio Manager");
                GC.Collect();
                FireBaseHandler.Init();
                OnEnteringTheGame?.Invoke(_tokenMachine);
           }
        }
        
        private void OnFirstEnterTheGame()
        {
            // _loader.LoadScenes(null, _firstScene.SceneBuildIndex);
            OnApplicationStart?.Invoke();
        }
    }
}
