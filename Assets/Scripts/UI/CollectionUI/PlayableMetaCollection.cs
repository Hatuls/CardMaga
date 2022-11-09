using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public class PlayableMetaCollection : MonoBehaviour
{
    [SerializeField] private MetaComboUIScrollHandler _comboScrollPanelHandler;
    [SerializeField] private MetaCardUIScrollHandler _cardScrollPanelHandler;
    [SerializeField] private AccountDataAccess _accountDataAccess;

    private Dictionary<int, int> _sortCardDataIds;

    private List<MetaCardData> _cardDatas;

    void Start()
    {
        _cardScrollPanelHandler.Init();
        _comboScrollPanelHandler.Init();
        _cardScrollPanelHandler.AddObjectToPanel(_cardDatas);
        _comboScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Combos);//need to move from start
    }

    private void SortMetaCardData()
    {
        
        List<MetaCardData> temp;

        
    }

    private void InitDictionary()
    {
        _cardDatas = _accountDataAccess.AccountData.CharacterDatas.CharacterData.Decks[0].Cards;

        _sortCardDataIds = new Dictionary<int, int>();

        for (int i = 0; i < _cardDatas.Count; i++)
        {
            int cache = _cardDatas[i].CardInstance.ID;
            
            if (_sortCardDataIds.TryGetValue(cache, out int value))
            {
                int temp = value++;
                _sortCardDataIds[cache] = temp;
            }
            else
            {
                _sortCardDataIds.Add(cache,1);
            }
        }
    }
}
