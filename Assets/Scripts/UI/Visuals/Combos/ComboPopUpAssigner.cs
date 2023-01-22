using Account.GeneralData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{
    public class ComboPopUpAssigner : MonoBehaviour
    {
        [SerializeField]
        private ComboTypeVisualSO _comboTypeVisualSO;
        [SerializeField]
        private TextMeshProUGUI _title, _context;
        [SerializeField]
        private Image _img;
        public void SetVisual(ComboCore comboCore)
        {
            var comboSO = comboCore.ComboSO();
            var type = comboSO.GoToDeckAfterCrafting;
            _title.text = _comboTypeVisualSO.GetTypeName(type);
            _context.text = _comboTypeVisualSO.GetTypeDescription(type);
            _img.sprite = _comboTypeVisualSO.GetTypeSprite(type);
        }
    }

}