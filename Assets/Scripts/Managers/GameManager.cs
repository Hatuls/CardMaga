using Battles.UI;
using Battles;
using Battles.Turns;
using Managers;
using UnityEngine;
using Art;

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
        _art = new ArtSettings(_panel);
        base.Awake();

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
            Combo.ComboManager.Instance,
            Keywords.KeywordManager.Instance,
            Battles.Deck.DeckManager.Instance,
            CardUIManager.Instance,
            ComboRecipeHandler.Instance,
            BattleManager.Instance
        };
        for (int i = 0; i < _singletons.Length; i++)
            _singletons[i]?.Init();
        
    }
}
[System.Serializable]
public  class ArtSettings
{

    public ArtSO ArtSO;
 
    public static CardTypePalette CardTypePalette;
    public static BarsUIPalette BarsUIPalette;
    public static IconsPalette IconsPalette;
    public static CardUIPalette CardUIPalette;
    public static CraftingUIPalette CraftingUIPalette;
    public static BuffUIPalette BuffUIPalette;
    public static RecipePanelUIPalette RecipePanelUIPalette;

    public static CardIconCollectionSO CardIconCollectionSO;
    public ArtSettings(ArtSO artSO)
    {
        if (ArtSO!= artSO)
        ArtSO = artSO;


        CardTypePalette = ArtSO.GetPallette<CardTypePalette>();
        BarsUIPalette = ArtSO.GetPallette<BarsUIPalette>();
        IconsPalette = ArtSO.GetPallette<IconsPalette>();
        CardUIPalette = ArtSO.GetPallette<CardUIPalette>();
        CraftingUIPalette = ArtSO.GetPallette<CraftingUIPalette>();
        BuffUIPalette = ArtSO.GetPallette<BuffUIPalette>();
        RecipePanelUIPalette = ArtSO.GetPallette<RecipePanelUIPalette>();



        CardIconCollectionSO = artSO.GetSpriteCollections<CardIconCollectionSO>();
    }

}