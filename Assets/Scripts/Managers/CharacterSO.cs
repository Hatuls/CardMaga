using UnityEngine;
namespace Battles
{
    [System.Serializable]
    public class CharacterSO : ScriptableObject
    {
        public int ID { get; private set; }


        [SerializeField] CharacterTypeEnum _characterType;
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
                        _cards[i] = Managers.CardManager.CreateCard(_characterType == CharacterTypeEnum.Class_1,_characterCards[i].CardName);
                }

                return   _cards; 
            }
        }
        public CharacterTypeEnum GetOpponent => _characterType;
        public CharacterDifficulty GetDifficulty => _characterDifficulty;
        public Characters.Stats.CharacterStats GetCharacterStats => _stats;



        public bool Init(string[] row, CardsCollectionSO cardsCollection, Collections.ComboRecipeCollectionSO comboRecipeCollections)
        {
            const int ID = 0;
            if (row[ID] == "-" || row[ID] == "")
                return false;
           this.ID = int.Parse(row[ID]);

            #region Sheet Data Information
            // Data
            const int CharacterType = 1;
            const int Difficulty = 2;
            const int CharacterName = 3;
            const int ModelName = 4;

            // stats
            const int MaxHP = 5;
            const int Defense = 6;
            const int StartingStamina = 7;
            const int CardDraw = 8;
            const int Gold = 9;
            const int StrengthPoint = 10;
            const int DexterityPoint = 11;

            //deckCards
            const int Deck = 12;

            //Recipe 
            const int Recipes = 13;

            //Rewards
            const int Rewards = 14;
            #endregion

            if (int.TryParse(row[CharacterType], out int characterTypeIndex))
            {

            }
            else
            {
                Debug.LogError($"Coulmne A, ID = {row[ID]}");
                return false;
            }











            return true ;
        }
    }

    public enum CharacterTypeEnum
    {
        None = 0,
        Class_1 = 1,
        Class_2 = 2,
        Class_3 = 3,
        Class_4 = 4,
        Basic_Enemy =5,
        Elite_Enemy =6,
        Boss__Enemy =7,
    }
    public enum CharacterDifficulty
    {
        None =0,
        Player = 1,
        Tutorial =2,
        Easy=3,
        Medium =4,
        Hard = 5,
    }
}
