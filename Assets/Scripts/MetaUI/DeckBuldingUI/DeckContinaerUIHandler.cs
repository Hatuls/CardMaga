using System.Linq;
using Account.GeneralData;
using CardMaga.InventorySystem;
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
    
    public void AddCardUI(CardInstance cardData)
    {
        if (!_metaCardUIContainer.TryGetEmptySlot(out var cardSlot))
            return;
        
        var cache = FindEmptyCard();
        _metaCardUIContainer.TryAddObject(cache);
        cache.AssignVisual(cardData);
        cache.Show();
    }
    
    public void AddComboUI(ComboCore metaComboData)
    {
        var cache = FindEmptyCombo();
        cache.AssignVisual(metaComboData);
        _metaComboUiContainer.TryAddObject(cache);  
        cache.Show();
    }
    
    public void RemoveCardUI(CardInstance metaCardData)
    {
        _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
    }
    
    public void RemoveComboUI(ComboCore metaComboData)
    {
        _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
    }

    public void UnLoadObjects()
    {
        _metaComboUiContainer.Reset();
        _metaCardUIContainer.Reset();
    }
    
    private MetaComboUI FindComboUI(ComboCore comboData) => _metaComboUiContainer.AllInventoryObject.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => comboData.ID == metaComboUI.ComboData.ID && !metaComboUI.IsEmpty);
    private MetaCardUI FindCardUI(CardInstance cardData) => _metaCardUIContainer.AllInventoryObject.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => cardData.CoreID == metaCardUi.CardInstance.CoreID && !metaCardUi.IsEmpty);
    private MetaCardUI FindEmptyCard() => _metaCardUIContainer.AllInventoryObject.TakeWhile(metaCardUi => !ReferenceEquals(metaCardUi, null)).FirstOrDefault(metaCardUi => metaCardUi.IsEmpty);
    private MetaComboUI FindEmptyCombo() =>  _metaComboUiContainer.AllInventoryObject.TakeWhile(metaComboUI => !ReferenceEquals(metaComboUI, null)).FirstOrDefault(metaComboUI => metaComboUI.IsEmpty);

}