using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeTest : MonoBehaviour
{
    private void Start()
    {
        InputReciever.OnSwipeDetected += Test;
    }
    
    private void Test(SwipeData swipeData)
    {
        Debug.Log(swipeData.SwipeDirection);
    }
}
