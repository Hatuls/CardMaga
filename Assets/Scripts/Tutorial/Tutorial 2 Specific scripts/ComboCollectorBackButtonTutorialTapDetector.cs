using CardMaga.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCollectorBackButtonTutorialTapDetector : MonoBehaviour
{
    [SerializeField] Button _comboCollectionExitButton;

    public static event Action OnButtonPress;

    private void OnEnable()
    {
        _comboCollectionExitButton.OnClick += CheckForPlayerPress;
    }

    private void CheckForPlayerPress()
    {
        if (OnButtonPress != null)
            OnButtonPress.Invoke();
    }

    private void OnDestroy()
    {
        _comboCollectionExitButton.OnClick -= OnButtonPress;
    }
}
