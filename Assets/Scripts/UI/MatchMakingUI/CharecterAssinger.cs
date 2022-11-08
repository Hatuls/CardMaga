using Battle.Characters;
using UnityEngine;
using BattleCharacter = Battle.Characters.BattleCharacter;

namespace CardMaga.UI.MatchMMaking
{
    public class CharecterAssinger : MonoBehaviour
    {
        [SerializeField] private ComboAssinger _comboAssinger;
        [SerializeField] private CharecterPortraitAssinger _charecterPortraitAssinger;

        public void AssingCharecter(BattleCharacter character)
        {
            CharacterBattleData data = character.CharacterData;
            
            _comboAssinger.AssingCombosUI(data.ComboRecipe);
            _charecterPortraitAssinger.AssignCharacter(data.CharacterSO.CharacterSprite,character.DisplayName);
        }
    }

}
