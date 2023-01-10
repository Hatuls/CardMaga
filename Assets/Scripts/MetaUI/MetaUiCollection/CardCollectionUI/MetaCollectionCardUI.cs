using System;
using CardMaga.MetaData.Collection;
using CardMaga.MetaUI.CollectionUI;
using CardMaga.Tools.Pools;
using CardMaga.UI.Card;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace CardMaga.MetaUI
{
    public class MetaCollectionCardUI : BaseCollectionUIItem, IPoolableMB<MetaCollectionCardUI>,IVisualAssign<MetaCollectionCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCollectionCardUI> OnDisposed;

        [SerializeField] private BattleCardUI _cardUI;
        [SerializeField] private TMP_Text _cardNumberText;
        [SerializeField,ReadOnly] private MetaCollectionCardData _cardData;
        
        public int CoreId => _cardData.CardCoreID;

        public int NumberOfInstant => _cardData.NumberOfInstance;

        public BattleCardUI CardUI => _cardUI;

        public override void Init()
        {
            base.Init();
            Show();
        }
        
        public void AssignVisual(MetaCollectionCardData cardData)
        {
            _cardData = cardData;
            _cardNumberText.text = NumberOfInstant.ToString();

            CardUI.AssignVisualAndData(Factory.GameFactory.Instance.CardFactoryHandler.CreateCard(cardData.CardCoreID));
            
            _cardData.OnSuccessAddOrRemoveFromCollection += SuccessAddOrRemoveCollection;
            
            UpdateCardVisual();
        }
        
        public void PlusPress()
        {
            _cardData.AddCardToCollection();
        }

        public void MinusPress()
        {
            _cardData.RemoveCardFromCollection();
        }

        public void Dispose()
        {
            _cardData.OnSuccessAddOrRemoveFromCollection -= SuccessAddOrRemoveCollection;
            
            Hide();
            OnDisposed?.Invoke(this);
        }

        private void OnDestroy()
        {
            Dispose();
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
                DisableMinus();
                return;
            }
        }

        protected override void SuccessAddOrRemoveCollection()
        {
            UpdateCardVisual();
        }
    }
}