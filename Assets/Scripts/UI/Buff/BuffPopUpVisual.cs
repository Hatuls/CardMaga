using CardMaga.Keywords;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMaga.UI.Visuals
{

    public class BuffPopUpVisual : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title, _context;
        [SerializeField]
        private Image _img;
        [SerializeField]
        private BuffCollectionVisualSO _buffCollectionVisualSO;
        public void SetVisual(KeywordSO keywordSO, int amount)
        {
            _img.sprite = _buffCollectionVisualSO.GetBuffSO(keywordSO.GetKeywordType).BuffIcon;
            _title.text = keywordSO.GetKeywordType.ToString();
            _context.text = keywordSO.GetDescription(amount);
        }
    }
}
