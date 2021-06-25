
using UnityEngine;

using System.Collections;
using Battles.Deck;
namespace Battles.UI
{
    public class CardUIManager : MonoSingleton<CardUIManager>, ITouchable
    {
        #region Field
        [Tooltip("List of cards that UI Know About")]
        [SerializeField] CardUI[] _cardUIArr;

        [Tooltip("Current Clicked Card UI")]
        [SerializeField] CardUI _clickedCardUI;

        [Tooltip("Hand Middle Position")]
        [SerializeField] RectTransform _handMiddlePosition;

        [Tooltip("Draw Card position")]
        [SerializeField] RectTransform _drawDeckPosition;

        [Tooltip("Discard Card position")]
        [SerializeField] RectTransform _discardDeckPosition;

        [Tooltip("Exhaust Card position")]
        [SerializeField] RectTransform _exhaustDeckPosition;

        [Tooltip("Card UI Settings")]
        [SerializeField] CardUISO _cardUISettings;

        private HandUI _handUI;

        bool _isTryingToPlace;

        [SerializeField]
        AudioConfigurationSO _drawCardSound;


        [SerializeField]
        ArtSO _artSO;
        #endregion

        #region Events
        [SerializeField]
        Unity.Events.AudioConfigurationSOEvent _onDrawCard;
        #endregion

        #region Properties
        public CardUI[] CardUIArr => _cardUIArr;

        public RectTransform GetDrawDeckPosition => _drawDeckPosition;
        public RectTransform GetDiscardDeckPosition => _discardDeckPosition;
        public RectTransform GetExhaustDeckPosition => _exhaustDeckPosition;
        public RectTransform GetHandMiddlePosition => _handMiddlePosition;
        public ref CardUI GetClickedCardUI { get => ref _clickedCardUI; }
        public CardUI SetClickedCardUI { set => _clickedCardUI = value; }
        public bool IsTryingToPlace { get => _isTryingToPlace; set { _isTryingToPlace = value; } }

        #endregion

