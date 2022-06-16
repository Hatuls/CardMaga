using System;
using System.Collections;
using System.Collections.Generic;
using Battles.UI;
using UnityEngine;

public class SelectCardUI : MonoBehaviour
{
    [SerializeField] private CardUIManager _cardUIManager;
    private CardUI _selectCard;
    
    public void SetSelectCardUI(CardUI cardUI)
    {
        _selectCard.AssignData(cardUI.RecieveCardReference());
        _selectCard.CardTransitionManager.Transition(cardUI.transform.position);
        _selectCard.GFX.SetActive(true);
    }

    public void DisposeCard(Action onDispose = null)
    {
        if (onDispose != null)
        {
            onDispose?.Invoke();
        }
        
        _selectCard.GFX.SetActive(false);
    }
}
