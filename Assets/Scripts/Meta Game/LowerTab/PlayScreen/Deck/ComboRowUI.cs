using Art;
using Battle.Combo;
using TMPro;
using UnityEngine;

namespace UI.Meta.PlayScreen
{
    public class ComboRowUI: MonoBehaviour
    {
        #region Fields
        [SerializeField]
        TextMeshProUGUI _comboName;
        [SerializeField]
        TextMeshProUGUI _levelText;
        [SerializeField]
        BodyPartGFX _comboBodyPart;
        [SerializeField]
        BodyPartGFX[] _comboRecipe;
        #endregion
        #region Public Methods
        public void Init(ComboSO combo, int comboLevel, ArtSO artSO)
        {
            ResetComboRecipeSlots();
            _comboName.text = combo.ComboName;
            _levelText.text = $"LVL {comboLevel}";
            var cardTypePalette = artSO.GetPallette<CardTypePalette>();
            var iconCollection = artSO.IconCollection;
            _comboBodyPart.AssignBodyPart(combo.CraftedCard.CardType);
            for (int i = 0; i < combo.ComboSequence.Length; i++)
            {
                _comboRecipe[i].gameObject.SetActive(true);
                _comboRecipe[i].AssignBodyPart(combo.ComboSequence[i]);
            }
        }
        private void ResetComboRecipeSlots()
        {
            for (int i = 0; i < _comboRecipe.Length; i++)
            {
                _comboRecipe[i].gameObject.SetActive(false);
            }
        }
        #endregion
    }
}