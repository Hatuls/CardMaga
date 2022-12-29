using System.Linq;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using UnityEngine;

public class DeckContinaerUIHandler : MonoBehaviour
{
    [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
    [SerializeField] private MetaComboUiContainer _metaComboUiContainer;

    public void Init(MetaCardUI[] metaCardUis, MetaComboUI[] metaComboUis)
    {
        _metaCardUIContainer.InitializeSlots(metaCardUis);
        _metaComboUiContainer.InitializeSlots(metaComboUis);
    }
    
    public void AddCardUI(MetaCardData metaCardData)
    {
        if (!_metaCardUIContainer.TryGetEmptySlot(out var cardSlot))
            return;
        
        var cache = FindEmptyCard();
        _metaCardUIContainer.TryAddObject(cache);
        cache.AssignVisual(metaCardData);
        cache.Show();
    }
    
    public void AddComboUI(MetaComboData metaComboData)
    {
       
        var cache = FindEmptyCombo();
        cache.AssignVisual(metaComboData);
        _metaComboUiContainer.TryAddObject(cache);  
        cache.Show();
    }
    
    public void RemoveCardUI(MetaCardData metaCardData)
    {
        _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
    }
    
    public void RemoveComboUI(MetaComboData metaComboData)
    {
        _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
    }

    public void UnLoadObjects()
    {
        _metaComboUiContainer.Reset();
        _metaCardUIContainer.Reset();
    }
    
    private MetaComboUI FindComboUI(MetaComboData metaComboData) => _metaComboUiContainer.AllInventoryObject.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboData.ID == metaComboUI.MetaComboData.ID && !metaComboUI.IsEmpty);
    private MetaCardUI FindCardUI(MetaCardData metaCardData) => _metaCardUIContainer.AllInventoryObject.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardData.CardInstance.CoreID == metaCardUi.CardInstance.CoreID && !metaCardUi.IsEmpty);
    private MetaCardUI FindEmptyCard() => _metaCardUIContainer.AllInventoryObject.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardUi.IsEmpty);
    private MetaComboUI FindEmptyCombo() =>  _metaComboUiContainer.AllInventoryObject.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboUI.IsEmpty);

}