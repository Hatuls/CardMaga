using CardMaga.UI;
using CardMaga.UI.PopUp;
using UnityEngine;

public class LoginNamePopUpTerminal : BasePopUpTerminal
{
    [SerializeField] private string _title;
    [SerializeField] private string _message;
    
    [SerializeField] private LoginUserName _loginUserName;

    private void Awake()
    {
        _loginUserName.SetNameRequest.OnFailedName += ShowPopUp;
    }

    protected override void OnDestroy()
    {
        _loginUserName.SetNameRequest.OnFailedName -= ShowPopUp;
        base.OnDestroy();
    }

    protected override void ShowPopUp()
    {
        base.ShowPopUp();
        _currentActivePopUp.transform.localPosition = Vector3.zero;
        var messagePopUpHandler = _currentActivePopUp.transform.GetComponent<MessagePopUpHandler>();
        messagePopUpHandler.Init(_title,_message,HidePopUp);
    }

    protected override Vector2 GetStartLocation()
    {
        return PopUpManager.Instance.GetPosition(_startLocation);
    }
}
