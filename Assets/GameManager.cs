using CardMaga.LoadingScene;
using ReiTools.TokenMachine;
using System;
using UnityEngine;
namespace CardMaga.Managers.GameManager
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LoadingSceneManager _loader;
        TokenMachine _tokenMachine;

        public static event Action<ITokenReciever> OnEnteringTheGame;

        private void Start()
        {
            _tokenMachine = new TokenMachine( OnFirstEnterTheGame);
            using (_tokenMachine.GetToken())
            OnEnteringTheGame?.Invoke(_tokenMachine);
        }
        private void OnFirstEnterTheGame()
        {
            _loader.LoadScenes(null, 1);
        }
    }
}
