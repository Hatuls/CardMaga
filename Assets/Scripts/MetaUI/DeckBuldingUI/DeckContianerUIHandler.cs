using System.Linq;
using Account.GeneralData;
using CardMaga.InventorySystem;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using UnityEngine;

public class DeckContianerUIHandler : MonoBehaviour
{
    [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
    [SerializeField] private MetaComboUiContainer _metaComboUiContainer;

    public void Init(MetaCardUI[] metaCardUis, MetaComboUI[] metaComboUis)
    {
        _metaCardUIContainer.InitializeSlots(metaCardUis);
        _metaComboUiContainer.InitializeSlots(metaComboUis);
    }
    
    public void AddOnsuccessfulCardUI(CardInstance cardData)
    {
        if (!_metaCardUIContainer.TryGetEmptySlot(out var cardSlot))
            return;
        
        var cache = FindEmptyCard();
        _metaCardUIContainer.TryAddObject(cache);
        cache.AssignVisual(cardData);
        cache.Show();
    }
    
    public void AddComboUI(ComboInstance metaComboData)
    {
        var cache = FindEmptyCombo();
        cache.AssignVisual(metaComboData.ComboCore);
        _metaComboUiContainer.TryAddObject(cache);  
        cache.Show();
    }
    
    public void RemoveCardUI(CardInstance metaCardData)
    {
        _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
    }
    
    public void RemoveComboUI(ComboInstance metaComboData)
    {
        _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
    }

    public void UnLoadObjects()
    {
        _metaComboUiContainer.Reset();
        _metaCardUIContainer.Reset();
    }
    
    private MetaComboUI FindComboUI(ComboInstance comboData) => _metaComboUiContainer.AllInventoryObject.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => comboData.CoreID == metaComboUI.ComboData.CoreID && !metaComboUI.IsEmpty);
    private MetaCardUI FindCardUI(CardInstance cardData) => _metaCardUIContainer.AllInventoryObject.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => cardData.CoreID == metaCardUi.CardInstance.CoreID && !metaCardUi.IsEmpty);
    private MetaCardUI FindEmptyCard() => _metaCardUIContainer.AllInventoryObject.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardUi.IsEmpty);
    private MetaComboUI FindEmptyCombo() =>  _metaComboUiContainer.AllInventoryObject.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboUI.IsEmpty);

}