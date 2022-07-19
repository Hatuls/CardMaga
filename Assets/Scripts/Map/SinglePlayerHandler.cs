
using Battle;
using Characters;
using CardMaga.UI;
using UnityEngine;
using Account.GeneralData;

public class SinglePlayerHandler : MonoBehaviour
{
    private static SinglePlayerHandler _instance;
    public static SinglePlayerHandler Instance => _instance;
    [SerializeField]
    private SceneIdentificationSO _battleScene;

    private ISceneHandler _sceneHandler;


    #region MonoBehaviour Callbacks
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            SceneHandler.OnSceneHandlerActivated += this.RecieveSceneHandler;
        }
        else if (_instance != this)
            Destroy(this.gameObject);


    }

    private void OnDestroy()
    {
            SceneHandler.OnSceneHandlerActivated -= this.RecieveSceneHandler;
        
    }
    // Need To be Re-Done
    private void Start()
    {
        //if (Account.AccountManager.Instance.BattleData.Player == null)
        //    StartNewRun(); 
    }

    #endregion
    private void RecieveSceneHandler(ISceneHandler sh) => _sceneHandler = sh;
    // Need To be Re-Done
    //  public static void RegisterPlayerCharacterDataForNewRun(Character playerCharacter) => Account.AccountManager.Instance.BattleData.Player = playerCharacter;
    // Need To be Re-Done
    public void StartNewRun(Character playerLoadOut = null)
    {

        if (playerLoadOut== null)
        {
            Debug.LogWarning("SingePlayerHandler: Character playerLoadOut class is null.\nCreateing Character from SO");
            playerLoadOut = Factory.GameFactory.Instance.CharacterFactoryHandler.CreateCharacter(CharacterTypeEnum.Player);
        }
        //var data = Account.AccountManager.Instance.BattleData;
        //data.PlayerWon = false;
       //Debug.Log($"Registering Player: {playerLoadOut.CharacterData.CharacterSO.CharacterName}");
        //data.Player = playerLoadOut;
        //UpperInfoUIHandler.Instance.UpdateUpperInfoHandler(ref data.Player.CharacterData.CharacterStats);
    }
    // Need To be Re-Done
    public void RegisterOpponent(Character opponent)
    {
//#if UNITY_EDITOR
//        Debug.Log($"Registering Opponent: {opponent.CharacterData.CharacterSO.CharacterName}\nDifficulty: {opponent.CharacterData.CharacterSO.CharacterDiffciulty}");
//#endif
//        Account.AccountManager.Instance.BattleData.Opponent = opponent;
    }

    // Need To be Re-Done
    public void Battle()
    {

//        if (Account.AccountManager.Instance.BattleData.Player == null)
//            StartNewRun();
//#if UNITY_EDITOR
//        Debug.Log($"Start battle!\n{Account.AccountManager.Instance.BattleData.Player.CharacterData.CharacterSO.CharacterName} VS {Account.AccountManager.Instance.BattleData.Opponent.CharacterData.CharacterSO.CharacterName}");
//#endif
//        _sceneHandler.MoveToScene(_battleScene);
    }


}

