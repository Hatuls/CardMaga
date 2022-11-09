using System.Collections.Generic;
using CardMaga.Meta.AccountMetaData;
using CardMaga.MetaData.Collection;
using CardMaga.UI.ScrollPanel;
using UnityEngine;

public class MetaCollectionManager : MonoBehaviour
{
    [SerializeField] private MetaComboUIScrollHandler _comboScrollPanelHandler;
    [SerializeField] private MetaCardUIScrollHandler _cardScrollPanelHandler;
    [SerializeField] private AccountDataAccess _accountDataAccess;
    private MetaCardDataHelper _cardDataHelper;
    
    void Start()
    {
        _cardDataHelper = new MetaCardDataHelper(_accountDataAccess);
        _cardScrollPanelHandler.Init();
        _comboScrollPanelHandler.Init();
        LoadObjects();
    }

    public void LoadObjects()
    {
        _cardScrollPanelHandler.AddObjectToPanel(_cardDataHelper.CollectionCardDatas);
        _comboScrollPanelHandler.AddObjectToPanel(_accountDataAccess.AccountData.AccountCombos);//need to move from start
    }
    
}
