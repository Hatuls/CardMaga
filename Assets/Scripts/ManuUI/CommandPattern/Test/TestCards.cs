using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UI;

public class TestCards : BaseView
{
    [SerializeField] private Button _button;
    public override void Init()
    {
        _button.onClick.AddListener(Back);
    }

    private void Back()
    {
        ViewWindowHandler.Instance.ShowLast();
    }
}
