using Battles;
using UnityEngine;


namespace Managers
{



    public class PlayerManager : MonoSingleton<PlayerManager>, IBattleHandler
    {
        #region Fields
        [Tooltip("Player Stats: ")]
        [SerializeField] Characters.Stats.CharacterStats _characterStats;
        [SerializeField] private CharacterSO _myCharacter;
        [SerializeField] Cards.Card[] _deck;
        [SerializeField] CharacterSO.RecipeInfo[] _recipes;

        AnimatorController _playerAnimatorController;


        #endregion
        public ref Characters.Stats.CharacterStats GetCharacterStats => ref _characterStats;
        public Cards.Card[] Deck => _deck;

        public CharacterSO.RecipeInfo[] Recipes => _recipes;
        public AnimatorController PlayerAnimatorController => _playerAnimatorController;
        public override void Init()
        {
        }


        public void AssignCharacterData(CharacterSO characterSO)
        {
            _myCharacter = characterSO;
            _characterStats = characterSO.CharacterStats;


            var CardInfo = characterSO.Deck;
            _deck = new Cards.Card[CardInfo.Length];
            for (int i = 0; i < CardInfo.Length; i++)
                _deck[i] = CardManager.Instance.CreateCard(CardInfo[i].Card, CardInfo[i].Level);

            Battles.Deck.DeckManager.Instance.InitDeck(true, _deck);

              _recipes = characterSO.Combos;
        }

        public void RestartBattle()
        {
            if (_myCharacter != null)
            {
                AssignCharacterData(_myCharacter);
                UpdateStats();
            }
        }

        public void UpdateStats()
        {
            Battles.UI.StatsUIManager.GetInstance.UpdateMaxHealthBar(true, _characterStats.MaxHealth);
            Battles.UI.StatsUIManager.GetInstance.UpdateShieldBar(true, _characterStats.Shield);
            Battles.UI.StatsUIManager.GetInstance.UpdateHealthBar(true, _characterStats.Health);

        }

        public void OnEndBattle()
        {
            throw new System.NotImplementedException();
        }
    }

}
