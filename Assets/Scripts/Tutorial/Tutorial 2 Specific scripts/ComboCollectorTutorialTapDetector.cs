using CardMaga.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCollectorTutorialTapDetector : MonoBehaviour
{
    [SerializeField] Button _comboCollectionButton;

    public static event Action OnButtonPress;

    private void OnEnable()
    {
        _comboCollectionButton.OnClick += CheckForPlayerPress;
    }

    private void CheckForPlayerPress()
    {
        if (OnButtonPress != null)
            OnButtonPress.Invoke();
    }

    private void OnDestroy()
    {
        _comboCollectionButton.OnClick -= OnButtonPress;
    }
}
