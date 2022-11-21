using System;
using TMPro;
using UnityEngine;

public class InputFieldHandler : MonoBehaviour
{
    public event Action<string> OnValueChange;

    [SerializeField] private TMP_InputField _inputField;

    private void Awake()
    {
        _inputField.onSubmit.AddListener(InputFieldUpdate);
    }

    private void OnDestroy()
    {
        _inputField.onSubmit.RemoveListener(InputFieldUpdate);
    }

    private void InputFieldUpdate(string text)
    {
        OnValueChange?.Invoke(text);
    }

    public void SetText(string text)
    {
        _inputField.text = text;
    }
}