        #region MonoBehaiviour callbacks
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DrawCards(null, DeckEnum.PlayerDeck);
            }
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                RemoveHands();
            }

        }

        #endregion

        #region Private Methods
        private IEnumerator RemoveCards()
        {

            if (_handUI.GetAmountOfCardsInHand == 0)
                yield break;

            CardUI[] cards = _handUI.GetHandCards;

            if (_handUI.GetAmountOfCardsInHand > 0)
            {
                for (int i = _handUI.GetAmountOfCardsInHand - 1; i >= 0; i--)
                {

                    if (cards[i] != null)
                        cards[i].MoveCard(true, GetDeckPosition(DeckEnum.Disposal), _cardUISettings.GetTimerForCardGoingToDiscardPile, false);


                    _handUI.TryRemove(ref cards[i]);

                    yield return new WaitForSeconds(_cardUISettings.GetDelayBetweenRemovalOfEachCard);
                }
            }

            yield return null;
     
        }
        internal CardUI ActivateCard(Cards.Card cardData, Vector2 pos)
        {
            //get one cardUI
            for (int i = 0; i < CardUIArr.Length; i++)
            {
                if (CardUIArr[i] == null)
                    break;
                if (CardUIArr[i].gameObject.activeInHierarchy == false || CardUIArr[i].gameObject.activeSelf == false)
                {
                    CardUIArr[i].gameObject.SetActive(true);
                    CardUIArr[i].SetPosition(pos);
                    CardUIArr[i].SetScale(_cardUISettings.GetCardDefaultScale, Time.deltaTime);
                    AssignDataToCardUI(ref CardUIArr[i], ref cardData);
                    CardUIArr[i].GetCanvasGroup.blocksRaycasts = true;

                    return CardUIArr[i];

                }
            }

            Debug.LogError("CardUI is NUll");
            return null;

            //turn it on
            //position of turning on
            //add data
        }
        public void AssignDataToCardUI(ref CardUI card, ref Cards.Card cardData)
        {
            card.SetCardReference(ref cardData, _artSO) ;

        }
        private Vector3 GetDeckPosition(DeckEnum fromDeck)
        {
            switch (fromDeck)
            {
                case DeckEnum.PlayerDeck:
                    return GetDrawDeckPosition.anchoredPosition3D;
                case DeckEnum.Hand:
                    return GetHandMiddlePosition.anchoredPosition3D;
                case DeckEnum.Disposal:
                    return GetDiscardDeckPosition.anchoredPosition3D;
                case DeckEnum.Exhaust:
                    return GetExhaustDeckPosition.anchoredPosition3D;
                default:
                    return Vector3.zero;
            }
        }
        #endregion


        #region Public Methods
        public void InitCardUI()
        {

            if (CardUIArr == null && CardUIArr.Length == 0)
            {
                Debug.LogError("Error in CardUIArr");
                return;
            }
            else
            {
                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                    {
                        Debug.LogError("CardUI is NUll");
                        return;
                    }
                    CardUIArr[i].gameObject.SetActive(false);
                }
            }
        }
        public void DrawCards(Cards.Card cardData, Battles.Deck.DeckEnum fromDeck, int amount = 1)
        {
            for (int j = 0; j < amount; j++)
            {
                if (_handUI.GetAmountOfCardsInHand >= _cardUISettings.GetAmountOfCardsUIInHand)
                {
                    Debug.Log("Tried to draw card but couldnt because hand is full");
                    break;
                }

                for (int i = 0; i < CardUIArr.Length; i++)
                {
                    if (CardUIArr[i] == null)
                    {
                        Debug.Log("CardUI is NULL");
                        return;
                    }
                    if (CardUIArr[i].gameObject.activeInHierarchy == false || !CardUIArr[i].gameObject.activeSelf)
                    {
                        ActivateCard(cardData, GetDeckPosition(fromDeck));
                        _onDrawCard?.Raise(_drawCardSound);
                        _handUI.Add(ref CardUIArr[i]);
                        break;
                    }
                }
            }

        }
        public void RemoveHands()
        {
            StopCoroutine(RemoveCards());
            StartCoroutine(RemoveCards());
        }
        public override void Init()
        {
            InitCardUI();
            _handUI = new HandUI(ref _cardUISettings.GetAmountOfCardsUIInHand, GetHandMiddlePosition.anchoredPosition, ref _cardUISettings);
          
            //     DrawCards(null, Battles.Deck.DeckEnum.PlayerDeck);
        }
        public void SetCardUI(CardUI cardUI)
        {
            if (cardUI == null)
            {
                Debug.LogError("Error in Set Card UI");
                return;
            }
            //  Debug.Log("Card Was Set");
            InputManager.Instance.AssignObjectFromTouch(this);
            SetClickedCardUI = cardUI;

        }
       
        public void TryRemoveFromHandUI(CardUI cache)
       => _handUI.TryRemove(ref cache);
        public void OnClickedCardUI(CardUI card)
        {
            if (card.GetRectTransform.localScale == _cardUISettings.GetCardUIZoomedScale)
            {
                _handUI.Add(ref card);
                card.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
            }
            else
            {
                _handUI.TryRemove(ref card);
                card.MoveCard(true, Vector2.zero, _cardUISettings.GetCardMoveToDeckDelay);
                card.SetScale(_cardUISettings.GetCardUIZoomedScale, _cardUISettings.GetCardScaleDelay);

            }
        }


        #endregion

        #region Touching Cards
        public void OnFirstTouch(in Vector2 touchPos)
        {
            if (GetClickedCardUI == null)
                return;
            Debug.Log("Card UI: OnFirstTouch, scaling card");

            TryRemoveFromHandUI(GetClickedCardUI);
        }

        public void OnReleaseTouch(in Vector2 touchPos)
        {
            if (GetClickedCardUI == null)
                return;

            if (_isTryingToPlace) // placed on placement Destination
            {
                Debug.Log("Placed On");
                /*
                _handUI.Remove(ref GetClickedCardUI);
                _placementUI.GetCurrentSlot.SetImage(
                    Battles.Deck.DeckManager.Instance.GetCardFromDeck(Battles.Deck.DeckEnum.Placement)[_placementUI.GetCurrentSlot.GetSlotID].GetSetCard.GetCardImage);
            */
                _isTryingToPlace = false;
            }
            else
            {
                _handUI.Add(ref GetClickedCardUI);
                GetClickedCardUI.GetCanvasGroup.blocksRaycasts = true;
                GetClickedCardUI.SetScale(_cardUISettings.GetCardDefaultScale, Time.deltaTime);
            }


            SetClickedCardUI = null;
        }

        public void OnHoldTouch(in Vector2 touchPos)
        {
            if (GetClickedCardUI != null)
            {
                GetClickedCardUI.SetScale(_cardUISettings.GetCardDefaultScale, _cardUISettings.GetCardScaleDelay);
                GetClickedCardUI.MoveCard(false, touchPos, _cardUISettings.GetCardFollowDelay);
            }
        }

        #endregion
    }

}
