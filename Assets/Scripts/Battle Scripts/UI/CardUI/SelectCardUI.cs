using System;
using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class SelectCardUI
{
    private CardUI _selectCard;

    public CardUI Card
    {
        get { return _selectCard; }
    }
    
    public void SetSelectCardUI(CardUI cardUI)
    {
        _selectCard = cardUI;
        Debug.Log("Card Select is " + _selectCard);
    }
    
    public void DisposeCard(Action onDispose = null)
    {
        if (onDispose != null)
        {
            onDispose?.Invoke();
        }
    }
}
