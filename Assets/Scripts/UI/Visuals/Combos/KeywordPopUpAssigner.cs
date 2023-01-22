using CardMaga.UI.Text;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.Visuals
{
    public class KeywordPopUpAssigner : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _title, _context;
        [SerializeField]
        private DescriptionColorSO _descriptionColorSO;
        public void SetVisual(KeywordTextAssigner keywordTextAssigner)
        {
            _title.text = keywordTextAssigner.KeywordName.ColorString(_descriptionColorSO.DescriptionColor);
            _context.text = keywordTextAssigner.KeywordDescription;
        }
    }

}