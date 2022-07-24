﻿using Battle;
using Battle.Characters;
using Characters;
using Characters.Stats;
using ReiTools.TokenMachine;
using UnityEngine;
using Battle.Combo;
using CardMaga.UI;
using System;
using System.Collections.Generic;
using CardMaga.Card;

namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager>, IBattleHandler
    {
        #region Fields

        [SerializeField] Character _character;
        [SerializeField] Combo _recipes;


        [SerializeField] MetaCardUIFilterScreen _cardUIFilter;
        [SerializeField] MetaComboUIFilterScreen _comboUIFilter;
        [SerializeField] CardSort[] _cardSorters;
        [SerializeField] ComboSort[] _comboSorters;

        [SerializeField] AnimatorController _playerAnimatorController;
        [SerializeField] AnimationBodyPartSoundsHandler _soundAnimation;
        #endregion
        static int Counter = 0;
        public ref CharacterStats GetCharacterStats => ref _character.CharacterData.CharacterStats;
        CardData[] _playerDeck;
        public CardData[] GetDeck() => _playerDeck;
        public Combo[] GetCombos() => _character.CharacterData.ComboRecipe;

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

        
       //     Debug.LogWarning("<a>Spawning " + Counter++ + " </a>");
            _character = characterData;
            var data = characterData.CharacterData;
            _soundAnimation.CurrentCharacter = data.CharacterSO;

            int Length = data.CharacterDeck.Length;

            _playerDeck = new CardData[Length];
            Array.Copy(data.CharacterDeck, _playerDeck, Length);
            Battle.Deck.DeckManager.Instance.InitDeck(true, _playerDeck);

      
            CharacterStatsManager.RegisterCharacterStats(true, ref data.CharacterStats);
            PlayerAnimatorController.ResetAnimator();
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
            for (int i = 0; i < _cardSorters.Length; i++)
                _cardSorters[i].OnSortingCollectionRequested += GetDeck;


            for (int i = 0; i < _comboSorters.Length; i++)
                _comboSorters[i].OnSortingCollectionRequested += GetCombos;

       //     _cardUIFilter.OnCollectionNeeded += GetDeck;
       //     _comboUIFilter.OnCollectionNeeded += GetCombos;
            SceneHandler.OnBeforeSceneShown += Init;
        }
        public void OnDestroy()
        {
            //_comboUIFilter.OnCollectionNeeded -= GetCombos;
            //_cardUIFilter.OnCollectionNeeded -= GetDeck;
            for (int i = 0; i < _cardSorters.Length; i++)
               _cardSorters[i].OnSortingCollectionRequested -= GetDeck;

            for (int i = 0; i < _comboSorters.Length; i++)
                _comboSorters[i].OnSortingCollectionRequested -= GetCombos;

            SceneHandler.OnBeforeSceneShown -= Init;
        }
        #endregion
    }

}
