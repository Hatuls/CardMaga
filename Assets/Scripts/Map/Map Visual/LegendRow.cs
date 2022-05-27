using CardMaga;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LegendRow : MonoBehaviour
{
    [SerializeField] Image _img;
    [SerializeField] TextMeshProUGUI _text;
    internal void Init(NodePointAbstSO eventPointAbstSO)
    {
        _text.text = eventPointAbstSO.Name;
        //_img.sprite = eventPointAbstSO.Icon;
        _img.sprite = eventPointAbstSO.Icon;
    }
}
