﻿using Account.GeneralData;
using CardMaga.MetaUI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CardMaga.Meta.Upgrade
{
    public class UpgradeCardsDisplayer : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _middleScreen;
        [SerializeField]
        private RectTransform _cardContainer;
        [SerializeField]
        private UpgradeCardMover _upgradeCardMover;

        [SerializeField]
        private List<ScrollItemData> _itemsDataList = new List<ScrollItemData>();

        [SerializeField]
        private MetaCardUI _prefab;//Temp: Replace with Pool Manager;

        [SerializeField, Range(0, 5f)]
        private float _focusedCardScale = 1f;
        [SerializeField, Range(0, 5f)]
        private float _unfocusedCardsScale = 1f;
        [SerializeField, Range(0, 1f)]
        private float _scaleDuration= 1f;
        private Vector3 _middleScreenPosition;
        private int _currentMiddleObjectIndex = 0;
        private float HalfScreenWidthDistance => Screen.width / 2;
        public ScrollItemData CurrentMiddleObject => _itemsDataList[_currentMiddleObjectIndex];
        private void Awake()
        {
            _middleScreenPosition = _middleScreen.position;
        }
        private void OnEnable()
        {
            _upgradeCardMover.OnSwipeExecuted += SetPositionAndScale;
            _upgradeCardMover.OnSwipingLeft += MoveAllCards;
            _upgradeCardMover.OnSwipingRight += MoveAllCards;
            _upgradeCardMover.OnSwipeLeftExecuted += MoveOneToTheRight;
            _upgradeCardMover.OnSwipeRightExecuted += MoveOneToTheLeft;
        }
        private void OnDisable()
        {
            _upgradeCardMover.OnSwipeExecuted -= SetPositionAndScale;
            _upgradeCardMover.OnSwipingRight -= MoveAllCards;
            _upgradeCardMover.OnSwipingLeft -= MoveAllCards;
            _upgradeCardMover.OnSwipeLeftExecuted -= MoveOneToTheRight;
            _upgradeCardMover.OnSwipeRightExecuted -= MoveOneToTheLeft;
        }
        public void InitCards(CardInstance cardInstance)
        {
            int maxItems = cardInstance.CardSO.CardsMaxLevel;

            int itemCount = _itemsDataList.Count;
            if (maxItems > itemCount)
                PopulateList(maxItems);
            AdjustCardsData(cardInstance, out int amountToActivate);
            ActivateItems(amountToActivate);
            SetView(cardInstance.Level);
        }

        private void SetView(int level)
        {
            _currentMiddleObjectIndex = level;
            SetPositionAndScale();
        }

        private void SetPositionAndScale()
        {
            AdjustPositions(_currentMiddleObjectIndex);
            AdjustScale(_currentMiddleObjectIndex);
        }

        private void MoveAllCards(float amount)
        {
            for (int i = 0; i < _itemsDataList.Count; i++)
                _itemsDataList[i].WorldPosition += (amount) *Vector3.right;
        }
        private void MoveOneToTheRight()
        {
            if (CurrentMiddleObject.CardUI.CardInstance.IsMaxLevel)
                return;

            _currentMiddleObjectIndex++;
            SetView(_currentMiddleObjectIndex);
        }
        private void MoveOneToTheLeft()
        {
            if (CurrentMiddleObject.CardUI.CardInstance.Level == 0)
                return;

            _currentMiddleObjectIndex--;
            SetView(_currentMiddleObjectIndex);
        }

        private void AdjustCardsData(CardInstance cardInstance, out int amountToActivate)
        {
            Card.CardSO cardSO = cardInstance.CardSO;


            int counter = 0;
            foreach (var card in cardSO.CardsCoreInfo)
            {
                var newCardInstance = new CardInstance(card.CardCore);
                var metaCard = new MetaData.AccoutData.MetaCardData(newCardInstance, cardSO, new Card.BattleCardData(newCardInstance));

                _itemsDataList[counter].CardUI.AssignDataAndVisual(metaCard);
                counter++;
            }

            amountToActivate = counter;
        }
        private void ActivateItems(int itemToActivate)
        {
            for (int i = _itemsDataList.Count - 1; i >= 0; i--)
                _itemsDataList[i].CardUI.gameObject.SetActive(itemToActivate > i);
        }
        private void AdjustScale(int level)
        {
            for (int i = 0; i < _itemsDataList.Count; i++)
            {
                float scale = (i != level) ? _unfocusedCardsScale : _focusedCardScale;
                _itemsDataList[i].CardUI.RectTransform.DOScale( scale , _scaleDuration);
            }

        }
        private void AdjustPositions(int level)
        {
            Vector3 middlePosition = _middleScreenPosition;
            _itemsDataList[level].WorldPosition = middlePosition;

            for (int i = 0; i < _itemsDataList.Count; i++)
            {
                ScrollItemData currentItem = _itemsDataList[i];


                if (i != level)
                {
                    Vector3 nextPos = middlePosition;
                    nextPos.x += (HalfScreenWidthDistance * (i - level));
                    currentItem.WorldPosition = nextPos;
                }

            }
        }

        private void PopulateList(int amount)
        {
            Vector3 startPosition = _middleScreenPosition;
            for (int i = 0; i < amount; i++)
            {
                Vector3 nextPos = startPosition + (HalfScreenWidthDistance * i * Vector3.right);
                if (i < _itemsDataList.Count)
                    _itemsDataList[i].WorldPosition = nextPos;
                else
                    _itemsDataList.Add(new ScrollItemData(nextPos, Instantiate(_prefab, _cardContainer)));
            }
        }
        [System.Serializable]
        public class ScrollItemData
        {
            [SerializeField, ReadOnly]
            private Vector3 _worldPosition;
            [SerializeField, ReadOnly]
            private MetaCardUI _metaCardUI;
            public MetaCardUI CardUI => _metaCardUI;
            public Vector3 WorldPosition
            {
                get => _worldPosition;
                set
                {
                    _worldPosition = value;
                    AdjustPosition();
                }
            }
            public ScrollItemData(Vector3 coordinate, MetaCardUI metaCardUI)
            {
                _metaCardUI = metaCardUI;
                WorldPosition = coordinate;
            }

            private void AdjustPosition() => _metaCardUI.RectTransform.DOMove (WorldPosition,.1f);
        }
        #region Editor
#if UNITY_EDITOR
        [Header("Editor:")]
        [SerializeField]
        private CardInstance _cardInstance;

        [SerializeField]
        private float _gizmosScale;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 middlePos = _middleScreenPosition;
            middlePos.x += HalfScreenWidthDistance;
            Gizmos.DrawLine(_middleScreenPosition, middlePos);

            Gizmos.color = Color.blue;
            for (int i = 0; i < _itemsDataList.Count; i++)
            {
                Gizmos.DrawSphere(_itemsDataList[i].WorldPosition, _gizmosScale);
            }
        }
        [ContextMenu("Try System")]
        private void TrySystem()
        {
            InitCards(_cardInstance);
        }

        private void Start()
        {
            TrySystem();
        }
#endif
        #endregion
    }
}