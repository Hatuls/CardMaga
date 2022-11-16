using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaUICollectionManager : MonoBehaviour
    {
        
        [SerializeField] private MetaCardUICollectionHandler _metaCardUICollectionHandler;
        [SerializeField] private MetaComboUICollectionHandler _metaComboUICollectionHandler;

        public void Init(AccountDataCollectionHelper accountData)
        {
            _metaCardUICollectionHandler.AddObjectToPanel(accountData.CollectionCardDatas);
            _metaComboUICollectionHandler.AddObjectToPanel(accountData.MetaComboDatas);
        }
    }
}

