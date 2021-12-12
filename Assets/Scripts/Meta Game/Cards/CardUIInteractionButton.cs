using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUIInteractionButton : MonoBehaviour
{
    [SerializeField]
    Button _button;
    [SerializeField]
    TextMeshProUGUI _buttonText;

    public void SetActive(bool setActive) => gameObject.SetActive(setActive);
    public void SetText(string text) => _buttonText.text = text;

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    protected virtual void AssingParams()
    {
        _button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();

    }
#endif
}
