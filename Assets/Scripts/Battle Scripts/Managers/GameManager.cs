using Battles.UI;
using Battles;
using Battles.Turns;
using Managers;
using UnityEngine;
using Art;

public class GameManager : MonoSingleton<GameManager>
{
    ISingleton[] _singletons;
    [SerializeField]
    int _maxFPS =30;
    [SerializeField]
    Art.ArtSO _panel;
    [SerializeField]
    ArtSettings _art;

    private void Start()
    {
        Init();
    }
  
    private void Update()
    {
     //  Application.targetFrameRate = _maxFPS;
        ThreadsHandler.ThreadHandler.TickThread();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Init();
        }
    }
    public override void Init()
    {
        _art = new ArtSettings(_panel);
        const byte amount = 14;
        _singletons = new ISingleton[amount]
        {
            VFXManager.Instance,
            CardExecutionManager.Instance,
            BattleUiManager.Instance,
            CardManager.Instance,
            CameraController.Instance,
            PlayerManager.Instance,
            EnemyManager.Instance,
            Combo.ComboManager.Instance,
            Keywords.KeywordManager.Instance,
            Battles.Deck.DeckManager.Instance,
            CardUIManager.Instance,
            BattleManager.Instance,
            Rewards.Battles.BattleUIRewardHandler.Instance ,
            Rewards.Battles.BattleRewardHandler.Instance
        };

        StartCoroutine(InitScripts());
    }
    System.Collections.IEnumerator InitScripts()
    {
        int frameSeperator = 3;
        for (int i = 0; i < _singletons.Length; i++)
        {
            if (i% frameSeperator == 0)
                 yield return null;
            _singletons[i]?.Init();
        }     
       
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
