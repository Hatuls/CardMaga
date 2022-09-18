using TMPro;
using UnityEngine;
namespace CardMaga.UI.PopUp
{
    [System.Serializable]
    public abstract class BaseDescriptionPopUp<T> : BaseVisualHandler<T>
    {
        public GameObject PopUpHolder;
        public TextMeshProUGUI PopUpText;

        public override void CheckValidation()
        {
            if (PopUpHolder == null)
                throw new System.Exception($"ComboDescriptionPopUp of {this.ToString()}has no Holder");
            if (PopUpText == null)
                throw new System.Exception($"ComboDescriptionPopUp of {this.ToString()} has no Text");
        }
    }
}
