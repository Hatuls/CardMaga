using UnityEngine;
using TMPro;
public class KeywordInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _title;
    [SerializeField] TextMeshProUGUI _description;
    public void SetKeywordName(string text)
          => _title.text = text;

    public void SetKeywordDescription(string Text)
        => _description.text = Text;
}
