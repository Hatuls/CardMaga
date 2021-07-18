using Battles.UI;
using Battles;
using Battles.Turns;
using Managers;
using UnityEngine;
public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] _singletons;
    [SerializeField] int _maxFPS =30;
    [SerializeField]
    Art.ArtSO _panel;
    [SerializeField]
    ArtSettings _art;

    public override void Awake()
    {
        base.Awake();
        _art = new ArtSettings(_panel);
    }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
       Application.targetFrameRate = _maxFPS;
        ThreadsHandler.ThreadHandler.TickThread();
    }
    public override void Init()
    {
   

        _singletons = new ISingleton[16]
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
            BattleManager.Instance,
            ComboRecipeHandler.Instance
        };
        for (int i = 0; i < _singletons.Length; i++)
            _singletons[i]?.Init();
        
    }
}
[System.Serializable]
public  class ArtSettings
{
    [Sirenix.OdinInspector.ShowInInspector]
    public static Art.ArtSO ArtSO;
    public ArtSettings(Art.ArtSO artSO)
    {
        ArtSO = artSO;
    }
}