using Battles;
using DesignPattern;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        if (BattleData.IsFinishedPlaying)
        {
            FinishGame();
        }
        else
        {
            _observerSO.Notify(null);
            if (_endScreen.activeSelf)
                _endScreen.SetActive(false);
        }
    }

    public void FinishGame()
    {
        _observerSO.Notify(this);
        _endScreen.SetActive(true);
        SetTexts();
    }

    public void SetTexts()
    {
        var rewards = BattleData.MapRewards;
        _expText.text = rewards.EXP.ToString();
        _diamondText.text = rewards.Diamonds.ToString();
    }

    public void ReturnToMainMenu()
    {
        CameraMovement.ResetCameraMovementLocation();
        SceneHandler.LoadScene(SceneHandler.ScenesEnum.MainMenuScene);
        var accountData = Account.AccountManager.Instance.AccountGeneralData;
        accountData.AccountResourcesData.Diamonds.AddValue(BattleData.MapRewards.Diamonds);
        accountData.AccountLevelData.Exp.AddValue(BattleData.MapRewards.EXP);
    }

    public void OnNotify(IObserver Myself)
    {
        throw new NotImplementedException();
    }
}
