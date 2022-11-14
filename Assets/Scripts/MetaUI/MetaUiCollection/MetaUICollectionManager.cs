using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaUICollectionManager : MonoBehaviour
    {
        [SerializeField] private MetaComboUICollectionHandler _comboCollectionPanelHandler;
        [SerializeField] private MetaCardUICollectionHandler _cardCollectionPanelHandler;
        
        private MetaCollectionDeckUIHandler _metaCollectionDeck;
    }
}

