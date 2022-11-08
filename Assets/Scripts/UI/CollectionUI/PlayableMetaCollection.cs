using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using UnityEngine;
using System.Linq;

public class PlayableMetaCollection : MonoBehaviour
{
    [SerializeField] private MetaComboUIScrollHandler _comboScrollPanelHandler;
    [SerializeField] private MetaCardUIScrollHandler _cardScrollPanelHandler;
    [SerializeField] private AccountDataAccess _accountDataAccess;

    private List<MetaCardData> _cardDatas;

    void Start()
    {
        _cardScrollPanelHandler.Init();
        _comboScrollPanelHandler.Init();
        _cardScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards);
        _comboScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Combos);
    }

    private void SortMetaCardData()
    {
        _cardDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards;
        
    }
}
