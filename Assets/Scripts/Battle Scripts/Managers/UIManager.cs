
using ReiTools.TokenMachine;
using UnityEngine;


public class UIManager : MonoSingleton<UIManager>
{
    #region Fields
    public static Vector2 MiddleScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

    #endregion
    public override void Init(IRecieveOnlyTokenMachine token)
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    #region Monobehaviour Callbacks 
    public override void Awake()
    {
        base.Awake();
        BattleSceneManager.OnBattleSceneLoaded += Init;
    }
    public void OnDestroy()
    {
        BattleSceneManager.OnBattleSceneLoaded -= Init;
    }
    #endregion
}
