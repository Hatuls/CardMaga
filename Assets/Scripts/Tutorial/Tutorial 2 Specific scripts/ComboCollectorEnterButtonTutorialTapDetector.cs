using CardMaga.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCollectorEnterButtonTutorialTapDetector : MonoBehaviour
{
    [SerializeField] Button _comboCollectionEnterButton;

    public static event Action OnButtonPress;

    private void OnEnable()
    {
        _comboCollectionEnterButton.OnClick += CheckForPlayerPress;
    }

    private void CheckForPlayerPress()
    {
        if (OnButtonPress != null)
            OnButtonPress.Invoke();
    }

    private void OnDestroy()
    {
        _comboCollectionEnterButton.OnClick -= CheckForPlayerPress;
    }
}
