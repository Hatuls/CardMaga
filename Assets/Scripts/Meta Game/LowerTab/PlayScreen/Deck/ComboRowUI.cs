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
        public void Init(Combo.ComboSO combo, byte comboLevel)
        {
            ResetComboRecipeSlots();
            _comboName.text = combo.ComboName;
            _levelText.text = $"LVL {comboLevel}";
            _comboBodyPart.Init(combo.CraftedCard);
            for (int i = 0; i < combo.ComboSequance.Length; i++)
            {
                _comboRecipe[i].gameObject.SetActive(true);
                _comboRecipe[i].Init(combo.ComboSequance[i]);
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