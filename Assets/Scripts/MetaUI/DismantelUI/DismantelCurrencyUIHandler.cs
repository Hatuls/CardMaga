using CardMaga.UI;
using TMPro;
using UnityEngine;

namespace CardMaga.MetaUI.DismantelUI
{
    public class DismantelCurrencyUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private void OnEnable()
        {
            UpdateText(0, 0);
        }
        public void UpdateText(int chipAmount, int goldAmount)
        {
            const string space = "                      ";
            _text.text =  string.Concat(chipAmount.ToString().AddImageInFrontOfText(0), space, goldAmount.ToString().AddImageInFrontOfText(1));
        }
    }
}