
using UnityEngine;

public interface ITouchable
{
    Battle.UI.CardUIAttributes.CardStateMachine.CardUIInput State { get; }

    RectTransform Rect { get; }
    bool IsInteractable { get; }
    void ResetTouch();
    void OnStateEnter();
    void OnStateExit();
    void OnTick(in Touch touchPos);
    void OnMouse();
}