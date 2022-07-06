using DesignPattern;
using ReiTools.TokenMachine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EndRunScreen : MonoBehaviour, IObserver
{
    public static bool _firstTime = true;
    public static event Action OnFinishGame;
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
    GameObject _returnButton;

    [SerializeField]
    OperationManager _tutorialSequence;
    [SerializeField]
    OperationManager _defaultSequence;

    [SerializeField,Tooltip("The scene that will be moved to when pressing on the end button")]
    private SceneIdentificationSO _scene;

    private ISceneHandler _sceneHandler;
    #region Monobehaviour Callbacks
    private void Awake()
    {
        SceneHandler.OnSceneHandlerActivated += Inject;

    }
    void Start()
    {
        _returnButton.SetActive(false);
        CheckIfFinishGame();
    }
    private void OnDestroy()
    {
        SceneHandler.OnSceneHandlerActivated -= Inject;
    }
    #endregion

    private void Inject(ISceneHandler sh)
        => _sceneHandler = sh;


    // maybe remove will need to see
    // Need To be Re-Done
    private void CheckIfFinishGame()
    {
        //if (Account.AccountManager.Instance.BattleData.IsFinishedPlaying)
        //{
        //    _defaultRewardContainer.SetActive(false);
        //    _tutorialRewardContainer.SetActive(false);
        //    FinishGame();
        //}
    }

    public void FinishGame()
    {
        OnFinishGame?.Invoke();
        _observerSO.Notify(this);
        SendData();
        ActivateContainer();
        _endScreen.SetActive(true);
        SetTexts();
        ActivateAnimationSequence();
    }
    // Need To be Re-Done
    private void ActivateAnimationSequence()
    {
        //var opponentSO = Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO;

        //TokenMachine t = new TokenMachine(ActivateReturnButton);
        //if (opponentSO!= null && opponentSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
        //{
        //    _tutorialSequence.Init(t);
        //    _tutorialSequence.StartOperation();
        //}
        //else
        //{
        //    _defaultSequence.Init(t);
        //    _defaultSequence.StartOperation();
        //}
    }
    // Need To be Re-Done
    private void ActivateContainer()
    {
        //var opponent = Account.AccountManager.Instance.BattleData.Opponent;
        //var charaterData = opponent.CharacterData.CharacterSO;
        //if (charaterData != null && charaterData.CharacterType == Battles.CharacterTypeEnum.Tutorial)
        //{
        //    _tutorialRewardContainer.SetActive(true);
        //}
        //else
        //{
        //    _defaultRewardContainer.SetActive(true);
        //}
    }
    private void ActivateReturnButton() => _returnButton.SetActive(true);
    // Need To be Re-Done
    private void SendData()
    {
        //var PlayerData = Account.AccountManager.Instance.BattleData;
        //var map = PlayerData.Map;
        //string floor = "floor_";
        //Dictionary<string, object> data = new Dictionary<string, object>(map.path.Count);
        //var fireBaseParameters = new List<Firebase.Analytics.Parameter>(data.Count);

        //data.Add("map:", map.configName);
        //fireBaseParameters.Add(new Firebase.Analytics.Parameter("map", map.configName));
        //for (int i = 0; i < map.path.Count; i++)
        //{
        //    var Node = map.GetNode(map.path[i]);
        //    string name = string.Concat(floor, i);
        //    var nodeType = Node.NodeTypeEnum.ToString();
        //    data.Add(name, nodeType);
        //    fireBaseParameters.Add(new Firebase.Analytics.Parameter(name.Replace(' ', '_'), nodeType));
        //}

        //UnityAnalyticHandler.SendEvent("road_path", data);
        //FireBaseHandler.SendEvent("road_path", fireBaseParameters.ToArray());
    }
    // Need To be Re-Done
    public void SetTexts()
    {
        //var battledata = Account.AccountManager.Instance.BattleData;
        //var opponentSO = battledata.Opponent.CharacterData.CharacterSO;
        //if (opponentSO!= null && opponentSO.CharacterType == Battles.CharacterTypeEnum.Tutorial)
        //{
        //    _totalReward.SetText(battledata[Battles.CharacterTypeEnum.Tutorial].Diamonds.ToString());
        //}
        //else
        //{
        //    const int offset = 3;
        //    for (int i = 0; i < _defaultRewards.Length; i++)
        //    {
        //        var character = (Battles.CharacterTypeEnum)(i + offset);
        //        _defaultRewards[i].SetText(battledata[character]?.Diamonds.ToString() ?? "0" );
        //    }

        //}
        //_totalReward.SetText(battledata.GetAllDiamonds().ToString());
        //_expText.text = battledata.GetAllExp().ToString();

    }
    // Need To be Re-Done
    public void ReturnToMainMenu()
    {
        //var data = Account.AccountManager.Instance.BattleData;
        //CameraMovement.ResetCameraMovementLocation();
        //var accountData = Account.AccountManager.Instance.AccountGeneralData;
        //accountData.AccountResourcesData.Diamonds.AddValue(data.GetAllDiamonds());
        //accountData.AccountLevelData.Exp.AddValue(data.GetAllExp());
        //_sceneHandler.MoveToScene(_scene);
    }

    public void OnNotify(IObserver Myself)
    {
        //   throw new NotImplementedException();
    }
}
