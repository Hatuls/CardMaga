using Account.GeneralData;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    [System.Serializable]
    public class ComboTypeVisualAssigner : BaseVisualAssigner<ComboCore>
    {
        [SerializeField] ComboTypeVisualSO _comboTypeVisualSO;
        [SerializeField] Image _comboTypeImage;
        public override void CheckValidation()
        {
            _comboTypeVisualSO.CheckValidation();

            if (_comboTypeImage == null)
                throw new System.Exception("ComboTypeVisualAssigner has no Combo Type Image");
        }
        public override void Init(ComboCore comboData)
        {
                _comboTypeImage.AssignSprite(_comboTypeVisualSO.GetTypeSprite(comboData.ComboSO().GoToDeckAfterCrafting));
        }
        public override void Dispose()
        {

        }

    }
}
