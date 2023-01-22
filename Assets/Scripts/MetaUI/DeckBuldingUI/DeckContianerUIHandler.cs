using System.Collections.Generic;
using CardMaga.InventorySystem;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using UnityEngine;

public class DeckContianerUIHandler : MonoBehaviour
{
    [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
    [SerializeField] private MetaComboUiContainer _metaComboUiContainer;

    public void Init()
    {
        _metaComboUiContainer.Init();
        _metaCardUIContainer.Init();
    }

    public void Init(List<MetaCardUI> metaCardUis, List<MetaComboUI> metaComboUis)
    {
        _metaCardUIContainer.InitializeSlots(metaCardUis.ToArray());
        _metaComboUiContainer.InitializeSlots(metaComboUis.ToArray());
    }
    
    public void AddCardUI(MetaCardUI cardData)
    {
        _metaCardUIContainer.TryAddObject(cardData);
    }
    
    public void AddComboUI(MetaComboUI metaComboData)
    {
        _metaComboUiContainer.TryAddObject(metaComboData);  
    }
    
    public void RemoveCardUI(MetaCardUI metaCardUI)
    {
        _metaCardUIContainer.RemoveObject(metaCardUI);
    }
    
    public void RemoveComboUI(MetaComboUI metaComboUI)
    {
        _metaComboUiContainer.RemoveObject(metaComboUI);
    }

    public void UnLoadObjects()
    {
        _metaComboUiContainer.Reset();
        _metaCardUIContainer.Reset();
    }
}