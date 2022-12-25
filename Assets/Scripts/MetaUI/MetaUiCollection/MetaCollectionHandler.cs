using System;
using System.Collections.Generic;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.UI;
using UnityEngine;

public class MetaCollectionHandler : MonoBehaviour
{
    [SerializeField] private MetaCardUICollectionScrollPanel _metaCardUICollectionScrollPanel;
    [SerializeField] private MetaComboUICollectionScrollPanel _metaComboUICollectionScrollPanel;

    public void Init()
    {
        _metaCardUICollectionScrollPanel.Init();
        _metaComboUICollectionScrollPanel.Init();
    }

    public void LoadObjects(List<MetaCollectionCardUI> cardVisuals,
        List<MetaCollectionComboUI> comboVisuals)
    {
        if (cardVisuals != null)
        {
            var cardUIElement = cardVisuals.ConvertAll(x => (IUIElement) x);
            LoadCards(cardUIElement);
        }

        if (comboVisuals != null)
        {
            var comboUIElement = comboVisuals.ConvertAll(x => (IUIElement) x);
            LoadCombos(comboUIElement);
        }
        
    }

    public void UnLoadObjects()
    {
        _metaCardUICollectionScrollPanel.RemoveAllObjectsFromPanel();
        _metaComboUICollectionScrollPanel.RemoveAllObjectsFromPanel();
    }

    private void LoadCombos(List<IUIElement> collectionComboUis)
    {
        _metaComboUICollectionScrollPanel.AddObjectToPanel(collectionComboUis);
    }

    private void LoadCards(List<IUIElement> collectionCardUis)
    {
        _metaCardUICollectionScrollPanel.AddObjectToPanel(collectionCardUis);
    }
}
