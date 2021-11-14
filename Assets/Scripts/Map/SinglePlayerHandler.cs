
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
        if (BattleData.Player == null)
            StartNewRun();
        else if (BattleData.Player.CharacterData.CharacterStats.Health <= 0)
            throw new System.Exception($"SingePlayerHandler: BattleData.Player's health is 0!\n need to return to main menu!");
            
    }
    public static void RegisterPlayerCharacterDataForNewRun(Character playerCharacter) => BattleData.Player = playerCharacter;
    public void StartNewRun(Character playerLoadOut = null)
    {
        if (playerLoadOut== null)
        {
            Debug.LogWarning("SingePlayerHandler: Character playerLoadOut class is null.\nCreateing Character from SO");
            playerLoadOut = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(CharacterTypeEnum.Player);
        }

        Debug.Log($"Registering Player: {playerLoadOut.CharacterData.Info.CharacterName}");
        BattleData.Player = playerLoadOut;
        UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref BattleData.Player.CharacterData.CharacterStats);
    }

    public void RegisterOpponent(Character opponent)
    {
        Debug.Log($"Registering Opponent: {opponent.CharacterData.Info.CharacterName}\nDifficulty: {opponent.CharacterData.Info.CharacterDiffciulty}");
        BattleData.Opponent = opponent;
    }


    public void Battle()
    {

        if (BattleData.Player == null)
            StartNewRun();
  
        Debug.Log($"Start battle!\n{BattleData.Player.CharacterData.Info.CharacterName} VS {BattleData.Opponent.CharacterData.Info.CharacterName}");
        _sceneloaderEvent.LoadScene(SceneHandler.ScenesEnum.GameBattleScene);
    }


}

