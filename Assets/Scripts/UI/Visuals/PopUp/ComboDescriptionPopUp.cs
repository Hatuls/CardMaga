using Account.GeneralData;
using CardMaga.UI.Visuals;
using UnityEngine;

namespace CardMaga.UI.PopUp
{
    [System.Serializable]
    public class ComboDescriptionPopUp : BaseDescriptionPopUp<ComboCore>
    {
        [SerializeField] ComboTypeVisualSO _comboTypeVisualSO;
        public override void CheckValidation()
        {
            base.CheckValidation();
            _comboTypeVisualSO.CheckValidation();
        }
        public override void Init(ComboCore comboData)
        {
            ActivatePopUP(true);
            PopUpText.AssignText(_comboTypeVisualSO.GetTypeDescription(comboData.ComboSO().GoToDeckAfterCrafting));
        }
        public override void Dispose()
        {
            ActivatePopUP(false);
        }
    }
}
