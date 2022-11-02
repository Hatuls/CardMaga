using UnityEngine;
using UnityEngine.UI;

public class TestCards : BaseView
{
    [SerializeField] private UnityEngine.UI.Button _button;
    public override void Init()
    {
        _button.onClick.AddListener(Back);
    }

    private void Back()
    {
        ViewWindowHandler.Instance.ShowLast();
    }
    private void OnDestroy()
    {
        _button.onClick.RemoveListener(Back);
    }
}
