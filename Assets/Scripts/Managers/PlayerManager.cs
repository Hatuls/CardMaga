using Battles;
using Characters.Stats;
using UnityEngine;


namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager>, IBattleHandler
    {
        #region Fields
        [Tooltip("Player Stats: ")]
        [SerializeField] CharacterStats _characterStats;
        [SerializeField] private CharacterSO _myCharacter;
        [SerializeField] Cards.Card[] _deck;
        [SerializeField] CharacterSO.RecipeInfo[] _recipes;

     [SerializeField]   AnimatorController _playerAnimatorController;


        #endregion
        public ref Characters.Stats.CharacterStats GetCharacterStats => ref _characterStats;
        public Cards.Card[] Deck => _deck;

        public CharacterSO.RecipeInfo[] Recipes => _recipes;
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
        {
            _myCharacter = characterSO;
            _characterStats = characterSO.CharacterStats;
            CharacterStatsManager.RegisterCharacterStats(true, ref _characterStats);

            var CardInfo = characterSO.Deck;
            _deck = new Cards.Card[CardInfo.Length];
            var cardFactory = Factory.GameFactory.Instance.CardFactoryHandler;
            for (int i = 0; i < CardInfo.Length; i++)
                _deck[i] = cardFactory.CreateCard(CardInfo[i].Card, CardInfo[i].Level);

            Battles.Deck.DeckManager.Instance.InitDeck(true, _deck);

              _recipes = characterSO.Combos;

            PlayerAnimatorController.ResetAnimator();
        }

        public void RestartBattle()
        {
            if (_myCharacter != null)
            {
                AssignCharacterData(_myCharacter);
                UpdateStats();
            }
        }

        public void OnEndTurn() => _playerAnimatorController.ResetLayerWeight();



        public void UpdateStats()
        {
            Battles.UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(true, _characterStats.MaxHealth);
            Battles.UI.StatsUIManager.GetInstance.InitHealthBar(true, _characterStats.Health);
            Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(true, _characterStats.Shield);

        }

        public void OnEndBattle()
        {
            throw new System.NotImplementedException();
        }
    }

}
