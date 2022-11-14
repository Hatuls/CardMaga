using Battle.Characters;
using UnityEngine;
namespace CardMaga.UI.MatchMMaking
{
    public class CharacterAssinger : MonoBehaviour
    {
        [SerializeField] private ComboAssinger _comboAssinger;
        [SerializeField] private CharacterPortraitAssinger _characterPortraitAssinger;

        public void AssingCharecter(BattleCharacter character)
        {
            CharacterBattleData data = character.CharacterData;
            
            _comboAssinger.AssingCombosUI(data.ComboRecipe);
            _characterPortraitAssinger.AssignCharacter(character.BattleCharacterVisual.BattleVisualCharacter.Portrait,character.DisplayName);
        }
    }

}
