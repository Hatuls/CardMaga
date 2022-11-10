using Battle.Combo;
using CardMaga.UI.Visuals;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Text
{
    [System.Serializable]
    public class ComboTypeTextAssigner : BaseTextAssigner<BattleComboData>
    {
        [SerializeField]ComboTypeVisualSO _comboTypeVisualSO;
        [SerializeField]TextMeshProUGUI _comboTypeNameText;
        public override void CheckValidation()
        {
            _comboTypeVisualSO.CheckValidation();

            if (_comboTypeNameText == null)
                throw new System.Exception("ComboTypeTextAssigner has no comboTypeNameText");
        }
        public override void Init(BattleComboData battleComboDataData)
        {
            _comboTypeNameText.AssignText(_comboTypeVisualSO.GetTypeName(battleComboDataData.GoToDeckAfterCrafting));
        }

        public override void Dispose()
        {

        }
    }
}