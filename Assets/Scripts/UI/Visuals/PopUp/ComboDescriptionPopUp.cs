using Battle.Combo;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    [System.Serializable]
    public class ComboDescriptionPopUp : BaseDescriptionPopUp<Combo>
    {
        [SerializeField] ComboTypeVisualSO _comboTypeVisualSO;
        public override void CheckValidation()
        {
            base.CheckValidation();
            _comboTypeVisualSO.CheckValidation();
        }
        public override void Init(Combo comboData)
        {
            PopUpText.AssignText(_comboTypeVisualSO.GetTypeDescription(comboData.GoToDeckAfterCrafting));
        }
        public override void Dispose()
        {
            ActivatePopUP(false);
        }
    }
}
