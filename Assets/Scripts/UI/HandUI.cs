using UnityEngine;
namespace Battles.UI
{

    public class HandUI
    {
        CardUISO _cardUISO;
        CardUI[] _handCards;
        Vector2 _middlePositionOnScreen;
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
            _middlePositionOnScreen = middlePos;
            _cardUISO = cardUISO;
        }

        private void AlignCards()
        {
            OrderArray();

            if (GetAmountOfCardsInHand != 0)
            {
                // position
                int cardAmountOffset = _cardUISO.CardAmountOffset;
                float spaceBetweenCards = _cardUISO.GetSpaceBetweenCards;
                float width = _cardUISO.CardAlignmentInHandHeight;
                float maxWidth = _cardUISO.MaxWidth;

                if (GetAmountOfCardsInHand > cardAmountOffset)
                {
                    float remain = GetAmountOfCardsInHand - cardAmountOffset;
                    spaceBetweenCards = maxWidth/(GetAmountOfCardsInHand - 1);
                    width += remain * _cardUISO.YFactorPerOffsetCardInHand;
                }

                Vector2 cardLocation;

                float xPos;
                float yPos;
                float moveLeftOffset;
                //rotation
                float degree = _cardUISO.DegreePerCard;
         
                int amountOfIndexInOneSide = (GetAmountOfCardsInHand - 1) / 2;
                bool isEvenNumber = GetAmountOfCardsInHand % 2 == 0;



                for (int i = 0; i < GetAmountOfCardsInHand; i++)
                {

                   
                    // position
                    yPos = (((-(i * i) + (GetAmountOfCardsInHand - 1) * i)) / width);

                    if(GetAmountOfCardsInHand > cardAmountOffset)
                    {
                        moveLeftOffset = -(maxWidth / 2);
                    }
                    else
                    {
                        moveLeftOffset = -((GetAmountOfCardsInHand-1)*_cardUISO.GetSpaceBetweenCards)/2;
                    }
                    xPos = i * spaceBetweenCards;
                    cardLocation = _middlePositionOnScreen + Vector2.right * xPos;

                    Vector2 finalPos = cardLocation + new Vector2( moveLeftOffset,yPos);


                    _handCards[i].CardTranslations?.MoveCard(
                        true,
                         finalPos,
                    _cardUISO.MovementToPositionTimer);


                    SetCardUIRotation(ref i, ref amountOfIndexInOneSide, ref degree, ref isEvenNumber);

                }
                OrderZLayers();
            }
        }
        private void SetCardUIRotation(ref int index,ref int amountOfIndexInOneside,ref float degree,ref bool isEvenNumber)
        {
             //rotation
             float rotation = (amountOfIndexInOneside - index) * degree;

                    if (isEvenNumber)
                        rotation += degree / 2;

              LeanTween.rotateZ(_handCards[index].gameObject ,rotation, _cardUISO.RotationTimer);
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
            AlignCards();
        }
        public void TryRemove( CardUI card)
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                if (ReferenceEquals(_handCards[i], card)) 
                {

                    _handCards[i] = null;
                    _amountOfCardsInHand--;
                    AlignCards();
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