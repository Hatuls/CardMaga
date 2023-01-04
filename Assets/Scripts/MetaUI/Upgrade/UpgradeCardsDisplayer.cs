using Account.GeneralData;
using CardMaga.MetaUI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CardMaga.Meta.Upgrade
{
    public class UpgradeCardsDisplayer : MonoBehaviour
    {
      
        public event Action<int> OnItemCountChanged;
        public event Action<int> OnItemsIndexChanged;
        [SerializeField]
        private RectTransform _middleScreen;
        [SerializeField]
        private RectTransform _cardContainer;
        [SerializeField]
        private UpgradeCardMover _upgradeCardMover;
        [SerializeField]
        private DotUIScroller _dotsHandler;
        [SerializeField]
        private List<ScrollItemData> _itemsDataList = new List<ScrollItemData>();

        [SerializeField]
        private MetaCardUI _prefab;//Temp: Replace with Pool Manager;

        [SerializeField, Range(0, 5f), Tooltip("The Time it takes to reset the cards position to the current position")]
        private float _adjustCardsPositionDuration = .1f;


        [SerializeField, Range(0, 5f)]
        private float _focusedCardScale = 1f;
        [SerializeField, Range(0, 5f)]
        private float _unfocusedCardsScale = 1f;
        [SerializeField, Range(0, 1f)]
        private float _scaleDuration= 1f;
        private Vector3 _middleScreenPosition;
        private int _currentMiddleObjectIndex = 0;
        private IDisposable _zoomToken;
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
            _upgradeCardMover.OnSwipeLeftExecuted += MoveOneToTheLeft;
            _upgradeCardMover.OnSwipeRightIsAtMaxValue += MoveOneToTheRight ;
        }
        private void OnDisable()
        {
            _upgradeCardMover.OnSwipeExecuted -= SetPositionAndScale;
            _upgradeCardMover.OnSwipeRightIsAtMaxValue -= MoveOneToTheRight;
            _upgradeCardMover.OnSwipingRight -= MoveAllCards;
            _upgradeCardMover.OnSwipingLeft -= MoveAllCards;
            _upgradeCardMover.OnSwipeLeftExecuted -= MoveOneToTheLeft;
            _upgradeCardMover.OnSwipeRightIsAtMaxValue -= MoveOneToTheRight ;
        }
        public void InitCards(CardInstance cardInstance)
        {
            int maxItems = cardInstance.CardSO.CardsMaxLevel;
            _dotsHandler.Init(maxItems, cardInstance.Level);
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
            OnItemsIndexChanged?.Invoke(level);
            SetPositionAndScale();
            ZoomIn();
        }

        private void ZoomIn()
        {
            if(_zoomToken!=null)
                _zoomToken.Dispose();
            _zoomToken =  _itemsDataList[_currentMiddleObjectIndex].CardUI.CardUI.CardVisuals.CardZoomHandler.ZoomTokenMachine?.GetToken();
        }

        private void SetPositionAndScale()
        {
            AdjustPositions(_currentMiddleObjectIndex);
            AdjustScale(_currentMiddleObjectIndex);
            
        }

        private void MoveAllCards(float amount)
        {
            for (int i = 0; i < _itemsDataList.Count; i++)
                _itemsDataList[i].AdjustPosition(_itemsDataList[i].WorldPosition + (amount) *Vector3.right, Time.deltaTime);
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

                _itemsDataList[counter].CardUI.AssignVisual(metaCard.CardInstance);
                counter++;
            }

            amountToActivate = counter;
        }
        private void ActivateItems(int itemToActivate)
        {
            int activeCount = 0;
            for (int i = _itemsDataList.Count - 1; i >= 0; i--)
            {
                bool toActivate = itemToActivate > i;
                _itemsDataList[i].CardUI.gameObject.SetActive(toActivate);
                if (toActivate)
                    activeCount++;
            }
            OnItemCountChanged?.Invoke(activeCount);
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
                DOTween.Kill(currentItem.CardUI);
                currentItem.AdjustPosition(currentItem.WorldPosition, _adjustCardsPositionDuration);
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

                _itemsDataList[i].AdjustPosition(nextPos, _adjustCardsPositionDuration);
            }
        }
        [Serializable]
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
                set=>     _worldPosition = value;
                
            }
            public ScrollItemData(Vector3 coordinate, MetaCardUI metaCardUI)
            {
                _metaCardUI = metaCardUI;
                WorldPosition = coordinate;
          
            }

            public void AdjustPosition(Vector3 pos,float duration) => _metaCardUI.RectTransform.DOMove (pos, duration);
        }
        #region Editor
#if UNITY_EDITOR


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

        [ContextMenu("Set Positions")]
        private void SetPositionInEditor()
        {
            Vector3 middlePosition  = _middleScreen.position; 
            _itemsDataList[_currentMiddleObjectIndex].WorldPosition = middlePosition;

            for (int i = 0; i < _itemsDataList.Count; i++)
            {
                ScrollItemData currentItem = _itemsDataList[i];


                if (i != _currentMiddleObjectIndex)
                {
                    Vector3 nextPos = middlePosition;
                    nextPos.x += (HalfScreenWidthDistance * (i - _currentMiddleObjectIndex));
                    currentItem.WorldPosition = nextPos;
                }
                currentItem.CardUI.RectTransform.position = currentItem.WorldPosition;
            }
        }
#endif
        //[Header("Editor:")]
        //[SerializeField]
        //private CardInstance _cardInstance;
        //[ContextMenu("Try System")]
        //private void TrySystem()
        //{
        //    InitCards(_cardInstance);
        //}

        //private void Start()
        //{
        //    TrySystem();
        //}
        #endregion
    }
}