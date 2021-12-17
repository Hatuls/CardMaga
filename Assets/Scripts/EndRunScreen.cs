using DesignPattern;
using System;
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
        string floor = "Floor ";
        Dictionary<string, object> data = new Dictionary<string, object>();

        data.Add("Map:", map.configName);
        for (int i = 0; i < map.path.Count; i++)
        {
            var Node = map.GetNode(map.path[i]);
            data.Add(string.Concat(floor, i), Node.NodeTypeEnum.ToString());
        }

        AnalyticsHandler.SendEvent("Road Path", data);
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
        SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);
        ReturnLoadingScene.GoToScene = SceneHandler.ScenesEnum.MapScene;
        var accountData = Account.AccountManager.Instance.AccountGeneralData;
        accountData.AccountResourcesData.Diamonds.AddValue(data.MapRewards.Diamonds);
        accountData.AccountLevelData.Exp.AddValue(data.MapRewards.EXP);
    }

    public void OnNotify(IObserver Myself)
    {
        throw new NotImplementedException();
    }
}
