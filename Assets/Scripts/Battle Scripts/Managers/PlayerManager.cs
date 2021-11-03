﻿using Battles;
using Characters;
using Characters.Stats;
using UnityEngine;


namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager>, IBattleHandler
    {
        #region Fields

        [SerializeField] Character _character;

        [SerializeField] CharacterSO.RecipeInfo[] _recipes;

        [SerializeField]   AnimatorController _playerAnimatorController;


        #endregion
        public ref CharacterStats GetCharacterStats => ref _character.CharacterData.CharacterStats;
        Cards.Card[] _playerDeck;
        public Cards.Card[] Deck => _playerDeck;
        public Combo.Combo[] Recipes => _character.CharacterData.ComboRecipe;

        public AnimatorController PlayerAnimatorController
        {
            get {
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
        public override void Init()
        {
        }

        public void AssignCharacterData(Character characterData)
        {
            this._character = characterData;
            var data = characterData.CharacterData;

            int Length = data.CharacterDeck.Length;

            _playerDeck = new Cards.Card[Length];

            System.Array.Copy(data.CharacterDeck, _playerDeck, Length);

            CharacterStatsManager.RegisterCharacterStats(true, ref data.CharacterStats);
            Battles.Deck.DeckManager.Instance.InitDeck(true, _playerDeck);
            PlayerAnimatorController.ResetAnimator();
        }
         public void UpdateStatsUI()
        {

            Battles.UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(true, GetCharacterStats.MaxHealth);
            Battles.UI.StatsUIManager.GetInstance.InitHealthBar(true, GetCharacterStats.Health);
            Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(true, GetCharacterStats.Shield);
        }

       
        public void OnEndTurn() 
            => _playerAnimatorController.ResetLayerWeight();



     
 public void RestartBattle()
        {
            throw new System.NotImplementedException();
        }

        public void OnEndBattle()
        {
            throw new System.NotImplementedException();
        }
    }

}