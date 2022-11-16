using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.DeckBuilding;
using CardMaga.MetaUI.CollectionUI;
using UnityEngine;

namespace CardMaga.MetaData.Collection
{
    public class MetaCollectionManager : MonoBehaviour
    {
        [SerializeField] private AccountDataAccess _accountDataAccess;
        [SerializeField] private MetaUICollectionManager _metaUICollectionManager;
        private AccountDataCollectionHelper _accountDataCollectionHelper;
        private DeckBuilder _deckBuilder;

        private void Start()
        {
            
        }
    }
}

