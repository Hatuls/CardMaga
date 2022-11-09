using CardMaga.Meta.AccountMetaData;
using CardMaga.Tools.Pools;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CardMaga.UI.MetaUI
{

    public class MetaCardUI : MonoBehaviour, IPoolableMB<MetaCardUI>, IUIElement, IVisualAssign<MetaCardData>//need to change to MetaCardData 
    {
        public event Action<MetaCardUI> OnDisposed;
        public event Action<int> OnAddCard;
        public event Action<int> OnRemoveCard;
        public event Action OnShow;
        public event Action OnHide;
        public event Action OnInitializable;

        [SerializeField] private BaseCardVisualHandler _cardVisuals;
        [SerializeField] private TMP_Text _cardNumberText;
        private List<MetaCardData> _cardDatas;
        private int _cardId;
        private int _cardLevel;

        public int CardID => _cardId;
        public int CardLevel => _cardLevel;

        public void Init()
        {
            OnInitializable?.Invoke();
            Show();
        }

        public void Dispose()
        {
            Hide();
            OnDisposed?.Invoke(this);
        }


        public void AssingVisual(MetaCardData data)
        {
            _cardLevel = data.BattleCardData.CardInstance.Level;
            _cardId = data.BattleCardData.CardInstance.ID;
            _cardVisuals.Init(data.BattleCardData);
        }

        public void AddToDeck()
        {
            UpdateCardNumber();
            Debug.Log("AddCard" + this.name);
            OnAddCard?.Invoke(_cardId);
        }

        public void RemoveFromDeck()
        {
            UpdateCardNumber();
            OnRemoveCard?.Invoke(_cardId);
        }

        private void UpdateCardNumber()
        {
            _cardNumberText.text = _cardDatas.Count.ToString();
        }


        public void Show()
        {
            OnShow?.Invoke();

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            OnHide?.Invoke();
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

        }
    }
}