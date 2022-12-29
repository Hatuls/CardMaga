using System;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionCardUI : BaseCollectionUIItem, IPoolableMB<MetaCollectionCardUI>,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCollectionCardUI> OnDisposed;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        [SerializeField,ReadOnly] private MetaCollectionCardData _cardData;
        public int CoreId => _cardData.CoreId;

        public int NumberOfInstant => _cardData.NumberOfInstance;
        
        public override void Init()
        {
            base.Init();
            Show();
        }
        
        public void AssignDataAndVisual(MetaCollectionCardData cardData)
        {
            _cardData = cardData;
            _cardNumberText.text = NumberOfInstant.ToString();

            _cardVisuals.Init(Factory.GameFactory.Instance.CardFactoryHandler.CreateCardCore(cardData.CoreId));
            
            _cardData.OnSuccessAddOrRemoveFromCollection += SuccessAddOrRemoveCollection;
            
            UpdateCardVisual();
        }

        public override void TryAddToCollection()
        {
            _cardData.RemoveCardFromCollection();
        }

        public override void TryRemoveFromCollection()
        {
            _cardData.RemoveCardFromCollection();
        }

        public void Dispose()
        {
            _cardData.OnSuccessAddOrRemoveFromCollection -= SuccessAddOrRemoveCollection;
            
            Hide();
            OnDisposed?.Invoke(this);
        }
        
        private void UpdateCardVisual()
        {
            _cardNumberText.text = NumberOfInstant.ToString();
            
            Enable();

            if (_cardData.NotMoreInstants)
            {
                DisablePlus();
                return;
            }

            if (_cardData.MaxInstants)
            {
                DisableMins();
                return;
            }
        }

        protected override void SuccessAddOrRemoveCollection()
        {
            UpdateCardVisual();
        }
    }
}