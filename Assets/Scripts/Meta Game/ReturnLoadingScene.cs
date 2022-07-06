using UnityEngine;

public class ReturnLoadingScene : MonoBehaviour
{
    public static ScenesEnum GoToScene;

    [SerializeField] GameObject _mainPanel;
    // Need To be Re-Done
    private void Start()
    {

        //Close();
        //var battledata = Account.AccountManager.Instance.BattleData;
        //switch (GoToScene)
        //{
        //    case ScenesEnum.MapScene:
        //        if (battledata.Map != null &&
        //            battledata.Player != null &&
        //            battledata.Player.CharacterData != null)
        //            OpenPanel();
        //        break;
        //    case ScenesEnum.GameBattleScene:
        //        if (battledata.Player != null &&
        //            battledata.Player.CharacterData != null &&
        //            battledata.Opponent != null &&
        //            battledata.Opponent.CharacterData != null)
        //            OpenPanel();
        //        else if (battledata.Player != null &&
        //            battledata.Player.CharacterData != null &&
        //            battledata.Map != null)
        //        {
        //   //         SceneHandler.LoadScene(SceneHandler.ScenesEnum.MapScene);
        //            Close();
        //        }


        //        break;
        //    default:
        //        break;
        //}
    }
    private void OpenPanel()
    {
        _mainPanel.SetActive(true);
    }
    public void SwitchScene()
    {
        //SceneHandler.LoadScene(GoToScene);
        Close();
    }
    public void Close()
    => _mainPanel.SetActive(false);
}
