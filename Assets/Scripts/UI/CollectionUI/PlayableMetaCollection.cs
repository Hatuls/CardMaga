using CardMaga.Meta.AccountMetaData;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public class PlayableMetaCollection : MonoBehaviour
{
    [SerializeField] private MetaComboUIScrollHandler _comboScrollPanelHandler;
    [SerializeField] private MetaCardUIScrollHandler _cardScrollPanelHandler;
    [SerializeField] private AccountDataAccess _accountDataAccess;
    
    void Start()
    {
        _cardScrollPanelHandler.Init();
        _comboScrollPanelHandler.Init();
        _cardScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards);
        _comboScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Combos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
