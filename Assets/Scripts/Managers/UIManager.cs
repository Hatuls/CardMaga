
using Battle;
using Managers;
using ReiTools.TokenMachine;
using UnityEngine;


public class UIManager : MonoSingleton<UIManager>
{
    #region Fields
    public static Vector2 MiddleScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

    #endregion
    public override void Init(ITokenReciever token)
    {
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
#endif

    }

    #region Monobehaviour Callbacks 
    public override void Awake()
    {
        base.Awake();
        SceneStarter.Register(new OperationTask(Init, 0));
    }
    #endregion
}
