
using UnityEngine;

public interface ITouchable
{
     void OnFirstTouch(in Vector2 touchPos);
     void OnReleaseTouch(in Vector2 touchPos);
     void OnHoldTouch(in Vector2 touchPos,in Vector2 startPos);


}
