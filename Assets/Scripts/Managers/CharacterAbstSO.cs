using UnityEngine;
namespace Battles
{
    [System.Serializable]
    public abstract class CharacterAbstSO : ScriptableObject
    {
        [SerializeField] CharactersEnum _opponent;
        [SerializeField] CharacterDifficulty _characterDifficulty;
        [SerializeField] Characters.Stats.CharacterStats _stats;
        [SerializeField] Cards.CardSO[] _characterCards;


       Cards.Card[] _cards;

        public Cards.Card[] GetCharacterCards {
            get
            {
                if (_cards == null || _cards.Length == 0)
                {
                    _cards = new Cards.Card[_characterCards.Length];

                    for (int i = 0; i < _characterCards.Length; i++)
                        _cards[i] = Managers.CardManager.CreateCard(_opponent == CharactersEnum.Player,_characterCards[i].GetCardName);
                }

                return   _cards; 
            }
        }
        public CharactersEnum GetOpponent => _opponent;
        public CharacterDifficulty GetDifficulty => _characterDifficulty;
        public Characters.Stats.CharacterStats GetCharacterStats => _stats;
    }

    public enum CharactersEnum
    {
        Player,
        Enemy
    }
    public enum CharacterDifficulty
    {
        Player,
        Easy,
        Medium,
        Hard,
        Elite,
        LegendREI
    }
}
