using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUITransitionManager : MonoBehaviour
{
    [Tooltip("Hand Middle Position")]
    [SerializeField] RectTransform _handMiddlePosition;

    [Tooltip("Draw Card position")]
    [SerializeField] RectTransform _drawDeckPosition;

    [Tooltip("Discard Card position")]
    [SerializeField] RectTransform _discardDeckPosition;

    [Tooltip("Exhaust Card position")]
    [SerializeField] RectTransform _exhaustDeckPosition;


    [SerializeField] RectTransform _craftingBtnPosition;
    
    


}
