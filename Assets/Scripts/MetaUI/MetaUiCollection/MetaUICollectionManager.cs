using System.Collections.Generic;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using CardMaga.ObjectPool;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaUICollectionManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _comboHolder;
        [SerializeField] private RectTransform _cardHolder;
        [SerializeField] private MetaCardUI _cardPrefabRef;
        [SerializeField] private MetaComboUI _comboPrefabRef;
        private VisualRequester<MetaComboUI, MetaComboData> _comboVisualRequester;
        private VisualRequester<MetaCardUI, MetaCardData> _cardVisualRequester;

        private List<MetaCardUI> _metaCardUis;
        private List<MetaComboUI> _metaComboUis;

        [SerializeField] private MetaCardUICollectionHandler _metaCardUICollectionHandler;
        [SerializeField] private MetaComboUICollectionHandler _metaComboUICollectionHandler;
        [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
        [SerializeField] private MetaComboUiContainer _metaComboUiContainer;

        public void Init(AccountDataCollectionHelper accountData,MetaAccountData metaAccountData)
        {
            _comboVisualRequester = new VisualRequester<MetaComboUI, MetaComboData>(_comboPrefabRef);
            _cardVisualRequester = new VisualRequester<MetaCardUI, MetaCardData>(_cardPrefabRef);

            _metaComboUis =  _comboVisualRequester.GetVisual(metaAccountData.CharacterDatas.CharacterData.Decks[0].Combos);
            _metaCardUis = _cardVisualRequester.GetVisual(metaAccountData.CharacterDatas.CharacterData.Decks[0].Cards);
            
            _metaCardUICollectionHandler.Init();
            _metaComboUICollectionHandler.Init();
            _metaCardUICollectionHandler.AddObjectToPanel(accountData.CollectionCardDatas);
            _metaComboUICollectionHandler.AddObjectToPanel(accountData.CollectionComboDatas);
            
            
            _metaComboUiContainer.InitializeSlots(_metaComboUis.ToArray());
            _metaCardUIContainer.InitializeSlots(_metaCardUis.ToArray());
        }

        public void AddCardUI(MetaCardData metaCardData)
        {
            //var cache = FindEmptyCard();
           // cache.AssignVisual(metaCardData);
           // _metaCardUIContainer.TryAddObject(cache);
           // cache.Show();
        }

        public void RemoveCardUI(MetaCardData metaCardData)
        {
            _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
        }

        public void AddComboUI(MetaComboData metaComboData)
        {
            //var cache = FindEmptyCombo();
          //  cache.AssignVisual(metaComboData);
           // _metaComboUiContainer.TryAddObject(cache);
           // cache.Show();
        }

        public void RemoveComboUI(MetaComboData metaComboData)
        {
            _metaComboUiContainer.RemoveObject(FindComboUI(metaComboData));
        }

        public MetaCardUI FindCardUI(MetaCardData metaCardData)
        {
            foreach (var metaCardUi in _metaCardUis)
            {
                if (metaCardData.CardInstance.ID == metaCardUi.CardInstance.ID)
                {
                    return metaCardUi;
                }
            }

            return null;
        }
        
        public MetaComboUI FindComboUI(MetaComboData metaComboData)
        {
            foreach (var metaComboUI in _metaComboUis)
            {
                if (metaComboData.ID == metaComboUI.MetaComboData.ID)
                {
                    return metaComboUI;
                }
            }

            return null;
        }
    }
}

