﻿using Battle;
using Battle.Characters;
using Characters;
using Characters.Stats;
using ReiTools.TokenMachine;
using UnityEngine;
using Battle.Combo;

namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager>, IBattleHandler
    {
        #region Fields

        [SerializeField] Character _character;

        [SerializeField] Combo _recipes;

        [SerializeField] AnimatorController _playerAnimatorController;
        [SerializeField] AnimationBodyPartSoundsHandler _soundAnimation;
        #endregion
        static int Counter = 0;
        public ref CharacterStats GetCharacterStats => ref _character.CharacterData.CharacterStats;
        Cards.Card[] _playerDeck;
        public Cards.Card[] Deck => _playerDeck;
        public Battle.Combo.Combo[] Recipes => _character.CharacterData.ComboRecipe;

        public AnimatorController PlayerAnimatorController
        {
            get
            {
                if (_playerAnimatorController == null)
                {
                    var animators = FindObjectsOfType<AnimatorController>();

                    if (animators != null && animators.Length > 0)
                    {
                        foreach (var anim in animators)
                        {
                            if (anim.tag == "Player")
                            {
                                _playerAnimatorController = anim;
                                break;
                            }
                        }
                    }

                }
                return _playerAnimatorController;
            }

        }
        public override void Init(ITokenReciever token)
        {
        }
        public void AssignCharacterData(Character characterData)
        {
            Instantiate(characterData.CharacterData.CharacterSO.CharacterAvatar, _playerAnimatorController.transform);
            Debug.LogWarning("<a>Spawning " + Counter++ + " </a>");
            _character = characterData;
            var data = characterData.CharacterData;
            _soundAnimation.CurrentCharacter = data.CharacterSO;

            int Length = data.CharacterDeck.Length;

            _playerDeck = new Cards.Card[Length];

            System.Array.Copy(data.CharacterDeck, _playerDeck, Length);

            CharacterStatsManager.RegisterCharacterStats(true, ref data.CharacterStats);
            Battle.Deck.DeckManager.Instance.InitDeck(true, _playerDeck);
            PlayerAnimatorController.ResetAnimator();
        }
        public void UpdateStatsUI()
        {
            var statsui = Battle.UI.StatsUIManager.Instance;
            statsui.UpdateMaxHealthBar(true, GetCharacterStats.MaxHealth);
            statsui.InitHealthBar(true, GetCharacterStats.Health);
            statsui.UpdateShieldBar(true, GetCharacterStats.Shield);
        }


        public void OnEndTurn()
            => _playerAnimatorController.ResetLayerWeight();

        public void PlayerWin()
        {
            _playerAnimatorController.CharacterWon();
            _character.CharacterData.CharacterSO.VictorySound.PlaySound();
        }


        public void RestartBattle()
        {
            throw new System.NotImplementedException();
        }

        public void OnEndBattle()
        {
            throw new System.NotImplementedException();
        }




        #region Monobehaviour Callbacks 
        public override void Awake()
        {
            base.Awake();
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            SceneHandler.OnBeforeSceneShown -= Init;
        }
        #endregion
    }

}
