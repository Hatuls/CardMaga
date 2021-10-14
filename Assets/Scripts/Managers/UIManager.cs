
using UnityEngine;


public class UIManager : MonoSingleton<UIManager>
{
    #region Fields
    public static Vector2 MiddleScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

    #endregion
    public override void Init()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
