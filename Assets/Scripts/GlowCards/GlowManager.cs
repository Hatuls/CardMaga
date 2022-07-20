using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using CardMaga.UI;
using CardMaga.Card;
using Characters.Stats;
using CardMaga.UI.Card;

public class GlowManager : MonoBehaviour
{
    IGetCardsUI cardUI;
    private void Awake()
    {
        cardUI = GetComponent<IGetCardsUI>();
        HandUI.OnCardsAddToHand += CheckCardToGlow;
        HandUI.OnCardsExecuteGetCards += CheckCardGlowAfterExecute;
        HandUI.OnCardSelect += ActiveDeckCardsGlow;
        HandUI.OnCardReturnToHand += DeactiveDeckCards;
    }

    public void CheckCardToGlow(IReadOnlyList<CardUI> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (StaminaHandler.Instance.IsEnoughStamina(true, cards[i].CardData))
            {
                cards[i].CardVisuals.ActivateGlow();
            }

            else
                cards[i].CardVisuals.DeactivateGlow();
        }
    }

    public void CheckCardGlowAfterExecute(IReadOnlyList<CardUI> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (StaminaHandler.Instance.IsEnoughStamina(true, cards[i].CardData))
                cards[i].CardVisuals.ActivateGlow();

            else
                cards[i].CardVisuals.DeactivateGlow();
        }
    }

    public void ActiveDeckCardsGlow(CardUI selectedCard)
    {
        for (int i = 0; i < cardUI.CardsUI.Count; i++)
        {
            if(selectedCard!=cardUI.CardsUI[i])
                cardUI.CardsUI[i].CardVisuals.DeactivateGlow();
        }
    }

    public void DeactiveDeckCards(CardUI selectedCard)
    {
        if (StaminaHandler.Instance.IsEnoughStamina(true, selectedCard.CardData))
            selectedCard.CardVisuals.ActivateGlow();
            
        for (int i = 0; i < cardUI.CardsUI.Count; i++)
        {
            if (StaminaHandler.Instance.IsEnoughStamina(true, cardUI.CardsUI[i].CardData))
                cardUI.CardsUI[i].CardVisuals.ActivateGlow();
        }

    }

    private void OnDestroy()
    {
        HandUI.OnCardsAddToHand -= CheckCardToGlow;
        HandUI.OnCardsExecuteGetCards -= CheckCardGlowAfterExecute;
        HandUI.OnCardSelect -= ActiveDeckCardsGlow;
        HandUI.OnCardSelect -= DeactiveDeckCards;
    }
}
