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
    TextMeshProUGUI _expText;
    [SerializeField]
    ObserverSO _observerSO;

    [SerializeField]
    GameObject _endScreen;
    const string _winTitle = "Loot Gained";
    const string _loseTitle = "Loot Gained";

    [SerializeField]
    DiamondRewardUI _totalReward;
    [SerializeField]
    DiamondRewardUI _tutorialReward;
    [SerializeField] DiamondRewardUI[] _defaultRewards;

    [SerializeField]
    GameObject _tutorialRewardContainer;
    [SerializeField]
    GameObject _defaultRewardContainer;

    [SerializeField]
    SequenceHandler _tutorialSequence;
    [SerializeField]
    SequenceHandler _defaultSequence;

    // Start is called before the first frame update
    //void Start()
    //{
    //    LoadingProgressBar.OnFinishLoadingScene += OnFinishGame;
    //}
    //private void OnDestroy()
    //{
    //    LoadingProgressBar.OnFinishLoadingScene -= OnFinishGame;
    //}
    private void OnFinishGame()
    {
        if (Account.AccountManager.Instance.BattleData.IsFinishedPlaying)
        {
    //    LoadingProgressBar.OnFinishLoadingScene -= OnFinishGame;
            _defaultRewardContainer.SetActive(false);
            _tutorialRewardContainer.SetActive(false);
            FinishGame();
        }
    }

    public void FinishGame()
    {
        _defaultSequence.StopSequence();
        _tutorialSequence.StopSequence();
        _observerSO.Notify(this);
        SendData();
        ActivateContainer();
        _endScreen.SetActive(true);
        SetTexts();
        ActivateAnimationSequence();
    }

    private void ActivateAnimationSequence()
    {
        if (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
        {
            _tutorialSequence.StartSequance();
        }
        else
        {
            _defaultSequence.StartSequance();
        }
    }

    private void ActivateContainer()
    {


        if (Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
        {
            _tutorialRewardContainer.SetActive(true);
        }
        else
        {
            _defaultRewardContainer.SetActive(true);
        }
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

        UnityAnalyticHandler.SendEvent("road_path", data);
        FireBaseHandler.SendEvent("road_path", fireBaseParameters.ToArray());
    }
    public void SetTexts()
    {
        var battledata = Account.AccountManager.Instance.BattleData;
        if (battledata.Opponent.CharacterData.CharacterSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
        {
            _totalReward.SetText(battledata[Battles.CharacterTypeEnum.Tutorial].Diamonds.ToString());
        }
        else
        {
            const int offset = 3;
            for (int i = 0; i < _defaultRewards.Length; i++)
            {
                var character = (Battles.CharacterTypeEnum)(i + offset);
                _defaultRewards[i].SetText(battledata[character].Diamonds.ToString());
            }

        }
        _totalReward.SetText(battledata.GetAllDiamonds().ToString());
        _expText.text = battledata.GetAllExp().ToString();

    }

    public void ReturnToMainMenu()
    {
        var data = Account.AccountManager.Instance.BattleData;
        CameraMovement.ResetCameraMovementLocation();
        ReturnLoadingScene.GoToScene = ScenesEnum.MainMenuScene;
     //   SceneHandler.LoadScene(ReturnLoadingScene.GoToScene);
        var accountData = Account.AccountManager.Instance.AccountGeneralData;
        accountData.AccountResourcesData.Diamonds.AddValue(data.GetAllDiamonds());
        accountData.AccountLevelData.Exp.AddValue(data.GetAllExp());
    }

    public void OnNotify(IObserver Myself)
    {
        //   throw new NotImplementedException();
    }
}
