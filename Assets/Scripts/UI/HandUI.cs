﻿using UnityEngine;
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

        public void AlignCards()
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

                    Vector3 finalPos = new Vector3(cardLocation.x +moveLeftOffset, cardLocation.y + yPos, GetAmountOfCardsInHand- i);


                    _handCards[i].CardTranslations?.MoveCard(
                        true,
                         finalPos,
                    _cardUISO.MovementToPositionTimer);


                    SetCardUIRotation(ref i, ref amountOfIndexInOneSide, ref degree, ref isEvenNumber);


                }
                OrderZLayers();
            }
        }

        /// <summary>
        /// Run over the cards in hand and lock or unlock the cards that are not in inputstate of zoomed or hold.
        /// </summary>
        /// <param name="toLock"></param>
        public void LockCardsInput(bool toLock)
        {
            for (int i = 0; i < _amountOfCardsInHand; i++)
            {
                if (_handCards[i]?.Inputs?.CurrentState != CardUIAttributes.CardInputs.CardUIInput.Zoomed 
                    &&_handCards[i]?.Inputs?.CurrentState != CardUIAttributes.CardInputs.CardUIInput.Hold)
                {
                _handCards[i].Inputs.CurrentState = toLock ? CardUIAttributes.CardInputs.CardUIInput.Locked : CardUIAttributes.CardInputs.CardUIInput.Hand;
                _handCards[i].Inputs.GetCanvasGroup.blocksRaycasts = !toLock;
                }
            }
        }


        private void SetCardUIRotation(ref int index,ref int amountOfIndexInOneside,ref float degree,ref bool isEvenNumber)
        {
             //rotation
             float rotation = (amountOfIndexInOneside - index) * degree;

                    if (isEvenNumber)
                        rotation += degree / 2;

            _handCards[index].CardTranslations.SetRotation(rotation, _cardUISO.RotationTimer);
          //    LeanTween.rotateZ(_handCards[index].gameObject ,rotation, _cardUISO.RotationTimer);
        }




        public void Add( CardUI card)
        {
            for (int i = 0; i < _handCards.Length; i++)
            {
                if (_handCards[i] == card)
                    return;
            }
            card.Inputs.CurrentState = CardUIAttributes.CardInputs.CardUIInput.Hand;
            _handCards[GetEmptyIndex()] = card;
            _amountOfCardsInHand++;
            AlignCards();
        }
        public bool TryRemove( CardUI card)
        {
            if (card != null)
            {
                for (int i = 0; i < _handCards.Length; i++)
                {
                    if (ReferenceEquals(_handCards[i], card))
                    {

                        _handCards[i] = null;
                        _amountOfCardsInHand--;
                        AlignCards();
                        return true;
                    }
                }
            }
            return false;
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