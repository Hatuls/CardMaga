using UnityEngine;
namespace Battles.UI
{

    public class HandUI
    {
        CardUISO _cardUISO;
        CardUI[] _handCards;
        Vector2 _middlePos;
        int _amountOfCardsInHand;




        public int GetAmountOfCardsInHand => _amountOfCardsInHand;
        public  CardUI[] GetHandCards
        {
            get
            {
                if (_handCards == null)
                {
                    Debug.LogError("Hand cards was not initiate correctly!\n Created hand cards in length of 10");
                    _handCards = new CardUI[10];
                }

                return  _handCards;
            }
        }
        public HandUI(ref int maxHand, Vector2 middlePos,  CardUISO cardUISO)
        {
            _handCards = new CardUI[maxHand];
            ResetHand();
            _middlePos = middlePos;
            _cardUISO = cardUISO;
        }

        private void SetCardsPosition()
        {
            OrderArray();
            Debug.Log("************************* " + GetAmountOfCardsInHand);
            if (GetAmountOfCardsInHand != 0)
            {
                Vector2 furthestPos = _middlePos + ((GetAmountOfCardsInHand - 1) * -Vector2.left * _cardUISO.GetSpaceBetweenCards);
                Vector2 cardLocation;
                float distance = Vector2.Distance(furthestPos, _middlePos);


                float degree = 20;


                float degreePerCard = degree / GetAmountOfCardsInHand;

                for (int i = 0; i < GetAmountOfCardsInHand; i++)
                {
                    cardLocation = _middlePos + (i * -Vector2.left * _cardUISO.GetSpaceBetweenCards);

                    _handCards[i].CardTranslations?.MoveCard(
                        true,
                        cardLocation + Vector2.left * distance / 2,
                        _cardUISO.GetCardFollowDelay);

                    float rotation = degree/2 - (i * degreePerCard);
                    if (GetAmountOfCardsInHand %2 !=0)
                    {
                        int middleCard = (GetAmountOfCardsInHand + 1) / 2;
                        if (i == middleCard-1)
                            rotation = 0;
                    }
                    else
                    {

                    }
                    _handCards[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));


                }
                OrderZLayers();
            }
        }

        public void Add( CardUI card)
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                if (_handCards[i] == card)
                    return;
            }
            _handCards[GetEmptyIndex()] = card;
            _amountOfCardsInHand++;
            SetCardsPosition();
        }
        public void TryRemove( CardUI card)
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                if (ReferenceEquals(_handCards[i], card)) 
                {

                    _handCards[i] = null;
                    _amountOfCardsInHand--;
                    SetCardsPosition();
                    return;
                }
            }

        }
        private void OrderArray()
        {
            for (int i = 0; i < _handCards.Length - 1; i++)
            {
                if (_handCards[i] == null)
                {
                    for (int j = i + 1; j < _handCards.Length; j++)
                    {
                        if (_handCards[j] != null)
                        {
                            _handCards[i] = _handCards[j];
                            _handCards[j] = null;
                            break;
                        }

                        if (j == _handCards.Length - 1)
                            return;
                    }
                }
            }
        }
        private int GetEmptyIndex()
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                if (_handCards[i] == null)
                    return  i;
            }

            Debug.LogError("ERROR : Didnt find empty Spots returning FIRST PLACE !");
            return  0;
        }

        public void ResetHand()
        {
            _amountOfCardsInHand = 0;

            for (int i = 0; i < _handCards.Length; i++)
                _handCards[i] = null;

        }

        void OrderZLayers()
        {

            for (int i = 0; i < _handCards.Length; i++)
            {
                if (_handCards[i] != null)
                    _handCards[i].transform.SetSiblingIndex(i);
            }

        }

    }



}