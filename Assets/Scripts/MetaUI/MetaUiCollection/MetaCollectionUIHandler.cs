using System.Collections.Generic;
using CardMaga.MetaUI;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.UI;
using UnityEngine;

public class MetaCollectionUIHandler : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool _haveCards;
    [SerializeField] private bool _haveCombo;
    [Header("Scroll Panel Reference")]
    [SerializeField] private MetaCardUICollectionScrollPanel _metaCardUICollectionScrollPanel;
    [SerializeField] private MetaComboUICollectionScrollPanel _metaComboUICollectionScrollPanel;

    public void Init()
    {
        if (_haveCards)
            _metaCardUICollectionScrollPanel.Init();
        if (_haveCombo)
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
        if (_haveCards)
            _metaCardUICollectionScrollPanel.RemoveAllObjectsFromPanel();
        if (_haveCombo)
            _metaComboUICollectionScrollPanel.RemoveAllObjectsFromPanel();
    }

    private void LoadCombos(List<IUIElement> collectionComboUis)
    {
        if (_haveCombo)
            _metaComboUICollectionScrollPanel.AddObjectToPanel(collectionComboUis);
    }

    private void LoadCards(List<IUIElement> collectionCardUis)
    {
        if (_haveCards)
            _metaCardUICollectionScrollPanel.AddObjectToPanel(collectionCardUis);
    }
}
