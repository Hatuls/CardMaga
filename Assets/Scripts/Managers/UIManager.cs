using UnityEngine;


public class UIManager : MonoBehaviour
{
    #region Fields
    public static Vector2 MiddleScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

    public int Priority => 99;
    #endregion


    #region Monobehaviour Callbacks 
    public void Awake()
    {
#if !UNITY_EDITOR
                Cursor.lockState = CursorLockMode.Confined;
#endif
    }


    #endregion
}
