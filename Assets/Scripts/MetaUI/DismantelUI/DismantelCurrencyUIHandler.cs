using TMPro;
using UnityEngine;

namespace CardMaga.MetaUI.DismantelUI
{
    public class DismantelCurrencyUIHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void UpdateText(int amount)
        {
            _text.text = amount.ToString();
        }
    }
}