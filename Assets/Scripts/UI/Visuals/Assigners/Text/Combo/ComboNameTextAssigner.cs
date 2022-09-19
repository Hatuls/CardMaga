using Battle.Combo;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class ComboNameTextAssigner : BaseTextAssigner<Combo>
    {
        [SerializeField]TextMeshProUGUI _comboNameText;
        public override void CheckValidation()
        {
            if (_comboNameText == null)
                throw new System.Exception("ComboNameTextAssigner has no combo name Text");
        }

        public override void Dispose()
        {

        }

        public override void Init(Combo comboData)
        {
            _comboNameText.AssignText(comboData.ComboSO.ComboName);
        }
    }
}