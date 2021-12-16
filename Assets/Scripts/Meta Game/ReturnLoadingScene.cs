
using TMPro;
using UnityEngine;

public class ReturnLoadingScene : MonoBehaviour
{
    public static SceneHandler.ScenesEnum GoToScene = SceneHandler.ScenesEnum.NetworkScene;

    [SerializeField] GameObject _mainPanel;
    private void Start()
    {

            Close();
        switch (GoToScene)
        {
            case SceneHandler.ScenesEnum.MapScene:
            case SceneHandler.ScenesEnum.GameBattleScene:
                OpenPanel();
                break;
            default:
                break;
        }
    }
    private void OpenPanel()
    {
        _mainPanel.SetActive(true);
    }
    public void SwitchScene()
    {
        SceneHandler.LoadScene(GoToScene);
        GoToScene = SceneHandler.ScenesEnum.NetworkScene;
        Close();
    }
    public void Close()
    => _mainPanel.SetActive(false);
}
