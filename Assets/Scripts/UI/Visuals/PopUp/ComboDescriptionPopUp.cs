using Battle.Combo;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    [System.Serializable]
    public class ComboDescriptionPopUp : BaseDescriptionPopUp<ComboData>
    {
        [SerializeField] ComboTypeVisualSO _comboTypeVisualSO;
        public override void CheckValidation()
        {
            base.CheckValidation();
            _comboTypeVisualSO.CheckValidation();
        }
        public override void Init(ComboData comboDataData)
        {
            ActivatePopUP(true);
            PopUpText.AssignText(_comboTypeVisualSO.GetTypeDescription(comboDataData.GoToDeckAfterCrafting));
        }
        public override void Dispose()
        {
            ActivatePopUP(false);
        }
    }
}
