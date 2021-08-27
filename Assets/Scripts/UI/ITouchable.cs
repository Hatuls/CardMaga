
using UnityEngine;

public interface ITouchable
{
    RectTransform Rect { get; }
    bool IsInteractable { get; }
     void ResetTouch();
     void OnFirstTouch(in Vector2 touchPos);
     void OnReleaseTouch(in Vector2 touchPos);
     void OnHoldTouch(in Vector2 touchPos,in Vector2 startPos);
}
