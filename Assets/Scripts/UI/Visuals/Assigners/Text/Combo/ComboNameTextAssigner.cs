using Battle.Combo;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class ComboNameTextAssigner : BaseTextAssigner<ComboData>
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

        public override void Init(ComboData comboDataData)
        {
            _comboNameText.AssignText(comboDataData.ComboSO.ComboName);
        }
    }
}