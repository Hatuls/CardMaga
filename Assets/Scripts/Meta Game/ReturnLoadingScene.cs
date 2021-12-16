
using TMPro;
using UnityEngine;

public class ReturnLoadingScene : MonoBehaviour
{
    public static SceneHandler.ScenesEnum GoToScene;
    [SerializeField] TextMeshProUGUI _returnText;
    [SerializeField] GameObject _mainPanel;
    private void Start()
    {
        if (_mainPanel.activeSelf)
         _mainPanel.SetActive(false);
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
        Close();
    }
    public void Close()
    => _mainPanel.SetActive(false);
}
