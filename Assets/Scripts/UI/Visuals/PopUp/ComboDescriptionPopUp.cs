using Battle.Combo;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    [System.Serializable]
    public class ComboDescriptionPopUp : BaseDescriptionPopUp<BattleComboData>
    {
        [SerializeField] ComboTypeVisualSO _comboTypeVisualSO;
        public override void CheckValidation()
        {
            base.CheckValidation();
            _comboTypeVisualSO.CheckValidation();
        }
        public override void Init(BattleComboData battleComboDataData)
        {
            ActivatePopUP(true);
            PopUpText.AssignText(_comboTypeVisualSO.GetTypeDescription(battleComboDataData.GoToDeckAfterCrafting));
        }
        public override void Dispose()
        {
            ActivatePopUP(false);
        }
    }
}
