using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMainManu : BaseView
{
    [SerializeField] private UnityEngine.UI.Button _button;
    public override void Init()
    {
        _button.onClick.AddListener(GoToCards);
    }

    private void GoToCards()
    {
        ViewWindowHandler.Instance.Show<TestCards>();
    }
}
