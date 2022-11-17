using System.Collections.Generic;
using CardMaga.InventorySystem;
using CardMaga.MetaData.AccoutData;
using CardMaga.MetaData.Collection;
using UnityEngine;

namespace CardMaga.MetaUI.CollectionUI
{
    public class MetaUICollectionManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _comboHolder;
        [SerializeField] private RectTransform _cardHolder;
        [SerializeField] private MetaCardUI _cardPrefabRef;
        [SerializeField] private MetaComboUI _comboPrefabRef;
        private BasePoolObjectVisualToData<MetaComboUI, MetaComboData> _comboPool;
        private BasePoolObjectVisualToData<MetaCardUI, MetaCardData> _cardPool;

        private List<MetaCardUI> _metaCardUis;
        private List<MetaComboUI> _metaComboUis;

        [SerializeField] private MetaCardUICollectionHandler _metaCardUICollectionHandler;
        [SerializeField] private MetaComboUICollectionHandler _metaComboUICollectionHandler;
        [SerializeField] private MetaCardUIContainer _metaCardUIContainer;
        [SerializeField] private MetaComboUiContainer _metaComboUiContainer;

        public void Init(AccountDataCollectionHelper accountData,MetaAccountData metaAccountData)
        {
            _cardPool = new BasePoolObjectVisualToData<MetaCardUI, MetaCardData>(_cardPrefabRef, _cardHolder);
            _comboPool = new BasePoolObjectVisualToData<MetaComboUI, MetaComboData>(_comboPrefabRef, _comboHolder);

            _metaComboUis = _comboPool.PullObjects(metaAccountData.CharacterDatas.CharacterData.Decks[0].Combos);
            _metaCardUis = _cardPool.PullObjects(metaAccountData.CharacterDatas.CharacterData.Decks[0].Cards);
            
            _metaCardUICollectionHandler.Init();
            _metaComboUICollectionHandler.Init();
            _metaCardUICollectionHandler.AddObjectToPanel(accountData.CollectionCardDatas);
            _metaComboUICollectionHandler.AddObjectToPanel(accountData.MetaComboDatas);
            
            
            _metaComboUiContainer.InitializeSlots(_metaComboUis.ToArray());
            _metaCardUIContainer.InitializeSlots(_metaCardUis.ToArray());
        }

        public void AddCardUI(MetaCardData metaCardData)
        {
            var cache = _cardPool.PullObjects(metaCardData);
            _metaCardUIContainer.TryAddObject(cache);
        }

        public void RemoveCardUI(MetaCardData metaCardData)
        {
            _metaCardUIContainer.RemoveObject(FindCardUI(metaCardData));
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
    }
}

