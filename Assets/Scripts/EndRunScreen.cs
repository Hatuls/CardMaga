using DesignPattern;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EndRunScreen : MonoBehaviour, IObserver
{
    public static bool _firstTime = true;

    [SerializeField]
    TextMeshProUGUI _title;
    [SerializeField]
    TextMeshProUGUI _diamondText;
    [SerializeField]
    TextMeshProUGUI _expText;
    [SerializeField]
    ObserverSO _observerSO;

    [SerializeField]
    GameObject _endScreen;
    const string _winTitle = "Loot Gained";
    const string _loseTitle = "Loot Gained";


    // Start is called before the first frame update
    void Start()
    {
        if (Account.AccountManager.Instance.BattleData.IsFinishedPlaying)
        {
            FinishGame();
        }
        //else
        //{
        //    _observerSO.Notify(null);
        //    if (_endScreen.activeSelf)
        //        _endScreen.SetActive(false);
        //}
    }

    public void FinishGame()
    {
        _observerSO.Notify(this);
        SendData();
        _endScreen.SetActive(true);
        SetTexts();
    }
    private void SendData()
    {
        var PlayerData = Account.AccountManager.Instance.BattleData;
        var map = PlayerData.Map;
        string floor = "floor_";
        Dictionary<string, object> data = new Dictionary<string, object>(map.path.Count);
        var fireBaseParameters = new List<Firebase.Analytics.Parameter>(data.Count);

        data.Add("map:", map.configName);
        fireBaseParameters.Add(new Firebase.Analytics.Parameter("map", map.configName));
        for (int i = 0; i < map.path.Count; i++)
        {
            var Node = map.GetNode(map.path[i]);
            string name = string.Concat(floor, i);
            var nodeType = Node.NodeTypeEnum.ToString();
            data.Add(name, nodeType);
            fireBaseParameters.Add(new Firebase.Analytics.Parameter(name.Replace(' ', '_'), nodeType));
        }

        AnalyticsHandler.SendEvent("road_path", data);
        FireBaseHandler.SendEvent("road_path", fireBaseParameters.ToArray());
    }
    public void SetTexts()
    {
        var rewards = Account.AccountManager.Instance.BattleData.MapRewards;
        _expText.text = rewards.EXP.ToString();
        _diamondText.text = rewards.Diamonds.ToString();
    }

    public void ReturnToMainMenu()
    {
        var data = Account.AccountManager.Instance.BattleData;
        CameraMovement.ResetCameraMovementLocation();
        ReturnLoadingScene.GoToScene = SceneHandler.ScenesEnum.MainMenuScene;
        SceneHandler.LoadScene(ReturnLoadingScene.GoToScene);
        var accountData = Account.AccountManager.Instance.AccountGeneralData;
        accountData.AccountResourcesData.Diamonds.AddValue(data.MapRewards.Diamonds);
        accountData.AccountLevelData.Exp.AddValue(data.MapRewards.EXP);
    }

    public void OnNotify(IObserver Myself)
    {
        //   throw new NotImplementedException();
    }
}
