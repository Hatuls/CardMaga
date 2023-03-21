using System;
using CardMaga.Input;
using TMPro;
using UnityEngine;

public class MessagePopUpHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text _title;
    [SerializeField] private TMP_Text _message;

    [SerializeField] private Button _conformButton;

    private Action OnConform;

    public void Init(string title, string message, Action onConform)
    {
        _title.text = title;
        _message.text = message;
        OnConform = onConform;

        _conformButton.OnClick += Click;
    }

    private void Click()
    {
        OnConform?.Invoke();
    }

    private void OnDestroy()
    {
        _conformButton.OnClick -= Click;
    }
}
