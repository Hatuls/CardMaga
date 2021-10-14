using Battles;
using Characters.Stats;
using UnityEngine;


namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager>, IBattleHandler
    {
        #region Fields

        [SerializeField] Character _characterData;

        [SerializeField] CharacterSO.RecipeInfo[] _recipes;

        [SerializeField]   AnimatorController _playerAnimatorController;


        #endregion
        public ref CharacterStats GetCharacterStats => ref _characterData.CharacterStats;
        public Cards.Card[] Deck => _characterData?.CharacterDeck;
        public Combo.Combo[] Recipes => _characterData.ComboRecipe;

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
        public void AssignCharacterData(CharacterSO characterSO)
         =>  AssignCharacterData(new Character(characterSO));
        
        public void AssignCharacterData(Character characterData)
        {
            this._characterData = characterData;
            CharacterStatsManager.RegisterCharacterStats(true, ref characterData.CharacterStats);
            Battles.Deck.DeckManager.Instance.InitDeck(true, Deck);
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
