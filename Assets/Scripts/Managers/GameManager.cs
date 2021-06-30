using Battles.UI;
using Battles;
using Battles.Turns;
using Managers;

public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] _singletons;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        ThreadsHandler.ThreadHandler.TickThread();
    }
    public override void Init()
    {
        _singletons = new ISingleton[15]
        {
            VFXManager.Instance,
            AudioManager.Instance,
            CardExecutionManager.Instance,
           BattleUiManager.Instance,
            CardManager.Instance,
            InputManager.Instance,
            CameraController.Instance,
            PlayerManager.Instance,
            EnemyManager.Instance,
            Relics.RelicManager.Instance,
            Keywords.KeywordManager.Instance,
            Battles.Deck.DeckManager.Instance,
            TurnHandler.Instance,
            CardUIManager.Instance,
            BattleManager.Instance
        };
        for (int i = 0; i < _singletons.Length; i++)
            _singletons[i]?.Init();
        
    }
}
