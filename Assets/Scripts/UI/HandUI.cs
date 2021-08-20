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

            if (GetAmountOfCardsInHand != 0)
            {
                // position
                int cardAmountOffset = _cardUISO.CardAmountOffset;
                float spaceBetweenCards = _cardUISO.GetSpaceBetweenCards;
                float width = _cardUISO.CardAlignmentInHandHeight;
                if (GetAmountOfCardsInHand > cardAmountOffset)
                {
                     float remain = GetAmountOfCardsInHand - cardAmountOffset;
                   spaceBetweenCards -=  remain * _cardUISO.ScaleFactorInSpaceInHand ;
                    width += remain * _cardUISO.YFactorPerOffsetCardInHand;

                }

                Vector2 furthestPos = _middlePos + ((GetAmountOfCardsInHand - 1) * -Vector2.left * spaceBetweenCards);
                Vector2 cardLocation;
                float distance = Vector2.Distance(furthestPos, _middlePos);
 
                float yPos = 0;

                //rotation
                float degree = _cardUISO.DegreePerCard;
                float rotation = 0;
                int amountOfIndexInOneSide = (GetAmountOfCardsInHand - 1) / 2;
                bool isEvenNumber = GetAmountOfCardsInHand % 2 == 0;
                int amountOfCards = GetAmountOfCardsInHand;


                for (int i = 0; i < GetAmountOfCardsInHand; i++)
                {

                   
                    // position
                    yPos = (((-(i * i) + (GetAmountOfCardsInHand - 1) * i)) / width);
                
                    cardLocation = _middlePos + (i * -Vector2.left * spaceBetweenCards);


                    _handCards[i].CardTranslations?.MoveCard(
                    true,
                 (cardLocation + Vector2.left * distance / 2) + Vector2.up * yPos,
                    _cardUISO.GetCardFollowDelay);


                    //rotation
                    rotation = (amountOfIndexInOneSide - i) * degree;

                    if (isEvenNumber)
                        rotation += degree / 2;

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