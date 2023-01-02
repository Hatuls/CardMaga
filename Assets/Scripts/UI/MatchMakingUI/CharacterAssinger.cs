using Account.GeneralData;
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

            ComboCore[] comboCores = new ComboCore[data.ComboRecipe.Length];

            for (int i = 0; i < data.ComboRecipe.Length; i++)
            {
                comboCores[i] = data.ComboRecipe[i].ComboCore;
            }
            
            _comboAssinger.AssingCombosUI(comboCores);
            _characterPortraitAssinger.AssignCharacter(character.BattleCharacterVisual.BattleVisualCharacter.Portrait,character.DisplayName);
        }
    }

}
