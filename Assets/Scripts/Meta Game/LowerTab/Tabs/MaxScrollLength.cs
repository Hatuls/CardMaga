using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MaxScrollLength : MonoBehaviour
    {
        [SerializeField]
        RectTransform _transform;
        [SerializeField]
        float _extraRowAmountOfDistance;

        public void InitTabScroll(int cardAmount)
        {
            int amountofRows;
            amountofRows = cardAmount / 4;
            //lets say we get a 25 card amount
            //we divide it by 4 and see that we have a module of 1
            //we want to get a 7

            if (cardAmount % 4 == 0)
            {
                amountofRows++;
                //every 4 cards add  _difference size to the
            }
            float distanceToAdd; ;
            distanceToAdd = _extraRowAmountOfDistance * amountofRows;


            _transform.sizeDelta = new Vector2(_transform.rect.width, distanceToAdd);
        }
    }
}
