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

    private void Awake()
    {
        HandUI.OnCardsAddToHand += CheckCardToGlow;
        HandUI.OnCardsExecuteGetCards += CheckCardGlowAfterExecute;
        HandUI.OnCardSelect += DeactiveDeckCards;
        HandUI.OnCardSelect += ActiveDeckCardsGlow;
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

    public void DeactiveDeckCards(CardUI cards)
    {
        if (StaminaHandler.Instance.IsEnoughStamina(true, cards.CardData))
            cards.CardVisuals.ActivateGlow();

        else
            cards.CardVisuals.DeactivateGlow();

    }

    public void ActiveDeckCardsGlow(CardUI cards)
    {
    }

    private void OnDestroy()
    {
        HandUI.OnCardsAddToHand -= CheckCardToGlow;
        HandUI.OnCardsExecuteGetCards -= CheckCardGlowAfterExecute;
        HandUI.OnCardSelect -= DeactiveDeckCards;
        HandUI.OnCardSelect -= ActiveDeckCardsGlow;
    }
}
