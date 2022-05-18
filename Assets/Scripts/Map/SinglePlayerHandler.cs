
using Battles;
using Characters;
using Map.UI;
using UnityEngine;

public class SinglePlayerHandler : MonoBehaviour
{
    private static SinglePlayerHandler _instance;
    public static SinglePlayerHandler Instance => _instance;
 [SerializeField] SceneLoaderCallback _sceneloaderEvent;

    public void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        if (Account.AccountManager.Instance.BattleData.Player == null)
            StartNewRun();    }
    public static void RegisterPlayerCharacterDataForNewRun(Character playerCharacter) => Account.AccountManager.Instance.BattleData.Player = playerCharacter;
    public void StartNewRun(Character playerLoadOut = null)
    {

        if (playerLoadOut== null)
        {
            Debug.LogWarning("SingePlayerHandler: Character playerLoadOut class is null.\nCreateing Character from SO");
            playerLoadOut = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(CharacterTypeEnum.Player);
        }
        var data = Account.AccountManager.Instance.BattleData;
        data.PlayerWon = false;
       Debug.Log($"Registering Player: {playerLoadOut.CharacterData.CharacterSO.CharacterName}");
        data.Player = playerLoadOut;
        UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref data.Player.CharacterData.CharacterStats);
    }

    public void RegisterOpponent(Character opponent)
    {
        Debug.Log($"Registering Opponent: {opponent.CharacterData.CharacterSO.CharacterName}\nDifficulty: {opponent.CharacterData.CharacterSO.CharacterDiffciulty}");
        Account.AccountManager.Instance.BattleData.Opponent = opponent;
    }


    public void Battle()
    {

        if (Account.AccountManager.Instance.BattleData.Player == null)
            StartNewRun();
  
        Debug.Log($"Start battle!\n{Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterSO.CharacterName} VS {Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterName}");
    //    _sceneloaderEvent.LoadScene(SceneHandler.ScenesEnum.GameBattleScene);
    }


}

